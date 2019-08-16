using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetMe.Service.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proposal",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MeetingId = table.Column<Guid>(nullable: false),
                    Time = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proposal", x => new { x.MeetingId, x.Id });
                    table.ForeignKey(
                        name: "FK_Proposal_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vote",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProposalMeetingId = table.Column<Guid>(nullable: false),
                    ProposalId = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vote", x => new { x.ProposalMeetingId, x.ProposalId, x.Id });
                    table.ForeignKey(
                        name: "FK_Vote_Proposal_ProposalMeetingId_ProposalId",
                        columns: x => new { x.ProposalMeetingId, x.ProposalId },
                        principalTable: "Proposal",
                        principalColumns: new[] { "MeetingId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vote");

            migrationBuilder.DropTable(
                name: "Proposal");

            migrationBuilder.DropTable(
                name: "Meetings");
        }
    }
}
