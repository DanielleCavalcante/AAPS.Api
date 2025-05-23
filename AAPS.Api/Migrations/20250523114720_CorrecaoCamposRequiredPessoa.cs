using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class CorrecaoCamposRequiredPessoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Uf",
                schema: "dbo",
                table: "Pessoas",
                type: "char(2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(2)");

            migrationBuilder.AlterColumn<string>(
                name: "ResponsavelContato",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(60)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)");

            migrationBuilder.AlterColumn<int>(
                name: "Numero",
                schema: "dbo",
                table: "Pessoas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Logradouro",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(150)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)");

            migrationBuilder.AlterColumn<string>(
                name: "Contato",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(11)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)");

            migrationBuilder.AlterColumn<string>(
                name: "Cidade",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "Cep",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(8)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)");

            migrationBuilder.AlterColumn<string>(
                name: "Celular",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(11)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)");

            migrationBuilder.AlterColumn<string>(
                name: "Bairro",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Uf",
                schema: "dbo",
                table: "Pessoas",
                type: "char(2)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ResponsavelContato",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(60)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Numero",
                schema: "dbo",
                table: "Pessoas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Logradouro",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(150)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Contato",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(11)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cidade",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cep",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(8)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Celular",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(11)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Bairro",
                schema: "dbo",
                table: "Pessoas",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);
        }
    }
}
