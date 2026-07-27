using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopLume.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CriandoMarketPlaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "DataCompra",
                table: "Equipamento",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateTable(
                name: "Marketplace",
                columns: table => new
                {
                    IdMarketplace = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LinkLoja = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Excluido = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DataExclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marketplace", x => x.IdMarketplace);
                });

            migrationBuilder.CreateTable(
                name: "TaxasMarketplace",
                columns: table => new
                {
                    IdTaxa = table.Column<Guid>(type: "uuid", nullable: false),
                    IdMarketplace = table.Column<Guid>(type: "uuid", nullable: false),
                    ValorInicial = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    ValorFinal = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Comissao = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    TaxaFixa = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxasMarketplace", x => x.IdTaxa);
                    table.ForeignKey(
                        name: "FK_TaxasMarketplace_Marketplace_IdMarketplace",
                        column: x => x.IdMarketplace,
                        principalTable: "Marketplace",
                        principalColumn: "IdMarketplace",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxasMarketplace_IdMarketplace",
                table: "TaxasMarketplace",
                column: "IdMarketplace");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxasMarketplace");

            migrationBuilder.DropTable(
                name: "Marketplace");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCompra",
                table: "Equipamento",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
