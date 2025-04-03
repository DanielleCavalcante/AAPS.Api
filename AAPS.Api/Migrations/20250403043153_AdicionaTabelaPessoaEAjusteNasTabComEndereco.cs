using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaTabelaPessoaEAjusteNasTabComEndereco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animais_Doadores_DoadorId",
                schema: "dbo",
                table: "Animais");

            migrationBuilder.DropForeignKey(
                name: "FK_Telefones_Adotantes_AdotanteId",
                schema: "dbo",
                table: "Telefones");

            migrationBuilder.DropForeignKey(
                name: "FK_Telefones_Doadores_DoadorId",
                schema: "dbo",
                table: "Telefones");

            migrationBuilder.DropForeignKey(
                name: "FK_Telefones_PontosAdocao_PontoAdocaoId",
                schema: "dbo",
                table: "Telefones");

            migrationBuilder.DropTable(
                name: "AnimalEvento",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Doadores",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Telefones_AdotanteId",
                schema: "dbo",
                table: "Telefones");

            migrationBuilder.DropIndex(
                name: "IX_Telefones_DoadorId",
                schema: "dbo",
                table: "Telefones");

            migrationBuilder.DropColumn(
                name: "Cpf",
                schema: "dbo",
                table: "Voluntarios");

            migrationBuilder.DropColumn(
                name: "Nome",
                schema: "dbo",
                table: "Voluntarios");

            migrationBuilder.DropColumn(
                name: "AdotanteId",
                schema: "dbo",
                table: "Telefones");

            migrationBuilder.DropColumn(
                name: "DoadorId",
                schema: "dbo",
                table: "Telefones");

            migrationBuilder.DropColumn(
                name: "Responsavel",
                schema: "dbo",
                table: "Telefones");

            migrationBuilder.DropColumn(
                name: "Bairro",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropColumn(
                name: "Cep",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropColumn(
                name: "Cidade",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropColumn(
                name: "Complemento",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropColumn(
                name: "Logradouro",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropColumn(
                name: "Numero",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropColumn(
                name: "Uf",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropColumn(
                name: "Bairro",
                schema: "dbo",
                table: "Adotantes");

            migrationBuilder.DropColumn(
                name: "Cep",
                schema: "dbo",
                table: "Adotantes");

            migrationBuilder.DropColumn(
                name: "Cidade",
                schema: "dbo",
                table: "Adotantes");

            migrationBuilder.DropColumn(
                name: "Complemento",
                schema: "dbo",
                table: "Adotantes");

            migrationBuilder.DropColumn(
                name: "Cpf",
                schema: "dbo",
                table: "Adotantes");

            migrationBuilder.DropColumn(
                name: "Logradouro",
                schema: "dbo",
                table: "Adotantes");

            migrationBuilder.DropColumn(
                name: "Nome",
                schema: "dbo",
                table: "Adotantes");

            migrationBuilder.DropColumn(
                name: "Numero",
                schema: "dbo",
                table: "Adotantes");

            migrationBuilder.DropColumn(
                name: "Rg",
                schema: "dbo",
                table: "Adotantes");

            migrationBuilder.DropColumn(
                name: "SituacaoEndereco",
                schema: "dbo",
                table: "Adotantes");

            migrationBuilder.DropColumn(
                name: "Uf",
                schema: "dbo",
                table: "Adotantes");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "dbo",
                table: "Voluntarios",
                newName: "PessoaId");

            migrationBuilder.RenameColumn(
                name: "PontoAdocaoId",
                schema: "dbo",
                table: "Telefones",
                newName: "PessoaId");

            migrationBuilder.RenameIndex(
                name: "IX_Telefones_PontoAdocaoId",
                schema: "dbo",
                table: "Telefones",
                newName: "IX_Telefones_PessoaId");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "dbo",
                table: "PontosAdocao",
                newName: "PessoaId");

            migrationBuilder.RenameColumn(
                name: "DoadorId",
                schema: "dbo",
                table: "Animais",
                newName: "PessoaId");

            migrationBuilder.RenameIndex(
                name: "IX_Animais_DoadorId",
                schema: "dbo",
                table: "Animais",
                newName: "IX_Animais_PessoaId");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "dbo",
                table: "Adotantes",
                newName: "PessoaId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataNascimento",
                schema: "dbo",
                table: "Animais",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.CreateTable(
                name: "Acompanhamentos",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateTime>(type: "date", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(600)", nullable: true),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    EventoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acompanhamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Acompanhamentos_Animais_AnimalId",
                        column: x => x.AnimalId,
                        principalSchema: "dbo",
                        principalTable: "Animais",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Acompanhamentos_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalSchema: "dbo",
                        principalTable: "Eventos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Pessoas",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(60)", nullable: true),
                    Rg = table.Column<string>(type: "nvarchar(9)", nullable: true),
                    Cpf = table.Column<string>(type: "nvarchar(11)", nullable: true),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Enderecos",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Logradouro = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Complemento = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Bairro = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Uf = table.Column<string>(type: "char(2)", nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Cep = table.Column<string>(type: "nvarchar(8)", nullable: false),
                    SituacaoEndereco = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    PessoaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enderecos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enderecos_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalSchema: "dbo",
                        principalTable: "Pessoas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Voluntarios_PessoaId",
                schema: "dbo",
                table: "Voluntarios",
                column: "PessoaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PontosAdocao_PessoaId",
                schema: "dbo",
                table: "PontosAdocao",
                column: "PessoaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adotantes_PessoaId",
                schema: "dbo",
                table: "Adotantes",
                column: "PessoaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Acompanhamentos_AnimalId",
                schema: "dbo",
                table: "Acompanhamentos",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Acompanhamentos_EventoId",
                schema: "dbo",
                table: "Acompanhamentos",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_PessoaId",
                schema: "dbo",
                table: "Enderecos",
                column: "PessoaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Adotantes_Pessoas_PessoaId",
                schema: "dbo",
                table: "Adotantes",
                column: "PessoaId",
                principalSchema: "dbo",
                principalTable: "Pessoas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Animais_Pessoas_PessoaId",
                schema: "dbo",
                table: "Animais",
                column: "PessoaId",
                principalSchema: "dbo",
                principalTable: "Pessoas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PontosAdocao_Pessoas_PessoaId",
                schema: "dbo",
                table: "PontosAdocao",
                column: "PessoaId",
                principalSchema: "dbo",
                principalTable: "Pessoas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Telefones_Pessoas_PessoaId",
                schema: "dbo",
                table: "Telefones",
                column: "PessoaId",
                principalSchema: "dbo",
                principalTable: "Pessoas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Voluntarios_Pessoas_PessoaId",
                schema: "dbo",
                table: "Voluntarios",
                column: "PessoaId",
                principalSchema: "dbo",
                principalTable: "Pessoas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adotantes_Pessoas_PessoaId",
                schema: "dbo",
                table: "Adotantes");

            migrationBuilder.DropForeignKey(
                name: "FK_Animais_Pessoas_PessoaId",
                schema: "dbo",
                table: "Animais");

            migrationBuilder.DropForeignKey(
                name: "FK_PontosAdocao_Pessoas_PessoaId",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropForeignKey(
                name: "FK_Telefones_Pessoas_PessoaId",
                schema: "dbo",
                table: "Telefones");

            migrationBuilder.DropForeignKey(
                name: "FK_Voluntarios_Pessoas_PessoaId",
                schema: "dbo",
                table: "Voluntarios");

            migrationBuilder.DropTable(
                name: "Acompanhamentos",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Enderecos",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Pessoas",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Voluntarios_PessoaId",
                schema: "dbo",
                table: "Voluntarios");

            migrationBuilder.DropIndex(
                name: "IX_PontosAdocao_PessoaId",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropIndex(
                name: "IX_Adotantes_PessoaId",
                schema: "dbo",
                table: "Adotantes");

            migrationBuilder.RenameColumn(
                name: "PessoaId",
                schema: "dbo",
                table: "Voluntarios",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "PessoaId",
                schema: "dbo",
                table: "Telefones",
                newName: "PontoAdocaoId");

            migrationBuilder.RenameIndex(
                name: "IX_Telefones_PessoaId",
                schema: "dbo",
                table: "Telefones",
                newName: "IX_Telefones_PontoAdocaoId");

            migrationBuilder.RenameColumn(
                name: "PessoaId",
                schema: "dbo",
                table: "PontosAdocao",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "PessoaId",
                schema: "dbo",
                table: "Animais",
                newName: "DoadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Animais_PessoaId",
                schema: "dbo",
                table: "Animais",
                newName: "IX_Animais_DoadorId");

            migrationBuilder.RenameColumn(
                name: "PessoaId",
                schema: "dbo",
                table: "Adotantes",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                schema: "dbo",
                table: "Voluntarios",
                type: "nvarchar(11)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                schema: "dbo",
                table: "Voluntarios",
                type: "nvarchar(60)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AdotanteId",
                schema: "dbo",
                table: "Telefones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DoadorId",
                schema: "dbo",
                table: "Telefones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Responsavel",
                schema: "dbo",
                table: "Telefones",
                type: "nvarchar(60)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                schema: "dbo",
                table: "PontosAdocao",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Cep",
                schema: "dbo",
                table: "PontosAdocao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                schema: "dbo",
                table: "PontosAdocao",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                schema: "dbo",
                table: "PontosAdocao",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Logradouro",
                schema: "dbo",
                table: "PontosAdocao",
                type: "nvarchar(150)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Numero",
                schema: "dbo",
                table: "PontosAdocao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Uf",
                schema: "dbo",
                table: "PontosAdocao",
                type: "nvarchar(2)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataNascimento",
                schema: "dbo",
                table: "Animais",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                schema: "dbo",
                table: "Adotantes",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Cep",
                schema: "dbo",
                table: "Adotantes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                schema: "dbo",
                table: "Adotantes",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                schema: "dbo",
                table: "Adotantes",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                schema: "dbo",
                table: "Adotantes",
                type: "nvarchar(11)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Logradouro",
                schema: "dbo",
                table: "Adotantes",
                type: "nvarchar(150)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                schema: "dbo",
                table: "Adotantes",
                type: "nvarchar(60)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Numero",
                schema: "dbo",
                table: "Adotantes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Rg",
                schema: "dbo",
                table: "Adotantes",
                type: "nvarchar(9)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SituacaoEndereco",
                schema: "dbo",
                table: "Adotantes",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Uf",
                schema: "dbo",
                table: "Adotantes",
                type: "nvarchar(2)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AnimalEvento",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "date", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(600)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalEvento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalEvento_Animais_AnimalId",
                        column: x => x.AnimalId,
                        principalSchema: "dbo",
                        principalTable: "Animais",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnimalEvento_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalSchema: "dbo",
                        principalTable: "Eventos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Doadores",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bairro = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Cep = table.Column<int>(type: "int", nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Complemento = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Logradouro = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Rg = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Uf = table.Column<string>(type: "nvarchar(2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doadores", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Telefones_AdotanteId",
                schema: "dbo",
                table: "Telefones",
                column: "AdotanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Telefones_DoadorId",
                schema: "dbo",
                table: "Telefones",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Animais_Doadores_DoadorId",
                schema: "dbo",
                table: "Animais",
                column: "DoadorId",
                principalSchema: "dbo",
                principalTable: "Doadores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Telefones_Adotantes_AdotanteId",
                schema: "dbo",
                table: "Telefones",
                column: "AdotanteId",
                principalSchema: "dbo",
                principalTable: "Adotantes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Telefones_Doadores_DoadorId",
                schema: "dbo",
                table: "Telefones",
                column: "DoadorId",
                principalSchema: "dbo",
                principalTable: "Doadores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Telefones_PontosAdocao_PontoAdocaoId",
                schema: "dbo",
                table: "Telefones",
                column: "PontoAdocaoId",
                principalSchema: "dbo",
                principalTable: "PontosAdocao",
                principalColumn: "Id");
        }
    }
}
