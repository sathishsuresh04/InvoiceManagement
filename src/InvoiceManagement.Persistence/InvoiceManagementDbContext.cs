using System.Reflection;
using InvoiceManagement.Application.Abstractions.Common;
using InvoiceManagement.Application.Abstractions.Data;
using InvoiceManagement.Application.Core.Extensions;
using InvoiceManagement.Domain.Abstractions;
using InvoiceManagement.Domain.Invoices;
using InvoiceManagement.Domain.Primitives;
using InvoiceManagement.Persistence.Common;
using InvoiceManagement.Persistence.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace InvoiceManagement.Persistence;

public class InvoiceManagementDbContext(
    DbContextOptions options,
    IConfiguration configuration,
    IDateTime dateTime,
    IMediator mediator
)
    : DbContext(options), IDbContext, IUnitOfWork
{
    private readonly PostgresDbOptions _postgresDbOptions =
        configuration.GetOptions<PostgresDbOptions>(nameof(PostgresDbOptions));

    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();

    /// <inheritdoc />
    public new DbSet<TEntity> Set<TEntity>()
    where TEntity : Entity
    {
        return base.Set<TEntity>();
    }


    /// <inheritdoc />
    public async Task<TEntity?> GetBydIdAsync<TEntity>(Guid id)
    where TEntity : Entity
    {
        return id == Guid.Empty ?
                   null :
                   await Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
    }

    /// <inheritdoc />
    public void Insert<TEntity>(TEntity entity)
    where TEntity : Entity
    {
        Set<TEntity>().Add(entity);
    }


    /// <inheritdoc />
    public void InsertRange<TEntity>(IReadOnlyCollection<TEntity> entities)
    where TEntity : Entity
    {
        Set<TEntity>().AddRange(entities);
    }


    /// <inheritdoc />
    public new void Remove<TEntity>(TEntity entity)
    where TEntity : Entity
    {
        Set<TEntity>().Remove(entity);
    }

    /// <summary>
    ///     Saves all the pending changes in the unit of work.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of entities that have been saved.</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var utcNow = dateTime.UtcNow;

        UpdateAuditableEntities(utcNow);

        UpdateSoftDeletableEntities(utcNow);

        await PublishDomainEventsAsync(cancellationToken);

        return await base.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Database.BeginTransactionAsync(cancellationToken);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (!string.IsNullOrWhiteSpace(_postgresDbOptions.DefaultSchema))
            modelBuilder.HasDefaultSchema(_postgresDbOptions.DefaultSchema);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.ApplyUtcDateTimeConverter();

        base.OnModelCreating(modelBuilder);
    }

    private void UpdateAuditableEntities(DateTime utcNow)
    {
        foreach (var entityEntry in ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entityEntry.State == EntityState.Added)
                entityEntry.Property(nameof(IAuditableEntity.CreatedOnUtc)).CurrentValue = utcNow;

            if (entityEntry.State == EntityState.Modified)
                entityEntry.Property(nameof(IAuditableEntity.ModifiedOnUtc)).CurrentValue = utcNow;
        }
    }

    private void UpdateSoftDeletableEntities(DateTime utcNow)
    {
        foreach (var entityEntry in ChangeTracker.Entries<ISoftDeletableEntity>())
        {
            if (entityEntry.State != EntityState.Deleted) continue;

            entityEntry.Property(nameof(ISoftDeletableEntity.DeletedOnUtc)).CurrentValue = utcNow;

            entityEntry.Property(nameof(ISoftDeletableEntity.Deleted)).CurrentValue = true;

            entityEntry.State = EntityState.Modified;

            UpdateDeletedEntityEntryReferencesToUnchanged(entityEntry);
        }
    }

    private static void UpdateDeletedEntityEntryReferencesToUnchanged(EntityEntry entityEntry)
    {
        if (!entityEntry.References.Any()) return;

        foreach (var referenceEntry in entityEntry.References.Where(
                     r => r.TargetEntry is
                          {
                              State: EntityState.Deleted,
                          }))
        {
            if (referenceEntry.TargetEntry == null) continue;
            referenceEntry.TargetEntry.State = EntityState.Unchanged;

            UpdateDeletedEntityEntryReferencesToUnchanged(referenceEntry.TargetEntry);
        }
    }

    private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
    {
        var aggregateRoots = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(entityEntry => entityEntry.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = aggregateRoots.SelectMany(entityEntry => entityEntry.Entity.DomainEvents).ToList();

        aggregateRoots.ForEach(entityEntry => entityEntry.Entity.ClearDomainEvents());

        var tasks = domainEvents.Select(domainEvent => mediator.Publish(domainEvent, cancellationToken));

        await Task.WhenAll(tasks);
    }
}
