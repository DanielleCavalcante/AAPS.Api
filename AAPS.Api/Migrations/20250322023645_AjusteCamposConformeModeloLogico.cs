using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AjusteCamposConformeModeloLogico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adocao_Adotante_AdotanteId",
                schema: "dbo",
                table: "Adocao");

            migrationBuilder.DropForeignKey(
                name: "FK_Adocao_Animal_AnimalId",
                schema: "dbo",
                table: "Adocao");

            migrationBuilder.DropForeignKey(
                name: "FK_Adocao_PontoAdocao_PontoAdocaoId",
                schema: "dbo",
                table: "Adocao");

            migrationBuilder.DropForeignKey(
                name: "FK_Adocao_Voluntario_VoluntarioId",
                schema: "dbo",
                table: "Adocao");

            migrationBuilder.DropForeignKey(
                name: "FK_Animal_Doador_DoadorId",
                schema: "dbo",
                table: "Animal");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimalEvento_Animal_AnimalId",
                schema: "dbo",
                table: "AnimalEvento");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimalEvento_Evento_EventoId",
                schema: "dbo",
                table: "AnimalEvento");

            migrationBuilder.DropForeignKey(
                name: "FK_Telefone_Adotante_AdotanteId",
                schema: "dbo",
                table: "Telefone");

            migrationBuilder.DropForeignKey(
                name: "FK_Telefone_Doador_DoadorId",
                schema: "dbo",
                table: "Telefone");

            migrationBuilder.DropForeignKey(
                name: "FK_Telefone_PontoAdocao_PontoAdocaoId",
                schema: "dbo",
                table: "Telefone");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Telefone",
                schema: "dbo",
                table: "Telefone");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PontoAdocao",
                schema: "dbo",
                table: "PontoAdocao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Evento",
                schema: "dbo",
                table: "Evento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Doador",
                schema: "dbo",
                table: "Doador");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Animal",
                schema: "dbo",
                table: "Animal");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Adotante",
                schema: "dbo",
                table: "Adotante");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Adocao",
                schema: "dbo",
                table: "Adocao");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "dbo",
                table: "Evento");

            migrationBuilder.RenameTable(
                name: "Telefone",
                schema: "dbo",
                newName: "Telefones",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "PontoAdocao",
                schema: "dbo",
                newName: "PontosAdocao",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Evento",
                schema: "dbo",
                newName: "Eventos",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Doador",
                schema: "dbo",
                newName: "Doadores",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Animal",
                schema: "dbo",
                newName: "Animais",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Adotante",
                schema: "dbo",
                newName: "Adotantes",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Adocao",
                schema: "dbo",
                newName: "Adocoes",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "NomeCompleto",
                schema: "dbo",
                table: "Voluntario",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "DataAcompanhamento",
                schema: "dbo",
                table: "AnimalEvento",
                newName: "Data");

            migrationBuilder.RenameIndex(
                name: "IX_Telefone_PontoAdocaoId",
                schema: "dbo",
                table: "Telefones",
                newName: "IX_Telefones_PontoAdocaoId");

            migrationBuilder.RenameIndex(
                name: "IX_Telefone_DoadorId",
                schema: "dbo",
                table: "Telefones",
                newName: "IX_Telefones_DoadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Telefone_AdotanteId",
                schema: "dbo",
                table: "Telefones",
                newName: "IX_Telefones_AdotanteId");

            migrationBuilder.RenameIndex(
                name: "IX_Animal_DoadorId",
                schema: "dbo",
                table: "Animais",
                newName: "IX_Animais_DoadorId");

            migrationBuilder.RenameColumn(
                name: "DataAdocao",
                schema: "dbo",
                table: "Adocoes",
                newName: "Data");

            migrationBuilder.RenameIndex(
                name: "IX_Adocao_VoluntarioId",
                schema: "dbo",
                table: "Adocoes",
                newName: "IX_Adocoes_VoluntarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Adocao_PontoAdocaoId",
                schema: "dbo",
                table: "Adocoes",
                newName: "IX_Adocoes_PontoAdocaoId");

            migrationBuilder.RenameIndex(
                name: "IX_Adocao_AnimalId",
                schema: "dbo",
                table: "Adocoes",
                newName: "IX_Adocoes_AnimalId");

            migrationBuilder.RenameIndex(
                name: "IX_Adocao_AdotanteId",
                schema: "dbo",
                table: "Adocoes",
                newName: "IX_Adocoes_AdotanteId");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "dbo",
                table: "Voluntario",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<string>(
                name: "Observacao",
                schema: "dbo",
                table: "AnimalEvento",
                type: "nvarchar(600)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)");

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                schema: "dbo",
                table: "Eventos",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "dbo",
                table: "Animais",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "dbo",
                table: "Adotantes",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "SituacaoEndereco",
                schema: "dbo",
                table: "Adotantes",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Telefones",
                schema: "dbo",
                table: "Telefones",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PontosAdocao",
                schema: "dbo",
                table: "PontosAdocao",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Eventos",
                schema: "dbo",
                table: "Eventos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Doadores",
                schema: "dbo",
                table: "Doadores",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Animais",
                schema: "dbo",
                table: "Animais",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Adotantes",
                schema: "dbo",
                table: "Adotantes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Adocoes",
                schema: "dbo",
                table: "Adocoes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Adocoes_Adotantes_AdotanteId",
                schema: "dbo",
                table: "Adocoes",
                column: "AdotanteId",
                principalSchema: "dbo",
                principalTable: "Adotantes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Adocoes_Animais_AnimalId",
                schema: "dbo",
                table: "Adocoes",
                column: "AnimalId",
                principalSchema: "dbo",
                principalTable: "Animais",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Adocoes_PontosAdocao_PontoAdocaoId",
                schema: "dbo",
                table: "Adocoes",
                column: "PontoAdocaoId",
                principalSchema: "dbo",
                principalTable: "PontosAdocao",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Adocoes_Voluntario_VoluntarioId",
                schema: "dbo",
                table: "Adocoes",
                column: "VoluntarioId",
                principalSchema: "dbo",
                principalTable: "Voluntario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Animais_Doadores_DoadorId",
                schema: "dbo",
                table: "Animais",
                column: "DoadorId",
                principalSchema: "dbo",
                principalTable: "Doadores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalEvento_Animais_AnimalId",
                schema: "dbo",
                table: "AnimalEvento",
                column: "AnimalId",
                principalSchema: "dbo",
                principalTable: "Animais",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalEvento_Eventos_EventoId",
                schema: "dbo",
                table: "AnimalEvento",
                column: "EventoId",
                principalSchema: "dbo",
                principalTable: "Eventos",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adocoes_Adotantes_AdotanteId",
                schema: "dbo",
                table: "Adocoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Adocoes_Animais_AnimalId",
                schema: "dbo",
                table: "Adocoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Adocoes_PontosAdocao_PontoAdocaoId",
                schema: "dbo",
                table: "Adocoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Adocoes_Voluntario_VoluntarioId",
                schema: "dbo",
                table: "Adocoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Animais_Doadores_DoadorId",
                schema: "dbo",
                table: "Animais");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimalEvento_Animais_AnimalId",
                schema: "dbo",
                table: "AnimalEvento");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimalEvento_Eventos_EventoId",
                schema: "dbo",
                table: "AnimalEvento");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Telefones",
                schema: "dbo",
                table: "Telefones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PontosAdocao",
                schema: "dbo",
                table: "PontosAdocao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Eventos",
                schema: "dbo",
                table: "Eventos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Doadores",
                schema: "dbo",
                table: "Doadores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Animais",
                schema: "dbo",
                table: "Animais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Adotantes",
                schema: "dbo",
                table: "Adotantes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Adocoes",
                schema: "dbo",
                table: "Adocoes");

            migrationBuilder.DropColumn(
                name: "Descricao",
                schema: "dbo",
                table: "Eventos");

            migrationBuilder.RenameTable(
                name: "Telefones",
                schema: "dbo",
                newName: "Telefone",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "PontosAdocao",
                schema: "dbo",
                newName: "PontoAdocao",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Eventos",
                schema: "dbo",
                newName: "Evento",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Doadores",
                schema: "dbo",
                newName: "Doador",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Animais",
                schema: "dbo",
                newName: "Animal",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Adotantes",
                schema: "dbo",
                newName: "Adotante",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Adocoes",
                schema: "dbo",
                newName: "Adocao",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "Nome",
                schema: "dbo",
                table: "Voluntario",
                newName: "NomeCompleto");

            migrationBuilder.RenameColumn(
                name: "Data",
                schema: "dbo",
                table: "AnimalEvento",
                newName: "DataAcompanhamento");

            migrationBuilder.RenameIndex(
                name: "IX_Telefones_PontoAdocaoId",
                schema: "dbo",
                table: "Telefone",
                newName: "IX_Telefone_PontoAdocaoId");

            migrationBuilder.RenameIndex(
                name: "IX_Telefones_DoadorId",
                schema: "dbo",
                table: "Telefone",
                newName: "IX_Telefone_DoadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Telefones_AdotanteId",
                schema: "dbo",
                table: "Telefone",
                newName: "IX_Telefone_AdotanteId");

            migrationBuilder.RenameIndex(
                name: "IX_Animais_DoadorId",
                schema: "dbo",
                table: "Animal",
                newName: "IX_Animal_DoadorId");

            migrationBuilder.RenameColumn(
                name: "Data",
                schema: "dbo",
                table: "Adocao",
                newName: "DataAdocao");

            migrationBuilder.RenameIndex(
                name: "IX_Adocoes_VoluntarioId",
                schema: "dbo",
                table: "Adocao",
                newName: "IX_Adocao_VoluntarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Adocoes_PontoAdocaoId",
                schema: "dbo",
                table: "Adocao",
                newName: "IX_Adocao_PontoAdocaoId");

            migrationBuilder.RenameIndex(
                name: "IX_Adocoes_AnimalId",
                schema: "dbo",
                table: "Adocao",
                newName: "IX_Adocao_AnimalId");

            migrationBuilder.RenameIndex(
                name: "IX_Adocoes_AdotanteId",
                schema: "dbo",
                table: "Adocao",
                newName: "IX_Adocao_AdotanteId");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                schema: "dbo",
                table: "Voluntario",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Observacao",
                schema: "dbo",
                table: "AnimalEvento",
                type: "nvarchar(500)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(600)");

            migrationBuilder.AddColumn<byte>(
                name: "Code",
                schema: "dbo",
                table: "Evento",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                schema: "dbo",
                table: "Animal",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                schema: "dbo",
                table: "Adotante",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "SituacaoEndereco",
                schema: "dbo",
                table: "Adotante",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Telefone",
                schema: "dbo",
                table: "Telefone",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PontoAdocao",
                schema: "dbo",
                table: "PontoAdocao",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Evento",
                schema: "dbo",
                table: "Evento",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Doador",
                schema: "dbo",
                table: "Doador",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Animal",
                schema: "dbo",
                table: "Animal",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Adotante",
                schema: "dbo",
                table: "Adotante",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Adocao",
                schema: "dbo",
                table: "Adocao",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Adocao_Adotante_AdotanteId",
                schema: "dbo",
                table: "Adocao",
                column: "AdotanteId",
                principalSchema: "dbo",
                principalTable: "Adotante",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Adocao_Animal_AnimalId",
                schema: "dbo",
                table: "Adocao",
                column: "AnimalId",
                principalSchema: "dbo",
                principalTable: "Animal",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Adocao_PontoAdocao_PontoAdocaoId",
                schema: "dbo",
                table: "Adocao",
                column: "PontoAdocaoId",
                principalSchema: "dbo",
                principalTable: "PontoAdocao",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Adocao_Voluntario_VoluntarioId",
                schema: "dbo",
                table: "Adocao",
                column: "VoluntarioId",
                principalSchema: "dbo",
                principalTable: "Voluntario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Animal_Doador_DoadorId",
                schema: "dbo",
                table: "Animal",
                column: "DoadorId",
                principalSchema: "dbo",
                principalTable: "Doador",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalEvento_Animal_AnimalId",
                schema: "dbo",
                table: "AnimalEvento",
                column: "AnimalId",
                principalSchema: "dbo",
                principalTable: "Animal",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalEvento_Evento_EventoId",
                schema: "dbo",
                table: "AnimalEvento",
                column: "EventoId",
                principalSchema: "dbo",
                principalTable: "Evento",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Telefone_Adotante_AdotanteId",
                schema: "dbo",
                table: "Telefone",
                column: "AdotanteId",
                principalSchema: "dbo",
                principalTable: "Adotante",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Telefone_Doador_DoadorId",
                schema: "dbo",
                table: "Telefone",
                column: "DoadorId",
                principalSchema: "dbo",
                principalTable: "Doador",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Telefone_PontoAdocao_PontoAdocaoId",
                schema: "dbo",
                table: "Telefone",
                column: "PontoAdocaoId",
                principalSchema: "dbo",
                principalTable: "PontoAdocao",
                principalColumn: "Id");
        }
    }
}
