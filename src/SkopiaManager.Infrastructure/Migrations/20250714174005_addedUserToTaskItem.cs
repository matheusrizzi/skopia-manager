using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkopiaManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedUserToTaskItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TaskItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TaskItem_UserId",
                table: "TaskItem",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItem_User_UserId",
                table: "TaskItem",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItem_User_UserId",
                table: "TaskItem");

            migrationBuilder.DropIndex(
                name: "IX_TaskItem_UserId",
                table: "TaskItem");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TaskItem");
        }
    }
}
