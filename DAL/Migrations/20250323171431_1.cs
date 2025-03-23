using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BorrowerId",
                table: "Books",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Books_BorrowerId",
                table: "Books",
                column: "BorrowerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_BorrowerId",
                table: "Books",
                column: "BorrowerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_BorrowerId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BorrowerId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BorrowerId",
                table: "Books");
        }
    }
}
