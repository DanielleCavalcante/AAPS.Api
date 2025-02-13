using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaColunasNaTabelaVoluntarioEConfiguraRolesDoIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nivel",
                schema: "dbo",
                table: "Voluntario");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                schema: "dbo",
                table: "Voluntario",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "IdentityRoleId",
                schema: "dbo",
                table: "Voluntario",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                schema: "dbo",
                table: "Voluntario",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "RedefinirSenha",
                schema: "dbo",
                table: "Voluntario",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Voluntario_IdentityUserId",
                schema: "dbo",
                table: "Voluntario",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Voluntario_AspNetUsers_IdentityUserId",
                schema: "dbo",
                table: "Voluntario",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voluntario_AspNetUsers_IdentityUserId",
                schema: "dbo",
                table: "Voluntario");

            migrationBuilder.DropIndex(
                name: "IX_Voluntario_IdentityUserId",
                schema: "dbo",
                table: "Voluntario");

            migrationBuilder.DropColumn(
                name: "IdentityRoleId",
                schema: "dbo",
                table: "Voluntario");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                schema: "dbo",
                table: "Voluntario");

            migrationBuilder.DropColumn(
                name: "RedefinirSenha",
                schema: "dbo",
                table: "Voluntario");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                schema: "dbo",
                table: "Voluntario",
                type: "bit",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AddColumn<byte>(
                name: "Nivel",
                schema: "dbo",
                table: "Voluntario",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
