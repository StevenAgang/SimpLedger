using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SimpLedger.Migrations
{
    /// <inheritdoc />
    public partial class deleteservicetokenschema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VerificationCodes_ServiceTokens_ServiceTokenId",
                table: "VerificationCodes");

            migrationBuilder.DropTable(
                name: "ServiceTokens");

            migrationBuilder.DropIndex(
                name: "IX_VerificationCodes_ServiceTokenId",
                table: "VerificationCodes");

            migrationBuilder.DropColumn(
                name: "ServiceTokenId",
                table: "VerificationCodes");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "VerificationCodes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "VerificationCodes");

            migrationBuilder.AddColumn<int>(
                name: "ServiceTokenId",
                table: "VerificationCodes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ServiceTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<int>(type: "integer", nullable: true),
                    Deleted_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted_By = table.Column<int>(type: "integer", nullable: true),
                    ExpiresInSeconds = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Token = table.Column<string>(type: "text", nullable: true),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Updated_By = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTokens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VerificationCodes_ServiceTokenId",
                table: "VerificationCodes",
                column: "ServiceTokenId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationCodes_ServiceTokens_ServiceTokenId",
                table: "VerificationCodes",
                column: "ServiceTokenId",
                principalTable: "ServiceTokens",
                principalColumn: "Id");
        }
    }
}
