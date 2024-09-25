using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceManagementInitialDbMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "invoice_management");

            migrationBuilder.CreateTable(
                name: "invoice",
                schema: "invoice_management",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "varchar(500)", nullable: true),
                    number = table.Column<string>(type: "varchar(50)", nullable: false),
                    seller = table.Column<string>(type: "varchar(100)", nullable: false),
                    buyer = table.Column<string>(type: "varchar(100)", nullable: false),
                    issued_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    acceptance_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_on_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_on_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_on_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "invoice_item",
                schema: "invoice_management",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", nullable: false),
                    count = table.Column<long>(type: "bigint", nullable: false),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    invoice_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_on_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_item", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoice_item_invoice_invoice_id",
                        column: x => x.invoice_id,
                        principalSchema: "invoice_management",
                        principalTable: "invoice",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_invoice_item_invoice_id",
                schema: "invoice_management",
                table: "invoice_item",
                column: "invoice_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "invoice_item",
                schema: "invoice_management");

            migrationBuilder.DropTable(
                name: "invoice",
                schema: "invoice_management");
        }
    }
}
