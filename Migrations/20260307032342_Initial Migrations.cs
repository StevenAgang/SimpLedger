using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SimpLedger.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Created_By = table.Column<int>(type: "integer", nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_By = table.Column<int>(type: "integer", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Deleted_By = table.Column<int>(type: "integer", nullable: false),
                    Deleted_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Created_By = table.Column<int>(type: "integer", nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_By = table.Column<int>(type: "integer", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Deleted_By = table.Column<int>(type: "integer", nullable: false),
                    Deleted_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountType_Id = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Salt = table.Column<string>(type: "text", nullable: true),
                    Created_By = table.Column<int>(type: "integer", nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_By = table.Column<int>(type: "integer", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Deleted_By = table.Column<int>(type: "integer", nullable: false),
                    Deleted_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAccount_AccountType_AccountType_Id",
                        column: x => x.AccountType_Id,
                        principalTable: "AccountType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserAccount_Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Created_By = table.Column<int>(type: "integer", nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_By = table.Column<int>(type: "integer", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Deleted_By = table.Column<int>(type: "integer", nullable: false),
                    Deleted_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_UserAccount_UserAccount_Id",
                        column: x => x.UserAccount_Id,
                        principalTable: "UserAccount",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Company_Id = table.Column<int>(type: "integer", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Created_By = table.Column<int>(type: "integer", nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_By = table.Column<int>(type: "integer", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Deleted_By = table.Column<int>(type: "integer", nullable: false),
                    Deleted_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branch_Company_Company_Id",
                        column: x => x.Company_Id,
                        principalTable: "Company",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserAccount_Id = table.Column<int>(type: "integer", nullable: false),
                    Branch_Id = table.Column<int>(type: "integer", nullable: false),
                    Created_By = table.Column<int>(type: "integer", nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_By = table.Column<int>(type: "integer", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Deleted_By = table.Column<int>(type: "integer", nullable: false),
                    Deleted_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Branch_Branch_Id",
                        column: x => x.Branch_Id,
                        principalTable: "Branch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employee_UserAccount_UserAccount_Id",
                        column: x => x.UserAccount_Id,
                        principalTable: "UserAccount",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Branch_Id = table.Column<int>(type: "integer", nullable: false),
                    Product_Id = table.Column<int>(type: "integer", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false),
                    ArrivalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CostPrice = table.Column<double>(type: "double precision", nullable: false),
                    Created_By = table.Column<int>(type: "integer", nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_By = table.Column<int>(type: "integer", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Deleted_By = table.Column<int>(type: "integer", nullable: false),
                    Deleted_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventory_Branch_Branch_Id",
                        column: x => x.Branch_Id,
                        principalTable: "Branch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inventory_Product_Product_Id",
                        column: x => x.Product_Id,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Branch_Id = table.Column<int>(type: "integer", nullable: false),
                    TotalAmount = table.Column<double>(type: "double precision", nullable: false),
                    Created_By = table.Column<int>(type: "integer", nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_By = table.Column<int>(type: "integer", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Deleted_By = table.Column<int>(type: "integer", nullable: false),
                    Deleted_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Branch_Branch_Id",
                        column: x => x.Branch_Id,
                        principalTable: "Branch",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalesItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sales_Id = table.Column<int>(type: "integer", nullable: false),
                    Inventory_Id = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Subtotal = table.Column<double>(type: "double precision", nullable: false),
                    Created_By = table.Column<int>(type: "integer", nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_By = table.Column<int>(type: "integer", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Deleted_By = table.Column<int>(type: "integer", nullable: false),
                    Deleted_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesItem_Inventory_Inventory_Id",
                        column: x => x.Inventory_Id,
                        principalTable: "Inventory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesItem_Sales_Sales_Id",
                        column: x => x.Sales_Id,
                        principalTable: "Sales",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branch_Company_Id",
                table: "Branch",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_UserAccount_Id",
                table: "Company",
                column: "UserAccount_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Branch_Id",
                table: "Employee",
                column: "Branch_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_UserAccount_Id",
                table: "Employee",
                column: "UserAccount_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_Branch_Id",
                table: "Inventory",
                column: "Branch_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_Product_Id",
                table: "Inventory",
                column: "Product_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_Branch_Id",
                table: "Sales",
                column: "Branch_Id");

            migrationBuilder.CreateIndex(
                name: "IX_SalesItem_Inventory_Id",
                table: "SalesItem",
                column: "Inventory_Id");

            migrationBuilder.CreateIndex(
                name: "IX_SalesItem_Sales_Id",
                table: "SalesItem",
                column: "Sales_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_AccountType_Id",
                table: "UserAccount",
                column: "AccountType_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "SalesItem");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "UserAccount");

            migrationBuilder.DropTable(
                name: "AccountType");
        }
    }
}
