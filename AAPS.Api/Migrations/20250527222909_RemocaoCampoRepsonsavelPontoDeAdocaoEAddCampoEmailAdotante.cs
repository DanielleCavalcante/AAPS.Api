using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemocaoCampoRepsonsavelPontoDeAdocaoEAddCampoEmailAdotante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Responsavel",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "dbo",
                table: "Adotantes",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                schema: "dbo",
                table: "Adotantes");

            migrationBuilder.AddColumn<string>(
                name: "Responsavel",
                schema: "dbo",
                table: "PontosAdocao",
                type: "nvarchar(60)",
                nullable: false,
                defaultValue: "");
        }
    }
}
