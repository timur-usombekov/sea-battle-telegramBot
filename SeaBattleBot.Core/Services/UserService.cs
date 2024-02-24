using SeaBattleBot.Core.Domain.Contracts.Repository;
using SeaBattleBot.Core.Domain.Contracts.Services;
using SeaBattleBot.Core.Domain.Entities;
using SeaBattleBot.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleBot.Core.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IEnemyStateRepository _enemyStateRepository;
		private readonly IGameStateRepository _gameStateRepository;
		public UserService(IUserRepository userRepository, IEnemyStateRepository enemyStateRepository,
			IGameStateRepository gameStateRepository)
		{
			_userRepository = userRepository;
			_enemyStateRepository = enemyStateRepository;
			_gameStateRepository = gameStateRepository;
		}
		public async Task<User> AddNewPlayer(long chatId)
		{
			var enemyState = await _enemyStateRepository.AddEnemyState(new EnemyState());
			var gameState = await _gameStateRepository.AddGameState(new GameState()
			{
				GameStatus = GameStatus.NotStarted,
				EnemyFieldAsJson = "",
				PlayerFieldAsJson = "",
				EnemyStateId = enemyState.Id,
			});
			var user = await _userRepository.AddUser(new User()
			{
				ChatId = chatId,
				GameStateId = gameState.Id,
			});
			return user;
		}

		public async Task<bool> CheckIfPlayerExist(long chatId)
		{
			return await _userRepository.GetUserByChatId(chatId) is not null;
		}

		public async Task<bool> RestartPlayerGame(long chatId)
		{
			try
			{
				var user = await _userRepository.GetUserByChatId(chatId);
				var userGameState = await _gameStateRepository.GetGameStateByUserId(user.Id);
				userGameState.GameStatus = GameStatus.NotStarted;
				userGameState.EnemyFieldAsJson = null;
				userGameState.PlayerFieldAsJson = null;
				await _gameStateRepository.UpdateGameState(userGameState);
				var enemyState = await _gameStateRepository.GetEnemyStateByGameStateId(userGameState.Id);
				enemyState.LastMoveShipDirection = null;
				enemyState.FirstHittedMoveColumn = null;
				enemyState.FirstHittedMoveRow = null;
				enemyState.LastHittedMoveColumn = null;
				enemyState.LastHittedMoveRow = null;
				enemyState.LastMoveHitted = false;
				await _enemyStateRepository.UpdateEnemyState(enemyState);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"RestartPlayerGame exception - {ex.Message}");
				return false;
			}
			
		}
	}
}
