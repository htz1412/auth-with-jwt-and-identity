using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthImplementation.Migrations
{
    public partial class SeedsEmployees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Employees (Name, Email) VALUES ('Harsh Gohel', 'harsh.gohel@testing.com'), ('Test User', 'test.user@testing.com')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Employees WHERE Email IN ('harsh.gohel@testing.com', 'test.user@testing.com')");
        }
    }
}
