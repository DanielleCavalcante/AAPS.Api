using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAPS.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemocaoColunasIdentityUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "EmailIndex",
                schema: "dbo",
                table: "Voluntario");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                schema: "dbo",
                table: "Voluntario");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                schema: "dbo",
                table: "Voluntario");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                schema: "dbo",
                table: "Voluntario");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                schema: "dbo",
                table: "Voluntario");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                schema: "dbo",
                table: "Voluntario");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                schema: "dbo",
                table: "Voluntario");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                schema: "dbo",
                table: "Voluntario");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                schema: "dbo",
                table: "Voluntario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                schema: "dbo",
                table: "Voluntario",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                schema: "dbo",
                table: "Voluntario",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                schema: "dbo",
                table: "Voluntario",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                schema: "dbo",
                table: "Voluntario",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                schema: "dbo",
                table: "Voluntario",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                schema: "dbo",
                table: "Voluntario",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                schema: "dbo",
                table: "Voluntario",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                schema: "dbo",
                table: "Voluntario",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "dbo",
                table: "Voluntario",
                column: "NormalizedEmail");
        }
    }
}
