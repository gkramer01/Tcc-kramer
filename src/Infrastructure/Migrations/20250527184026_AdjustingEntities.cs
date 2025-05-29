using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdjustingEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "criado_por",
                table: "User");

            migrationBuilder.DropColumn(
                name: "criado_por",
                table: "Brand");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Store",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Store",
                newName: "endereco");

            migrationBuilder.RenameColumn(
                name: "PaymentConditions",
                table: "Store",
                newName: "meios_de_pagamento");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Brand",
                newName: "nome");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "nome",
                table: "Store",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "meios_de_pagamento",
                table: "Store",
                newName: "PaymentConditions");

            migrationBuilder.RenameColumn(
                name: "endereco",
                table: "Store",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "Brand",
                newName: "name");

            migrationBuilder.AddColumn<Guid>(
                name: "criado_por",
                table: "User",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "criado_por",
                table: "Brand",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
