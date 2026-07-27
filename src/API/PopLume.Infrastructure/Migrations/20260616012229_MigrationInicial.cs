using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopLume.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipamento",
                columns: table => new
                {
                    IdEquipamento = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Apelido = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DataCompra = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Potencia = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipamento", x => x.IdEquipamento);
                });

            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    IdProduto = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PrecoCusto = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    QuantidadeFilamento = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    TempoImpressao = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.IdProduto);
                });

            migrationBuilder.CreateTable(
                name: "Produto_Composicao",
                columns: table => new
                {
                    IdProdutoComposicao = table.Column<Guid>(type: "uuid", nullable: false),
                    IdProdutoPai = table.Column<Guid>(type: "uuid", nullable: true),
                    IdProdutoFilho = table.Column<Guid>(type: "uuid", nullable: true),
                    Quantidade = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto_Composicao", x => x.IdProdutoComposicao);
                    table.ForeignKey(
                        name: "FK_Produto_Composicao_Produto_IdProdutoFilho",
                        column: x => x.IdProdutoFilho,
                        principalTable: "Produto",
                        principalColumn: "IdProduto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Produto_Composicao_Produto_IdProdutoPai",
                        column: x => x.IdProdutoPai,
                        principalTable: "Produto",
                        principalColumn: "IdProduto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produto_Composicao_IdProdutoFilho",
                table: "Produto_Composicao",
                column: "IdProdutoFilho");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_Composicao_IdProdutoPai_IdProdutoFilho",
                table: "Produto_Composicao",
                columns: new[] { "IdProdutoPai", "IdProdutoFilho" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipamento");

            migrationBuilder.DropTable(
                name: "Produto_Composicao");

            migrationBuilder.DropTable(
                name: "Produto");
        }
    }
}
