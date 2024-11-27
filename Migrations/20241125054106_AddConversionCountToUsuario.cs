﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conversor_Monedas_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddConversionCountToUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConversionCount",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversionCount",
                table: "Users");
        }
    }
}
