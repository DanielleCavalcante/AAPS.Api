using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAdminPorMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- Criar Pessoa
                SET IDENTITY_INSERT Pessoas ON;

                IF NOT EXISTS (SELECT 1 FROM Pessoas WHERE Id = 1)
                BEGIN
                    INSERT INTO Pessoas (Id, Nome, Status, Tipo)
                    VALUES (1, 'Administrador', 1, 4);
                END

                SET IDENTITY_INSERT Pessoas OFF;
            ");

            var passwordHasher = new PasswordHasher<IdentityUser>();
            string adminPasswordHash = passwordHasher.HashPassword(null, "Admin@123");

            migrationBuilder.Sql($@"
                -- Criar Voluntário e associar à Pessoa
                SET IDENTITY_INSERT dbo.Voluntarios ON;

                IF NOT EXISTS (SELECT 1 FROM dbo.Voluntarios WHERE Id = 1)
                BEGIN
                    INSERT INTO dbo.Voluntarios 
                        (Id, UserName, NormalizedUserName, Email, PasswordHash, SecurityStamp, PessoaId)
                    VALUES 
                        (1, 'admin', 'ADMIN', 'aaps.sorocaba@gmail.com', 
                        '{adminPasswordHash}', 
                        NEWID(), 1);
                END

                SET IDENTITY_INSERT dbo.Voluntarios OFF;
            ");

            migrationBuilder.Sql(@"
                -- Criar Roles
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

            migrationBuilder.Sql(@"
                -- Associar Voluntário Admin à Role Admin
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
            migrationBuilder.Sql("DELETE FROM AspNetUserRoles WHERE UserId = 1;");
            migrationBuilder.Sql("DELETE FROM dbo.Voluntarios WHERE Id = 1;");
            migrationBuilder.Sql("DELETE FROM Pessoas WHERE Id = 1;");
            migrationBuilder.Sql("DELETE FROM AspNetRoles WHERE Id = 1;");
        }
    }
}
