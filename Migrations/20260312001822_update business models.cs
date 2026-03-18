using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpLedger.Migrations
{
    /// <inheritdoc />
    public partial class updatebusinessmodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Company",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Company");
        }
    }
}
