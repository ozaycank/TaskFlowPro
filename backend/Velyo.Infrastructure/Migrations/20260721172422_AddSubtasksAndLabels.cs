using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Velyo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubtasksAndLabels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Users",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "Labels",
                table: "TaskItems",
                type: "text[]",
                nullable: false,
                defaultValueSql: "'{}'");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentTaskId",
                table: "TaskItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_ParentTaskId",
                table: "TaskItems",
                column: "ParentTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_TaskItems_ParentTaskId",
                table: "TaskItems",
                column: "ParentTaskId",
                principalTable: "TaskItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_TaskItems_ParentTaskId",
                table: "TaskItems");

            migrationBuilder.DropIndex(
                name: "IX_TaskItems_ParentTaskId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "Labels",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "ParentTaskId",
                table: "TaskItems");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);
        }
    }
}
