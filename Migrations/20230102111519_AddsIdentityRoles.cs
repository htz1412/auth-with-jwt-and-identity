using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthImplementation.Migrations
{
    public partial class AddsIdentityRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "032aaf1e-9327-494f-9f7e-79e06a149e76", "a38621e6-9363-430a-a73f-9eec24308bb9", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f87eadca-be56-4b51-9105-27ea9a24f07c", "791543dc-8415-4fb0-a6ef-db06254811f1", "Administration", "ADMINISTRATION" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "032aaf1e-9327-494f-9f7e-79e06a149e76");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f87eadca-be56-4b51-9105-27ea9a24f07c");
        }
    }
}
