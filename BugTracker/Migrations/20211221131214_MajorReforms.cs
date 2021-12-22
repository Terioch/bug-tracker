using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTracker.Migrations
{
    public partial class MajorReforms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedDeveloper",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Submitter",
                table: "Tickets");

            migrationBuilder.AddColumn<string>(
                name: "AssignedDeveloperId",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubmitterId",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "TicketId",
                table: "TicketHistoryRecords",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AssignedDeveloperId",
                table: "Tickets",
                column: "AssignedDeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SubmitterId",
                table: "Tickets",
                column: "SubmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistoryRecords_TicketId",
                table: "TicketHistoryRecords",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketHistoryRecords_Tickets_TicketId",
                table: "TicketHistoryRecords",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_AssignedDeveloperId",
                table: "Tickets",
                column: "AssignedDeveloperId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_SubmitterId",
                table: "Tickets",
                column: "SubmitterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketHistoryRecords_Tickets_TicketId",
                table: "TicketHistoryRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_AssignedDeveloperId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_SubmitterId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_AssignedDeveloperId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_SubmitterId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_TicketHistoryRecords_TicketId",
                table: "TicketHistoryRecords");

            migrationBuilder.DropColumn(
                name: "AssignedDeveloperId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SubmitterId",
                table: "Tickets");

            migrationBuilder.AddColumn<string>(
                name: "AssignedDeveloper",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Submitter",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "TicketId",
                table: "TicketHistoryRecords",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
