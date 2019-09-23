using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetMe.Service.Migrations
{
    public partial class AddedVoteProposalId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proposal_Meetings_MeetingId",
                table: "Proposal");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Invitations_InvitationId",
                table: "Vote");

            migrationBuilder.AddColumn<Guid>(
                name: "ProposalId",
                table: "Vote",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Proposal_Meetings_MeetingId",
                table: "Proposal",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Invitations_InvitationId",
                table: "Vote",
                column: "InvitationId",
                principalTable: "Invitations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proposal_Meetings_MeetingId",
                table: "Proposal");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Invitations_InvitationId",
                table: "Vote");

            migrationBuilder.DropColumn(
                name: "ProposalId",
                table: "Vote");

            migrationBuilder.AddForeignKey(
                name: "FK_Proposal_Meetings_MeetingId",
                table: "Proposal",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Invitations_InvitationId",
                table: "Vote",
                column: "InvitationId",
                principalTable: "Invitations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
