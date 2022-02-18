using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTracker.Migrations
{
    public partial class AddedTicketHistoryModifierId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "TicketHistoryRecords");

            migrationBuilder.AddColumn<string>(
                name: "ModifierId",
                table: "TicketHistoryRecords",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistoryRecords_ModifierId",
                table: "TicketHistoryRecords",
                column: "ModifierId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketHistoryRecords_AspNetUsers_ModifierId",
                table: "TicketHistoryRecords",
                column: "ModifierId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketHistoryRecords_AspNetUsers_ModifierId",
                table: "TicketHistoryRecords");

            migrationBuilder.DropIndex(
                name: "IX_TicketHistoryRecords_ModifierId",
                table: "TicketHistoryRecords");

            migrationBuilder.DropColumn(
                name: "ModifierId",
                table: "TicketHistoryRecords");

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "TicketHistoryRecords",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
