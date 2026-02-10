using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conversor_Monedas_Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMaximoConversionesType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MaximoConversiones",
                table: "Suscripcion",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "MaximoConversiones",
                table: "Suscripcion",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
