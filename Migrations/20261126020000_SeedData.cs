using G_NET69_EF_Assignment_4.DbContexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G_NET69_EF_Assignment_4.Migrations;

[DbContext(typeof(NationalBankDbContext))]
[Migration("20261126020000_SeedData")]
public partial class SeedData : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "Managers",
            columns: new[] { "Id", "FullName", "Email", "PhoneNumber", "HireDate" },
            columnTypes: new[] { "int", "nvarchar(150)", "nvarchar(255)", "nvarchar(30)", "datetime2" },
            values: new object[,]
            {
                { 1, "Ahmed Hassan", "ahmed.hassan@nationalbank.example", "01012345678", new DateTime(2018, 5, 1) },
                { 2, "Mariam Ali", "mariam.ali@nationalbank.example", "01098765432", new DateTime(2020, 8, 15) }
            });

        migrationBuilder.InsertData(
            table: "Branches",
            columns: new[] { "Code", "Name", "Address", "PhoneNumber", "ManagerId" },
            columnTypes: new[] { "nvarchar(20)", "nvarchar(100)", "nvarchar(250)", "nvarchar(30)", "int" },
            values: new object[,]
            {
                { "CAI-01", "Cairo Main Branch", "15 Tahrir Square, Cairo", "0223456789", 1 },
                { "ALX-01", "Alexandria Branch", "10 Saad Zaghloul Street, Alexandria", "0334567890", 2 }
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Branches",
            keyColumn: "Code",
            keyValues: new object[] { "CAI-01", "ALX-01" });

        migrationBuilder.DeleteData(
            table: "Managers",
            keyColumn: "Id",
            keyValues: new object[] { 1, 2 });
    }
}
