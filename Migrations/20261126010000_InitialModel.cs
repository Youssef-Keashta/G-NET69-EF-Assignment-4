using G_NET69_EF_Assignment_4.DbContexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G_NET69_EF_Assignment_4.Migrations;

[DbContext(typeof(NationalBankDbContext))]
[Migration("20261126010000_InitialModel")]
public partial class InitialModel : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Managers",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                PhoneNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                HireDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Managers", item => item.Id));

        migrationBuilder.CreateTable(
            name: "Customers",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                PhoneNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                NationalId = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                CustomerType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Customers", item => item.Id));

        migrationBuilder.CreateTable(
            name: "Branches",
            columns: table => new
            {
                Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                PhoneNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                ManagerId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Branches", item => item.Code);
                table.ForeignKey(
                    name: "FK_Branches_Managers_ManagerId",
                    column: item => item.ManagerId,
                    principalTable: "Managers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Accounts",
            columns: table => new
            {
                AccountNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                CurrentBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                AccountType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                OpeningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                BranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Accounts", item => item.AccountNumber);
                table.ForeignKey(
                    name: "FK_Accounts_Branches_BranchCode",
                    column: item => item.BranchCode,
                    principalTable: "Branches",
                    principalColumn: "Code",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "CustomerAccounts",
            columns: table => new
            {
                CustomerId = table.Column<int>(type: "int", nullable: false),
                AccountNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                OwnershipType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                OwnershipStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                AccountStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CustomerAccounts", item => new { item.CustomerId, item.AccountNumber });
                table.ForeignKey(
                    name: "FK_CustomerAccounts_Accounts_AccountNumber",
                    column: item => item.AccountNumber,
                    principalTable: "Accounts",
                    principalColumn: "AccountNumber",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_CustomerAccounts_Customers_CustomerId",
                    column: item => item.CustomerId,
                    principalTable: "Customers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Transactions",
            columns: table => new
            {
                TransactionNumber = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                TransactionType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                AccountNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Transactions", item => item.TransactionNumber);
                table.ForeignKey(
                    name: "FK_Transactions_Accounts_AccountNumber",
                    column: item => item.AccountNumber,
                    principalTable: "Accounts",
                    principalColumn: "AccountNumber",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Accounts_BranchCode",
            table: "Accounts",
            column: "BranchCode");

        migrationBuilder.CreateIndex(
            name: "IX_Branches_ManagerId",
            table: "Branches",
            column: "ManagerId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Customers_Email",
            table: "Customers",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Customers_NationalId",
            table: "Customers",
            column: "NationalId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_CustomerAccounts_AccountNumber",
            table: "CustomerAccounts",
            column: "AccountNumber");

        migrationBuilder.CreateIndex(
            name: "IX_Managers_Email",
            table: "Managers",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Transactions_AccountNumber",
            table: "Transactions",
            column: "AccountNumber");

    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "CustomerAccounts");
        migrationBuilder.DropTable(name: "Transactions");
        migrationBuilder.DropTable(name: "Accounts");
        migrationBuilder.DropTable(name: "Branches");
        migrationBuilder.DropTable(name: "Customers");
        migrationBuilder.DropTable(name: "Managers");
    }
}
