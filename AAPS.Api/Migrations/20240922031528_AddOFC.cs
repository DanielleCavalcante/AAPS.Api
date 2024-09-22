using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddOFC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "Doador",
                newName: "Doador",
                newSchema: "dbo");

            migrationBuilder.AlterColumn<string>(
                name: "Uf",
                schema: "dbo",
                table: "Doador",
                type: "nvarchar(2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Rg",
                schema: "dbo",
                table: "Doador",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                schema: "dbo",
                table: "Doador",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Logradouro",
                schema: "dbo",
                table: "Doador",
                type: "nvarchar(150)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                schema: "dbo",
                table: "Doador",
                type: "nvarchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Complemento",
                schema: "dbo",
                table: "Doador",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Cidade",
                schema: "dbo",
                table: "Doador",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Bairro",
                schema: "dbo",
                table: "Doador",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
                name: "Usuario",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(60)", nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(11)", nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    Nivel = table.Column<byte>(type: "tinyint", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
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
                name: "Adocao",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataAdocao = table.Column<DateTime>(type: "date", nullable: false),
                    AdotanteId = table.Column<int>(type: "int", nullable: false),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_Adocao_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "dbo",
                        principalTable: "Usuario",
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
                name: "IX_Adocao_UsuarioId",
                schema: "dbo",
                table: "Adocao",
                column: "UsuarioId");

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
                name: "Telefone",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Usuario",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Animal",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Evento",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Adotante",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PontoAdocao",
                schema: "dbo");

            migrationBuilder.RenameTable(
                name: "Doador",
                schema: "dbo",
                newName: "Doador");

            migrationBuilder.AlterColumn<string>(
                name: "Uf",
                table: "Doador",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2)");

            migrationBuilder.AlterColumn<string>(
                name: "Rg",
                table: "Doador",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Doador",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "Logradouro",
                table: "Doador",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)");

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Doador",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "Complemento",
                table: "Doador",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "Cidade",
                table: "Doador",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "Bairro",
                table: "Doador",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");
        }
    }
}
