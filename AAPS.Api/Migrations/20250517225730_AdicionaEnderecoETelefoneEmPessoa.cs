using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaEnderecoETelefoneEmPessoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enderecos",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Telefones",
                schema: "dbo");

            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cep",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(8)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contato1",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(11)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Contato2",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(11)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Logradouro",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(150)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Numero",
                schema: "dbo",
                table: "Pessoas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SituacaoEndereco",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Uf",
                schema: "dbo",
                table: "Pessoas",
                type: "char(2)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Resgatado",
                schema: "dbo",
                table: "Animais",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Cancelada",
                schema: "dbo",
                table: "Adocoes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bairro",
                schema: "dbo",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Cep",
                schema: "dbo",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Cidade",
                schema: "dbo",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Complemento",
                schema: "dbo",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Contato1",
                schema: "dbo",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Contato2",
                schema: "dbo",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Logradouro",
                schema: "dbo",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Numero",
                schema: "dbo",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "SituacaoEndereco",
                schema: "dbo",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Uf",
                schema: "dbo",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Resgatado",
                schema: "dbo",
                table: "Animais");

            migrationBuilder.DropColumn(
                name: "Cancelada",
                schema: "dbo",
                table: "Adocoes");

            migrationBuilder.CreateTable(
                name: "Enderecos",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PessoaId = table.Column<int>(type: "int", nullable: false),
                    Bairro = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Cep = table.Column<string>(type: "nvarchar(8)", nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Complemento = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Logradouro = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    SituacaoEndereco = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Uf = table.Column<string>(type: "char(2)", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Telefones",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PessoaId = table.Column<int>(type: "int", nullable: false),
                    NumeroTelefone = table.Column<string>(type: "nvarchar(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telefones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Telefones_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalSchema: "dbo",
                        principalTable: "Pessoas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_PessoaId",
                schema: "dbo",
                table: "Enderecos",
                column: "PessoaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Telefones_PessoaId",
                schema: "dbo",
                table: "Telefones",
                column: "PessoaId");
        }
    }
}
