using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpLedger.Migrations
{
    /// <inheritdoc />
    public partial class servicetokenschemaaddexpiresInSeconds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpiresInSeconds",
                table: "ServiceTokens",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiresInSeconds",
                table: "ServiceTokens");
        }
    }
}
