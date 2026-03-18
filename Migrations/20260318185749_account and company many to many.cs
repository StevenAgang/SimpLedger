using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpLedger.Migrations
{
    /// <inheritdoc />
    public partial class accountandcompanymanytomany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Company_UserAccount_Id",
                table: "Company");

            migrationBuilder.CreateIndex(
                name: "IX_Company_UserAccount_Id",
                table: "Company",
                column: "UserAccount_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Company_UserAccount_Id",
                table: "Company");

            migrationBuilder.CreateIndex(
                name: "IX_Company_UserAccount_Id",
                table: "Company",
                column: "UserAccount_Id",
                unique: true);
        }
    }
}
