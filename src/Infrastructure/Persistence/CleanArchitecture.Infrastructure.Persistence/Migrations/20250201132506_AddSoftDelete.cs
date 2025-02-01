using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_ApplicationUser_UserId",
                table: "RefreshTokens");

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ApplicationUser",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "ApplicationUser",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ApplicationUser",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_ApplicationUser_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_ApplicationUser_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ApplicationUser");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_ApplicationUser_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
