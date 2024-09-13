using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivicaEmployeeMaster.Migrations
{
    public partial class seedEmployeeDepartmenttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
           table: "EmployeeDepartments",
           columns: new[] { "DepartmentId", "DepartmentName" },
           values: new object[,]
{
                    {1,"Software" },
                    {2,"Admin" },
                    {3,"Finance" },
                    {4,"Accounts" }
});

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
table: "EmployeeDepartments",
keyColumn: "DepartmentId",
keyValue: new object[] { 1, 2, 3, 4 }
);
        }
    }
}
