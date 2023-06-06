using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyBudget.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBudgetIdFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Budgets_BudgetId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_BudgetId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BudgetId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserBudgets",
                columns: table => new
                {
                    SharedBudgetsId = table.Column<Guid>(type: "uuid", nullable: false),
                    SharedUsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBudgets", x => new { x.SharedBudgetsId, x.SharedUsersId });
                    table.ForeignKey(
                        name: "FK_UserBudgets_Budgets_SharedBudgetsId",
                        column: x => x.SharedBudgetsId,
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBudgets_Users_SharedUsersId",
                        column: x => x.SharedUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBudgets_SharedUsersId",
                table: "UserBudgets",
                column: "SharedUsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBudgets");

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_BudgetId",
                table: "Users",
                column: "BudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Budgets_BudgetId",
                table: "Users",
                column: "BudgetId",
                principalTable: "Budgets",
                principalColumn: "Id");
        }
    }
}
