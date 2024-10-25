using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECR.Domain.Migrations
{
    /// <inheritdoc />
    public partial class added_login_in_record : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoginId",
                table: "Record",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Record_LoginId",
                table: "Record",
                column: "LoginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Record_Login_LoginId",
                table: "Record",
                column: "LoginId",
                principalTable: "Login",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Record_Login_LoginId",
                table: "Record");

            migrationBuilder.DropIndex(
                name: "IX_Record_LoginId",
                table: "Record");

            migrationBuilder.DropColumn(
                name: "LoginId",
                table: "Record");
        }
    }
}
