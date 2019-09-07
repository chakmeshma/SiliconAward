using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SiliconAward.Migrations
{
    public partial class migrate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastStatusTime",
                table: "Participants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastStatusTime",
                table: "Participants");
        }
    }
}
