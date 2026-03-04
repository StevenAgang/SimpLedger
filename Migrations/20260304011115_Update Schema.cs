using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpLedger.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Inventory_Inventory_Id",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Inventory");

            migrationBuilder.RenameColumn(
                name: "Inventory_Id",
                table: "Sales",
                newName: "Branch_Id");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Sales",
                newName: "TotalAmount");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_Inventory_Id",
                table: "Sales",
                newName: "IX_Sales_Branch_Id");

            migrationBuilder.RenameColumn(
                name: "Expiration",
                table: "Inventory",
                newName: "ExpirationDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalDate",
                table: "Inventory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Branch_Id",
                table: "Inventory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "CostPrice",
                table: "Inventory",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Product_Id",
                table: "Inventory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AccountType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_By = table.Column<int>(type: "int", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_By = table.Column<int>(type: "int", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted_By = table.Column<int>(type: "int", nullable: false),
                    Deleted_At = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Created_By = table.Column<int>(type: "int", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_By = table.Column<int>(type: "int", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted_By = table.Column<int>(type: "int", nullable: false),
                    Deleted_At = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sales_Id = table.Column<int>(type: "int", nullable: false),
                    Inventory_Id = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Subtotal = table.Column<double>(type: "float", nullable: false),
                    Created_By = table.Column<int>(type: "int", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_By = table.Column<int>(type: "int", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted_By = table.Column<int>(type: "int", nullable: false),
                    Deleted_At = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountType_Id = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_By = table.Column<int>(type: "int", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_By = table.Column<int>(type: "int", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted_By = table.Column<int>(type: "int", nullable: false),
                    Deleted_At = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAccount_Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_By = table.Column<int>(type: "int", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_By = table.Column<int>(type: "int", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted_By = table.Column<int>(type: "int", nullable: false),
                    Deleted_At = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company_Id = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_By = table.Column<int>(type: "int", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_By = table.Column<int>(type: "int", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted_By = table.Column<int>(type: "int", nullable: false),
                    Deleted_At = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAccount_Id = table.Column<int>(type: "int", nullable: false),
                    Branch_Id = table.Column<int>(type: "int", nullable: false),
                    Created_By = table.Column<int>(type: "int", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_By = table.Column<int>(type: "int", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted_By = table.Column<int>(type: "int", nullable: false),
                    Deleted_At = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_Branch_Id",
                table: "Inventory",
                column: "Branch_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_Product_Id",
                table: "Inventory",
                column: "Product_Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Branch_Branch_Id",
                table: "Inventory",
                column: "Branch_Id",
                principalTable: "Branch",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Product_Product_Id",
                table: "Inventory",
                column: "Product_Id",
                principalTable: "Product",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Branch_Branch_Id",
                table: "Sales",
                column: "Branch_Id",
                principalTable: "Branch",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Branch_Branch_Id",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Product_Product_Id",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Branch_Branch_Id",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "SalesItem");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "UserAccount");

            migrationBuilder.DropTable(
                name: "AccountType");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_Branch_Id",
                table: "Inventory");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_Product_Id",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "ArrivalDate",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "Branch_Id",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "Product_Id",
                table: "Inventory");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Sales",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "Branch_Id",
                table: "Sales",
                newName: "Inventory_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_Branch_Id",
                table: "Sales",
                newName: "IX_Sales_Inventory_Id");

            migrationBuilder.RenameColumn(
                name: "ExpirationDate",
                table: "Inventory",
                newName: "Expiration");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Inventory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Inventory_Inventory_Id",
                table: "Sales",
                column: "Inventory_Id",
                principalTable: "Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
