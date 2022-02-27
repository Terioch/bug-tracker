using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTracker.PgsqlMigrations.Migrations
{
    public partial class TicketForeignKeyNullabilityChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_SubmitterId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Projects_ProjectId",
                table: "Tickets");

            migrationBuilder.AlterColumn<string>(
                name: "SubmitterId",
                table: "Tickets",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "Tickets",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_SubmitterId",
                table: "Tickets",
                column: "SubmitterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Projects_ProjectId",
                table: "Tickets",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_SubmitterId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Projects_ProjectId",
                table: "Tickets");

            migrationBuilder.AlterColumn<string>(
                name: "SubmitterId",
                table: "Tickets",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "Tickets",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_SubmitterId",
                table: "Tickets",
                column: "SubmitterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Projects_ProjectId",
                table: "Tickets",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
