using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaCampoStatusEBloqueioEDisponibilidadeEmTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "dbo",
                table: "Doadores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Disponibilidade",
                schema: "dbo",
                table: "Animais",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Bloqueio",
                schema: "dbo",
                table: "Adotantes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "dbo",
                table: "Doadores");

            migrationBuilder.DropColumn(
                name: "Disponibilidade",
                schema: "dbo",
                table: "Animais");

            migrationBuilder.DropColumn(
                name: "Bloqueio",
                schema: "dbo",
                table: "Adotantes");
        }
    }
}
