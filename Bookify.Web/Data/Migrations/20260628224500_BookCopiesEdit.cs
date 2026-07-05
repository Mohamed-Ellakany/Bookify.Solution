using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class BookCopiesEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "BookCopies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "BookCopies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DeletedById",
                table: "BookCopies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "BookCopies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BookCopies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedOn",
                table: "BookCopies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "BookCopies",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "BookCopies");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "BookCopies");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "BookCopies");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "BookCopies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BookCopies");

            migrationBuilder.DropColumn(
                name: "LastUpdatedOn",
                table: "BookCopies");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "BookCopies");
        }
    }
}
