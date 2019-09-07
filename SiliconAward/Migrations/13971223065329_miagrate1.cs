using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SiliconAward.Migrations
{
    public partial class miagrate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompetitionFields",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    LastUpdateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionFields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statues",
                columns: table => new
                {
                    StatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    CreateTime = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    LastUpdateTime = table.Column<string>(type: "nvarchar(30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statues", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(14)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: true),
                    Email = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: true),
                    PhoneNumberVerifyCode = table.Column<string>(nullable: true),
                    EmailVerifyCode = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    CreateTime = table.Column<string>(nullable: true),
                    LastUpdateTime = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionBranchs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    LastUpdateTime = table.Column<DateTime>(nullable: true),
                    CompetitionFieldId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionBranchs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetitionBranchs_CompetitionFields_CompetitionFieldId",
                        column: x => x.CompetitionFieldId,
                        principalTable: "CompetitionFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    File = table.Column<string>(nullable: true),
                    CreateTime = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    StatusId = table.Column<int>(nullable: true),
                    DocumentType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Statues_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statues",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Statues_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statues",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    LastUpdateTime = table.Column<DateTime>(nullable: true),
                    CompetitionBranchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetitionSubjects_CompetitionBranchs_CompetitionBranchId",
                        column: x => x.CompetitionBranchId,
                        principalTable: "CompetitionBranchs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CreateTime = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    TicketId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketDetails_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketDetails_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    AttachedFile = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    LastUpdateTime = table.Column<DateTime>(nullable: true),
                    StatusId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    CompetitionSubjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participants_CompetitionSubjects_CompetitionSubjectId",
                        column: x => x.CompetitionSubjectId,
                        principalTable: "CompetitionSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participants_Statues_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statues",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionBranchs_CompetitionFieldId",
                table: "CompetitionBranchs",
                column: "CompetitionFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionSubjects_CompetitionBranchId",
                table: "CompetitionSubjects",
                column: "CompetitionBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_StatusId",
                table: "Documents",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UserId",
                table: "Documents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_CompetitionSubjectId",
                table: "Participants",
                column: "CompetitionSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_StatusId",
                table: "Participants",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_UserId",
                table: "Participants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketDetails_TicketId",
                table: "TicketDetails",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketDetails_UserId",
                table: "TicketDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_StatusId",
                table: "Tickets",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "TicketDetails");

            migrationBuilder.DropTable(
                name: "CompetitionSubjects");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "CompetitionBranchs");

            migrationBuilder.DropTable(
                name: "Statues");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CompetitionFields");
        }
    }
}
