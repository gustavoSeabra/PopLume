using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopLume.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CriandoDominioFilamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ValorHora",
                table: "Equipamento",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                computedColumnSql: "CASE WHEN \"ExpectativaVida\" > 0 THEN \"ValorCompra\" / \"ExpectativaVida\" ELSE 0 END",
                stored: true);

            migrationBuilder.CreateTable(
                name: "Filamento",
                columns: table => new
                {
                    IdFilamento = table.Column<Guid>(type: "uuid", nullable: false),
                    Cor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Peso = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    DataCompra = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filamento", x => x.IdFilamento);
                    table.CheckConstraint("CK_Filamento_Tipo", "\"Tipo\" IN ('ABS', 'PETG', 'PLA', 'TPU')");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Filamento");

            migrationBuilder.DropColumn(
                name: "ValorHora",
                table: "Equipamento");
        }
    }
}
