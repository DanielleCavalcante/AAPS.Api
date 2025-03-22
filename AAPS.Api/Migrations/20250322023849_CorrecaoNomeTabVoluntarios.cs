using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class CorrecaoNomeTabVoluntarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adocoes_Voluntario_VoluntarioId",
                schema: "dbo",
                table: "Adocoes");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_Voluntario_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_Voluntario_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_Voluntario_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_Voluntario_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Voluntario",
                schema: "dbo",
                table: "Voluntario");

            migrationBuilder.RenameTable(
                name: "Voluntario",
                schema: "dbo",
                newName: "Voluntarios",
                newSchema: "dbo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Voluntarios",
                schema: "dbo",
                table: "Voluntarios",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Adocoes_Voluntarios_VoluntarioId",
                schema: "dbo",
                table: "Adocoes",
                column: "VoluntarioId",
                principalSchema: "dbo",
                principalTable: "Voluntarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_Voluntarios_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Voluntarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_Voluntarios_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Voluntarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Voluntarios_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Voluntarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_Voluntarios_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Voluntarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adocoes_Voluntarios_VoluntarioId",
                schema: "dbo",
                table: "Adocoes");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_Voluntarios_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_Voluntarios_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_Voluntarios_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_Voluntarios_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Voluntarios",
                schema: "dbo",
                table: "Voluntarios");

            migrationBuilder.RenameTable(
                name: "Voluntarios",
                schema: "dbo",
                newName: "Voluntario",
                newSchema: "dbo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Voluntario",
                schema: "dbo",
                table: "Voluntario",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Adocoes_Voluntario_VoluntarioId",
                schema: "dbo",
                table: "Adocoes",
                column: "VoluntarioId",
                principalSchema: "dbo",
                principalTable: "Voluntario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_Voluntario_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Voluntario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_Voluntario_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Voluntario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Voluntario_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Voluntario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_Voluntario_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Voluntario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
