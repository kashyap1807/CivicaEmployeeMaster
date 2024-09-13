using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivicaEmployeeMaster.Migrations
{
    public partial class seedPasswordHint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "PasswordHints",
            columns: new[] { "PasswordHintId", "PasswordHintQuestion" },
            values: new object[,]
{
                    {1,"Favourite color" },
                    {2,"Birth City" },
                    {3,"Favourite color destination" },
});

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
    table: "PasswordHints",
    keyColumn: "PasswordHintId",
    keyValue: new object[] { 1, 2, 3 }
    );

        }
    }
}
