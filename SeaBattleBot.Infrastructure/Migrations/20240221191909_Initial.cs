using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaBattleBot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnemiesState",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameStateId = table.Column<long>(type: "INTEGER", nullable: false),
                    LastHittedMoveRow = table.Column<byte>(type: "INTEGER", nullable: true),
                    LastHittedMoveColumn = table.Column<byte>(type: "INTEGER", nullable: true),
                    FirstHittedMoveRow = table.Column<byte>(type: "INTEGER", nullable: true),
                    FirstHittedMoveColumn = table.Column<byte>(type: "INTEGER", nullable: true),
                    LastMoveShipDirection = table.Column<int>(type: "INTEGER", nullable: true),
                    LastMoveHitted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnemiesState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    ChatId = table.Column<long>(type: "INTEGER", nullable: false),
                    GameStateId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GamesState",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    EnemyStateId = table.Column<long>(type: "INTEGER", nullable: false),
                    PlayerFieldAsJson = table.Column<string>(type: "TEXT", nullable: false),
                    EnemyFieldAsJson = table.Column<string>(type: "TEXT", nullable: false),
                    GameStatus = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamesState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GamesState_EnemiesState_EnemyStateId",
                        column: x => x.EnemyStateId,
                        principalTable: "EnemiesState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamesState_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamesState");

            migrationBuilder.DropTable(
                name: "EnemiesState");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
