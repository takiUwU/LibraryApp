using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Migrations
{
    /// <inheritdoc />
    public partial class Nine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookAmounts");

            migrationBuilder.AddColumn<string>(
                name: "Loans",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Loans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Loans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Authors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_UserID",
                table: "Loans",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Users_UserID",
                table: "Loans",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Users_UserID",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_UserID",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "Loans",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Authors");

            migrationBuilder.CreateTable(
                name: "BookAmounts",
                columns: table => new
                {
                    BookID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAmounts", x => x.BookID);
                    table.ForeignKey(
                        name: "FK_BookAmounts_Books_BookID",
                        column: x => x.BookID,
                        principalTable: "Books",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
