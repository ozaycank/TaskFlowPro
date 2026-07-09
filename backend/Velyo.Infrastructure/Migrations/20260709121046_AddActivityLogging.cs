using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Velyo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddActivityLogging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "ActivityLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "ActivityLogs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "ActivityLogs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "EntityType",
                table: "ActivityLogs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "ActivityLogs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                table: "ActivityLogs",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_CreatedAt",
                table: "ActivityLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_ProjectId",
                table: "ActivityLogs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_TaskId",
                table: "ActivityLogs",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_UserId",
                table: "ActivityLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_WorkspaceId",
                table: "ActivityLogs",
                column: "WorkspaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_CreatedAt",
                table: "ActivityLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_ProjectId",
                table: "ActivityLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_TaskId",
                table: "ActivityLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_UserId",
                table: "ActivityLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_WorkspaceId",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "ActivityLogs");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "ActivityLogs",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "ActivityLogs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
