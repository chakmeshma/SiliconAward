using Microsoft.EntityFrameworkCore.Migrations;

namespace SiliconAward.Migrations
{
    public partial class miagrate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Editable",
                table: "Statues",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Editable",
                table: "Statues");
        }
    }
}
