using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyToDoWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class addUserToTableToDo_Memo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "ToDos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Memos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToDos_OwnerId",
                table: "ToDos",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Memos_OwnerId",
                table: "Memos",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Memos_Users_OwnerId",
                table: "Memos",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_Users_OwnerId",
                table: "ToDos",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memos_Users_OwnerId",
                table: "Memos");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_Users_OwnerId",
                table: "ToDos");

            migrationBuilder.DropIndex(
                name: "IX_ToDos_OwnerId",
                table: "ToDos");

            migrationBuilder.DropIndex(
                name: "IX_Memos_OwnerId",
                table: "Memos");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Memos");
        }
    }
}
