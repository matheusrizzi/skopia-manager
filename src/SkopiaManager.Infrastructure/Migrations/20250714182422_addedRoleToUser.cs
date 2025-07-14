using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkopiaManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedRoleToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "User",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "Desenvolvedor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "User");
        }
    }
}
