using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECR.Domain.Migrations
{
    /// <inheritdoc />
    public partial class referencial_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audio_Record_RecordId",
                table: "Audio");

            migrationBuilder.DropTable(
                name: "Callers");

            migrationBuilder.AddColumn<string>(
                name: "Call_Address",
                table: "Record",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Call_ContactDetail",
                table: "Record",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Call_Name",
                table: "Record",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "RecordId",
                table: "Audio",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Audio_Record_RecordId",
                table: "Audio",
                column: "RecordId",
                principalTable: "Record",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audio_Record_RecordId",
                table: "Audio");

            migrationBuilder.DropColumn(
                name: "Call_Address",
                table: "Record");

            migrationBuilder.DropColumn(
                name: "Call_ContactDetail",
                table: "Record");

            migrationBuilder.DropColumn(
                name: "Call_Name",
                table: "Record");

            migrationBuilder.AlterColumn<int>(
                name: "RecordId",
                table: "Audio",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Callers",
                columns: table => new
                {
                    RecordId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactDetail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Callers", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_Callers_Record_RecordId",
                        column: x => x.RecordId,
                        principalTable: "Record",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Audio_Record_RecordId",
                table: "Audio",
                column: "RecordId",
                principalTable: "Record",
                principalColumn: "Id");
        }
    }
}
