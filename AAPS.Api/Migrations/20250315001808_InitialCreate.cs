using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Adotante",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(60)", nullable: false),
                    Rg = table.Column<string>(type: "nvarchar(9)", nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(11)", nullable: false),
                    LocalTrabalho = table.Column<string>(type: "nvarchar(80)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Facebook = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Instagram = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Logradouro = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Complemento = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Bairro = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Uf = table.Column<string>(type: "nvarchar(2)", nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Cep = table.Column<int>(type: "int", nullable: false),
                    SituacaoEndereco = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adotante", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Doador",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Rg = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Logradouro = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Complemento = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Bairro = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Uf = table.Column<string>(type: "nvarchar(2)", nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Cep = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Evento",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PontoAdocao",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeFantasia = table.Column<string>(type: "nvarchar(60)", nullable: false),
                    Responsavel = table.Column<string>(type: "nvarchar(60)", nullable: false),
                    Cnpj = table.Column<string>(type: "nvarchar(14)", nullable: false),
                    Logradouro = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Complemento = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Bairro = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Uf = table.Column<string>(type: "nvarchar(2)", nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Cep = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PontoAdocao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Voluntario",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeCompleto = table.Column<string>(type: "nvarchar(60)", nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(11)", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voluntario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Animal",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    Especie = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    Raca = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    Pelagem = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    Sexo = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "date", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    DoadorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animal_Doador_DoadorId",
                        column: x => x.DoadorId,
                        principalSchema: "dbo",
                        principalTable: "Doador",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Telefone",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroTelefone = table.Column<string>(type: "nvarchar(11)", nullable: false),
                    Responsavel = table.Column<string>(type: "nvarchar(60)", nullable: false),
                    AdotanteId = table.Column<int>(type: "int", nullable: false),
                    DoadorId = table.Column<int>(type: "int", nullable: false),
                    PontoAdocaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telefone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Telefone_Adotante_AdotanteId",
                        column: x => x.AdotanteId,
                        principalSchema: "dbo",
                        principalTable: "Adotante",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Telefone_Doador_DoadorId",
                        column: x => x.DoadorId,
                        principalSchema: "dbo",
                        principalTable: "Doador",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Telefone_PontoAdocao_PontoAdocaoId",
                        column: x => x.PontoAdocaoId,
                        principalSchema: "dbo",
                        principalTable: "PontoAdocao",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Voluntario_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Voluntario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Voluntario_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Voluntario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Voluntario_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Voluntario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Voluntario_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Voluntario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Adocao",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataAdocao = table.Column<DateTime>(type: "date", nullable: false),
                    AdotanteId = table.Column<int>(type: "int", nullable: false),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    VoluntarioId = table.Column<int>(type: "int", nullable: false),
                    PontoAdocaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adocao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Adocao_Adotante_AdotanteId",
                        column: x => x.AdotanteId,
                        principalSchema: "dbo",
                        principalTable: "Adotante",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Adocao_Animal_AnimalId",
                        column: x => x.AnimalId,
                        principalSchema: "dbo",
                        principalTable: "Animal",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Adocao_PontoAdocao_PontoAdocaoId",
                        column: x => x.PontoAdocaoId,
                        principalSchema: "dbo",
                        principalTable: "PontoAdocao",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Adocao_Voluntario_VoluntarioId",
                        column: x => x.VoluntarioId,
                        principalSchema: "dbo",
                        principalTable: "Voluntario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnimalEvento",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataAcompanhamento = table.Column<DateTime>(type: "date", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    EventoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalEvento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalEvento_Animal_AnimalId",
                        column: x => x.AnimalId,
                        principalSchema: "dbo",
                        principalTable: "Animal",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnimalEvento_Evento_EventoId",
                        column: x => x.EventoId,
                        principalSchema: "dbo",
                        principalTable: "Evento",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adocao_AdotanteId",
                schema: "dbo",
                table: "Adocao",
                column: "AdotanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Adocao_AnimalId",
                schema: "dbo",
                table: "Adocao",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Adocao_PontoAdocaoId",
                schema: "dbo",
                table: "Adocao",
                column: "PontoAdocaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Adocao_VoluntarioId",
                schema: "dbo",
                table: "Adocao",
                column: "VoluntarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Animal_DoadorId",
                schema: "dbo",
                table: "Animal",
                column: "DoadorId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalEvento_AnimalId",
                schema: "dbo",
                table: "AnimalEvento",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalEvento_EventoId",
                schema: "dbo",
                table: "AnimalEvento",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Telefone_AdotanteId",
                schema: "dbo",
                table: "Telefone",
                column: "AdotanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Telefone_DoadorId",
                schema: "dbo",
                table: "Telefone",
                column: "DoadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Telefone_PontoAdocaoId",
                schema: "dbo",
                table: "Telefone",
                column: "PontoAdocaoId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "dbo",
                table: "Voluntario",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "dbo",
                table: "Voluntario",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adocao",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AnimalEvento",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Telefone",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Animal",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Evento",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Voluntario",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Adotante",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PontoAdocao",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Doador",
                schema: "dbo");
        }
    }
}
