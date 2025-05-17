using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AjusteRelacinamentoPontoAdocaoPessoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pessoas_PontosAdocao_PontoAdocaoId",
                schema: "dbo",
                table: "Pessoas");

            migrationBuilder.DropIndex(
                name: "IX_Pessoas_PontoAdocaoId",
                schema: "dbo",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "PontoAdocaoId",
                schema: "dbo",
                table: "Pessoas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PontoAdocaoId",
                schema: "dbo",
                table: "Pessoas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pessoas_PontoAdocaoId",
                schema: "dbo",
                table: "Pessoas",
                column: "PontoAdocaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pessoas_PontosAdocao_PontoAdocaoId",
                schema: "dbo",
                table: "Pessoas",
                column: "PontoAdocaoId",
                principalSchema: "dbo",
                principalTable: "PontosAdocao",
                principalColumn: "Id");
        }
    }
}
