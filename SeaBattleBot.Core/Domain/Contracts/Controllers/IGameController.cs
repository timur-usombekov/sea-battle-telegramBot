using SeaBattleBot.Core.DTOs;
using SeaBattleBot.Core.Enums;

namespace SeaBattleBot.Core.Domain.Contracts.Controllers
{
    public interface IGameController
    {
        public Task<bool> CreateField(long chatId, string? field = null);
        public Task<string> GetFields(long chatId);
        public Task<GameStatus> GetGameStatus(long chatId);
        public Task<MoveResult> PlayerMakeMove(long chatId, string move);
        public Task EnemyMakeMove(long chatId);
        public Task StartNewGame(long chatId);
	}
}
