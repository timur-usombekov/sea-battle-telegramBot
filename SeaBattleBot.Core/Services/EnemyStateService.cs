using SeaBattleBot.Core.Domain.Contracts.Repository;
using SeaBattleBot.Core.Domain.Contracts.Services;
using SeaBattleBot.Core.Domain.Entities;
using SeaBattleBot.Core.Enums;
using System.Data.Common;

namespace SeaBattleBot.Core.Services
{
	public class EnemyStateService : IEnemyStateService
	{
		private readonly IEnemyStateRepository _enemyStateRepository;
		private readonly IGameStateRepository _gameStateRepository;
		private readonly IUserRepository _userRepository;
		public EnemyStateService(IEnemyStateRepository enemyStateRepository, IUserRepository userRepository, IGameStateRepository gameStateRepository)
		{
			_enemyStateRepository = enemyStateRepository;
			_userRepository = userRepository;
			_gameStateRepository = gameStateRepository;
		}

		public async Task<EnemyState> GetCurrentEnemyState(long chatId)
		{
			return await GetEnemyStateByChatId(chatId);
		}

		public async Task<bool> UpdaLastMoveHitted(long chatId, bool lastMoveHitted)
		{
			var state = await GetEnemyStateByChatId(chatId);
			state.LastMoveHitted = lastMoveHitted;
			await _enemyStateRepository.UpdateEnemyState(state);
			return true;
		}

		public async Task<(byte?, byte?)> UpdateFirstHittedMove(long chatId, byte? row, byte? column)
		{
			var state = await GetEnemyStateByChatId(chatId);
			state.FirstHittedMoveRow = row;
			state.FirstHittedMoveColumn = column;
			await _enemyStateRepository.UpdateEnemyState(state);
			return (state.FirstHittedMoveRow, state.FirstHittedMoveColumn);
		}

		public async Task<(byte?, byte?)> UpdateLastHittedMove(long chatId, byte? row, byte? column)
		{
			var state = await GetEnemyStateByChatId(chatId);
			state.LastHittedMoveRow = row;
			state.LastHittedMoveColumn = column;
			await _enemyStateRepository.UpdateEnemyState(state);
			return (state.FirstHittedMoveRow, state.FirstHittedMoveColumn);
		}

		public async Task<ShipDirection?> UpdateLastMoveShipDirection(long chatId, ShipDirection? shipDirection)
		{
			var state = await GetEnemyStateByChatId(chatId);
			state.LastMoveShipDirection = shipDirection;
			await _enemyStateRepository.UpdateEnemyState(state);
			return state.LastMoveShipDirection;
		}

		private async Task<EnemyState> GetEnemyStateByChatId(long chatId)
		{
			var user = await _userRepository.GetUserByChatId(chatId) ??
				throw new ArgumentException("User is null");
			var gameState = await _gameStateRepository.GetGameStateByUserId(user.Id) ??
				throw new ArgumentException("GameState is null");
			var enemyState = await _enemyStateRepository.GetEnemyStateById(gameState.EnemyStateId) ??
				throw new ArgumentException("EnemyState is null");
			return enemyState;
		}
	}
}
