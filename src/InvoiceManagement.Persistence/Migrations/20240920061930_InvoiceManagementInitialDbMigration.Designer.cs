﻿// <auto-generated />
using System;
using InvoiceManagement.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InvoiceManagement.Persistence.Migrations
{
    [DbContext(typeof(InvoiceManagementDbContext))]
    [Migration("20240920061930_InvoiceManagementInitialDbMigration")]
    partial class InvoiceManagementInitialDbMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("invoice_management")
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("InvoiceManagement.Domain.Invoices.Invoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("AcceptanceDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("acceptance_date");

                    b.Property<string>("Buyer")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("buyer");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_on_utc");

                    b.Property<bool>("Deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("deleted");

                    b.Property<DateTime?>("DeletedOnUtc")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("deleted_on_utc");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(500)")
                        .HasColumnName("description");

                    b.Property<DateTime>("IssuedDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("issued_date");

                    b.Property<DateTime?>("ModifiedOnUtc")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("modified_on_utc");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("number");

                    b.Property<string>("Seller")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("seller");

                    b.HasKey("Id")
                        .HasName("pk_invoice");

                    b.ToTable("invoice", "invoice_management");
                });

            modelBuilder.Entity("InvoiceManagement.Domain.Invoices.InvoiceItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<long>("Count")
                        .HasColumnType("bigint")
                        .HasColumnName("count");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_on_utc");

                    b.Property<Guid>("InvoiceId")
                        .HasColumnType("uuid")
                        .HasColumnName("invoice_id");

                    b.Property<DateTime?>("ModifiedOnUtc")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("modified_on_utc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("price");

                    b.HasKey("Id")
                        .HasName("pk_invoice_item");

                    b.HasIndex("InvoiceId")
                        .HasDatabaseName("ix_invoice_item_invoice_id");

                    b.ToTable("invoice_item", "invoice_management");
                });

            modelBuilder.Entity("InvoiceManagement.Domain.Invoices.InvoiceItem", b =>
                {
                    b.HasOne("InvoiceManagement.Domain.Invoices.Invoice", null)
                        .WithMany("InvoiceItems")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_invoice_item_invoice_invoice_id");
                });

            modelBuilder.Entity("InvoiceManagement.Domain.Invoices.Invoice", b =>
                {
                    b.Navigation("InvoiceItems");
                });
#pragma warning restore 612, 618
        }
    }
}
