using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conversor_Monedas_Api.Migrations
{
    /// <inheritdoc />
    public partial class InitalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Moneda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Leyenda = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Simbolo = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    IndiceConvertibilidad = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moneda", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suscripcion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    MaximoConversiones = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suscripcion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    SubscriptionStartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Conversion",
                columns: table => new
                {
                    ConversionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    MonedaOrigen = table.Column<string>(type: "TEXT", nullable: false),
                    MonedaDestino = table.Column<string>(type: "TEXT", nullable: false),
                    MontoOriginal = table.Column<decimal>(type: "TEXT", nullable: false),
                    MontoConvertido = table.Column<decimal>(type: "TEXT", nullable: false),
                    FechaConversion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversion", x => x.ConversionId);
                    table.ForeignKey(
                        name: "FK_Conversion_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conversion_UserId",
                table: "Conversion",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conversion");

            migrationBuilder.DropTable(
                name: "Moneda");

            migrationBuilder.DropTable(
                name: "Suscripcion");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
