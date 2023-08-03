using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Deixar.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterLeaveTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_Users_StatusUpdatedById",
                table: "Leaves");

            migrationBuilder.AlterColumn<int>(
                name: "StatusUpdatedById",
                table: "Leaves",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Leaves",
                type: "nvarchar(400)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)");

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_Users_StatusUpdatedById",
                table: "Leaves",
                column: "StatusUpdatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_Users_StatusUpdatedById",
                table: "Leaves");

            migrationBuilder.AlterColumn<int>(
                name: "StatusUpdatedById",
                table: "Leaves",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Leaves",
                type: "nvarchar(400)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_Users_StatusUpdatedById",
                table: "Leaves",
                column: "StatusUpdatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
