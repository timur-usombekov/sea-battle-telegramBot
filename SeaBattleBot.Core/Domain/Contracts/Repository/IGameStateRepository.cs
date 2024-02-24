using SeaBattleBot.Core.Domain.Entities;

namespace SeaBattleBot.Core.Domain.Contracts.Repository
{
	public interface IGameStateRepository
	{
		public Task<GameState> AddGameState(GameState gameState);
		public Task<GameState> UpdateGameState(GameState gameState);
		public Task<GameState?> GetGameStateByUserId(long userId);
		public Task<EnemyState?> GetEnemyStateByGameStateId(long gameStateId);
	}
}
