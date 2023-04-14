using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyToDoWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class addFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memos_Users_OwnerId",
                table: "Memos");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_Users_OwnerId",
                table: "ToDos");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "ToDos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "Memos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Memos_Users_OwnerId",
                table: "Memos",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_Users_OwnerId",
                table: "ToDos",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "ToDos",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "Memos",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

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
    }
}
