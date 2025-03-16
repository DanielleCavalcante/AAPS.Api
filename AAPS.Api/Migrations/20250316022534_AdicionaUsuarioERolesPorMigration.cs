using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaUsuarioERolesPorMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Observacao",
                schema: "dbo",
                table: "AnimalEvento",
                type: "nvarchar(500)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldNullable: true);

            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT AspNetRoles ON;

                IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Id = 1)
                BEGIN
                    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
                    VALUES (1, 'Admin', 'ADMIN', NEWID());
                END

                IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Id = 2)
                BEGIN
                    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
                    VALUES (2, 'Padrao', 'PADRAO', NEWID());
                END

                SET IDENTITY_INSERT AspNetRoles OFF;
            ");

            var passwordHasher = new PasswordHasher<IdentityUser>();
            string adminPasswordHash = passwordHasher.HashPassword(null, "Admin@123");

            migrationBuilder.Sql($@"
                -- Inserir voluntário (Admin)
                SET IDENTITY_INSERT dbo.Voluntario ON;

                IF NOT EXISTS (SELECT 1 FROM dbo.Voluntario WHERE Id = 1)
                BEGIN
                    INSERT INTO dbo.Voluntario 
                        (Id, UserName, NormalizedUserName, Email, PasswordHash, SecurityStamp, PhoneNumber, NomeCompleto, Cpf, Status)
                    VALUES 
                        (1, 'admin', 'ADMIN', 'aaps.sorocaba@gmail.com', 
                        '{adminPasswordHash}', 
                        NEWID(), '999999999', 'Administrador', '12345678901', 1);
                END

                SET IDENTITY_INSERT dbo.Voluntario OFF;
            ");

            migrationBuilder.Sql(@"
                -- Associar o voluntário Admin à role Admin
                IF NOT EXISTS (SELECT 1 FROM AspNetUserRoles WHERE UserId = 1 AND RoleId = 1)
                BEGIN
                    INSERT INTO AspNetUserRoles (UserId, RoleId)
                    VALUES (1, 1);
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Observacao",
                schema: "dbo",
                table: "AnimalEvento",
                type: "nvarchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)");

            migrationBuilder.Sql("DELETE FROM AspNetUserRoles WHERE UserId = 1;");
            migrationBuilder.Sql("DELETE FROM dbo.Voluntario WHERE Id = 1;");
            migrationBuilder.Sql("DELETE FROM AspNetRoles WHERE Id = 1;");
        }
    }
}
