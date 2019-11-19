using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkedMink.Data.Domain.EnvironManager.SqlServer.Migrations
{
    public partial class AddPortToHardwareDevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Port",
                table: "HardwareDevices",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Port",
                table: "HardwareDevices");
        }
    }
}
