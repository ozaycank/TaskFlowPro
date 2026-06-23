using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Velyo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBillingFieldsToWorkspace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CurrentPeriodEnd",
                table: "Workspaces",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerId",
                table: "Workspaces",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionPlanId",
                table: "Workspaces",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionStatus",
                table: "Workspaces",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPeriodEnd",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "SubscriptionPlanId",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "SubscriptionStatus",
                table: "Workspaces");
        }
    }
}
