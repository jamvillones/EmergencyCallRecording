using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECR.Domain.Migrations
{
    /// <inheritdoc />
    public partial class add_datetime_record : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Landmark",
                table: "Record");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeOfReport",
                table: "Record",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTimeOfReport",
                table: "Record");

            migrationBuilder.AddColumn<string>(
                name: "Landmark",
                table: "Record",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
