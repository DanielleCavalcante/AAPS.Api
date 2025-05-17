using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeparaTabPontoAdocaoDePessoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PontosAdocao_Pessoas_PessoaId",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropIndex(
                name: "IX_PontosAdocao_PessoaId",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.RenameColumn(
                name: "PessoaId",
                schema: "dbo",
                table: "PontosAdocao",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                schema: "dbo",
                table: "PontosAdocao",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cep",
                schema: "dbo",
                table: "PontosAdocao",
                type: "nvarchar(8)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                schema: "dbo",
                table: "PontosAdocao",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                schema: "dbo",
                table: "PontosAdocao",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contato1",
                schema: "dbo",
                table: "PontosAdocao",
                type: "nvarchar(11)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Contato2",
                schema: "dbo",
                table: "PontosAdocao",
                type: "nvarchar(11)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Logradouro",
                schema: "dbo",
                table: "PontosAdocao",
                type: "nvarchar(150)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Numero",
                schema: "dbo",
                table: "PontosAdocao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Uf",
                schema: "dbo",
                table: "PontosAdocao",
                type: "char(2)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "Bairro",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropColumn(
                name: "Cep",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropColumn(
                name: "Cidade",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropColumn(
                name: "Complemento",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropColumn(
                name: "Contato1",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropColumn(
                name: "Contato2",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropColumn(
                name: "Logradouro",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropColumn(
                name: "Numero",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropColumn(
                name: "Uf",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropColumn(
                name: "PontoAdocaoId",
                schema: "dbo",
                table: "Pessoas");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "dbo",
                table: "PontosAdocao",
                newName: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_PontosAdocao_PessoaId",
                schema: "dbo",
                table: "PontosAdocao",
                column: "PessoaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PontosAdocao_Pessoas_PessoaId",
                schema: "dbo",
                table: "PontosAdocao",
                column: "PessoaId",
                principalSchema: "dbo",
                principalTable: "Pessoas",
                principalColumn: "Id");
        }
    }
}
