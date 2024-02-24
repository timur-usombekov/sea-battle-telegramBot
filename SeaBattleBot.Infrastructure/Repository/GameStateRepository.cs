using Microsoft.EntityFrameworkCore;
using SeaBattleBot.Core.Domain.Contracts.Repository;
using SeaBattleBot.Core.Domain.Entities;
using SeaBattleBot.Infrastructure.Context;

namespace SeaBattleBot.Infrastructure.Repository
{
	public class GameStateRepository: IGameStateRepository
	{
		private readonly ApplicationContext _context;
		public GameStateRepository()
		{
			_context = new();
		}

		public async Task<GameState> AddGameState(GameState gameState)
		{
			_context.GamesState.Add(gameState);
			await _context.SaveChangesAsync();
			return gameState;
		}

		public async Task<EnemyState?> GetEnemyStateByGameStateId(long gameStateId)
		{
			var gameState = await _context.GamesState.Include(state => state.EnemyState)
				.FirstOrDefaultAsync(state => state.Id == gameStateId);
			if(gameState == null) { return null; }
			return gameState.EnemyState;
		}

		public async Task<GameState?> GetGameStateByUserId(long userId)
		{
			var user = await _context.Users.FindAsync(userId);
			if(user == null) { return null; }
			return await _context.GamesState.FindAsync(user.GameStateId);
		}

		public async Task<GameState> UpdateGameState(GameState gameState)
		{
			_context.GamesState.Update(gameState);
			await _context.SaveChangesAsync();
			return gameState;
		}
	}
}
