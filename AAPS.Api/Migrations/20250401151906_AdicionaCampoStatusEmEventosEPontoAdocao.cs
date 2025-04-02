using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaCampoStatusEmEventosEPontoAdocao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "dbo",
                table: "PontosAdocao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "dbo",
                table: "Eventos",
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
                table: "PontosAdocao");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "dbo",
                table: "Eventos");
        }
    }
}
