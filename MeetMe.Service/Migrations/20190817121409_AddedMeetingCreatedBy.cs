using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetMe.Service.Migrations
{
    public partial class AddedMeetingCreatedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Meetings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Meetings");
        }
    }
}
