using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECR.Domain.Migrations
{
    /// <inheritdoc />
    public partial class add_default_in_contact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Agency");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "ContactDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "ContactDetail");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Agency",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
