using SeaBattleBot.Core.Enums;

namespace SeaBattleBot.Core.Domain.Contracts.Services
{
	public interface IGameStateService
	{
		public Task<byte[,]?> GetCurrentPlayerField(long chatId);
		public Task<byte[,]?> UpdatePlayerField(long chatId, byte[,]? playerField);
		public Task<byte[,]?> GetCurrentEnemyField(long chatId);
		public Task<byte[,]?> UpdateEnemyField(long chatId, byte[,]? enemyField);
		public Task<GameStatus> ChangeGameStatus(long chatId, GameStatus gameStatus);
		public Task<GameStatus> GetGameStatus(long chatId);
	}
}
