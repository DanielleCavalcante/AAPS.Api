using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaCampoObsEmAdotanteEResponsavelContatoEmPessoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Contato2",
                schema: "dbo",
                table: "PontosAdocao",
                newName: "Contato");

            migrationBuilder.RenameColumn(
                name: "Contato1",
                schema: "dbo",
                table: "PontosAdocao",
                newName: "Celular");

            migrationBuilder.RenameColumn(
                name: "Contato2",
                schema: "dbo",
                table: "Pessoas",
                newName: "Contato");

            migrationBuilder.RenameColumn(
                name: "Contato1",
                schema: "dbo",
                table: "Pessoas",
                newName: "Celular");

            migrationBuilder.AddColumn<string>(
                name: "ResponsavelContato",
                schema: "dbo",
                table: "PontosAdocao",
                type: "nvarchar(60)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(60)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(11)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsavelContato",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(60)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ObservacaoBloqueio",
                schema: "dbo",
                table: "Adotantes",
                type: "nvarchar(500)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponsavelContato",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropColumn(
                name: "ResponsavelContato",
                schema: "dbo",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "ObservacaoBloqueio",
                schema: "dbo",
                table: "Adotantes");

            migrationBuilder.RenameColumn(
                name: "Contato",
                schema: "dbo",
                table: "PontosAdocao",
                newName: "Contato2");

            migrationBuilder.RenameColumn(
                name: "Celular",
                schema: "dbo",
                table: "PontosAdocao",
                newName: "Contato1");

            migrationBuilder.RenameColumn(
                name: "Contato",
                schema: "dbo",
                table: "Pessoas",
                newName: "Contato2");

            migrationBuilder.RenameColumn(
                name: "Celular",
                schema: "dbo",
                table: "Pessoas",
                newName: "Contato1");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(60)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)");

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(11)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)");
        }
    }
}
