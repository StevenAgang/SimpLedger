using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpLedger.Migrations
{
    /// <inheritdoc />
    public partial class addexpirationincodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresIn",
                table: "VerificationCodes",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiresIn",
                table: "VerificationCodes");
        }
    }
}
