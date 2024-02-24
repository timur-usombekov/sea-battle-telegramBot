using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaBattleBot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRefs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamesState_Users_UserId",
                table: "GamesState");

            migrationBuilder.DropIndex(
                name: "IX_GamesState_EnemyStateId",
                table: "GamesState");

            migrationBuilder.DropIndex(
                name: "IX_GamesState_UserId",
                table: "GamesState");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GamesState");

            migrationBuilder.DropColumn(
                name: "GameStateId",
                table: "EnemiesState");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GameStateId",
                table: "Users",
                column: "GameStateId");

            migrationBuilder.CreateIndex(
                name: "IX_GamesState_EnemyStateId",
                table: "GamesState",
                column: "EnemyStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_GamesState_GameStateId",
                table: "Users",
                column: "GameStateId",
                principalTable: "GamesState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_GamesState_GameStateId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_GameStateId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_GamesState_EnemyStateId",
                table: "GamesState");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "GamesState",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "GameStateId",
                table: "EnemiesState",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_GamesState_EnemyStateId",
                table: "GamesState",
                column: "EnemyStateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GamesState_UserId",
                table: "GamesState",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GamesState_Users_UserId",
                table: "GamesState",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
