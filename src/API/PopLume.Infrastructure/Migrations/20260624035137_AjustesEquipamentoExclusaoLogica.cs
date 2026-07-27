using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopLume.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjustesEquipamentoExclusaoLogica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataExclusao",
                table: "Equipamento",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Excluido",
                table: "Equipamento",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ExpectativaVida",
                table: "Equipamento",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorCompra",
                table: "Equipamento",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataExclusao",
                table: "Equipamento");

            migrationBuilder.DropColumn(
                name: "Excluido",
                table: "Equipamento");

            migrationBuilder.DropColumn(
                name: "ExpectativaVida",
                table: "Equipamento");

            migrationBuilder.DropColumn(
                name: "ValorCompra",
                table: "Equipamento");
        }
    }
}
