using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthImplementation.Migrations
{
    public partial class InsertsInitialIdentityUserRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO AspNetRoles (Name, NormalizedName, ConcurrencyStamp) VALUES ('Administrator', 'ADMINISTRATOR', '" + Guid.NewGuid().ToString() + "'), ('Manager', 'MANAGER', '" + Guid.NewGuid().ToString() + "')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM AspNetRoles WHERE Name IN ('Administrator', 'Manager')");
        }
    }
}
