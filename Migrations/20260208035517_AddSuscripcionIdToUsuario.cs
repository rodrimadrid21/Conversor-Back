using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conversor_Monedas_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSuscripcionIdToUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SuscripcionId",
                table: "Users",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SuscripcionId",
                table: "Users");
        }
    }
}
