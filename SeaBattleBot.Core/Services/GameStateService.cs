using Newtonsoft.Json;
using SeaBattleBot.Core.Domain.Contracts.Repository;
using SeaBattleBot.Core.Domain.Contracts.Services;
using SeaBattleBot.Core.Domain.Entities;
using SeaBattleBot.Core.Enums;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;

namespace SeaBattleBot.Core.Services
{
	public class GameStateService : IGameStateService
	{
		private readonly IGameStateRepository _gameStateRepository;
		private readonly IUserRepository _userRepository;
		public GameStateService(IGameStateRepository gameStateRepository, IUserRepository userRepository)
		{
			_gameStateRepository = gameStateRepository;
			_userRepository = userRepository;
		}

		public async Task<GameStatus> ChangeGameStatus(long chatId, GameStatus gameStatus)
		{
			var gameState = await GetGameStateByChatId(chatId);

			gameState.GameStatus = gameStatus;
			await _gameStateRepository.UpdateGameState(gameState);
			return gameState.GameStatus;
		}
		public async Task<byte[,]?> GetCurrentEnemyField(long chatId)
		{
			var gameState = await GetGameStateByChatId(chatId);
			if (gameState.EnemyFieldAsJson is null)
			{
				return null;
			}
			var field = JsonConvert.DeserializeObject<byte[,]>(gameState.EnemyFieldAsJson);
			return field;
		}
		public async Task<byte[,]?> GetCurrentPlayerField(long chatId)
		{
			var gameState = await GetGameStateByChatId(chatId);
			if(gameState.PlayerFieldAsJson is null)
			{
				return null;
			}
			var field = JsonConvert.DeserializeObject<byte[,]>(gameState.PlayerFieldAsJson);
			return field;
		}
		public async Task<GameStatus> GetGameStatus(long chatId)
		{
			var gameState = await GetGameStateByChatId(chatId);
			return gameState.GameStatus;
		}
		public async Task<byte[,]?> UpdateEnemyField(long chatId, byte[,]? enemyField)
		{
			var gameState = await GetGameStateByChatId(chatId);
			var field = JsonConvert.SerializeObject(enemyField);
			gameState.EnemyFieldAsJson = field;
			await _gameStateRepository.UpdateGameState(gameState);
			return enemyField;
		}
		public async Task<byte[,]?> UpdatePlayerField(long chatId, byte[,]? playerField)
		{
			var gameState = await GetGameStateByChatId(chatId);
			var field = JsonConvert.SerializeObject(playerField);
			gameState.PlayerFieldAsJson = field;
			await _gameStateRepository.UpdateGameState(gameState);
			return playerField;
		}

		private async Task<GameState> GetGameStateByChatId(long chatId)
		{
			var user = await _userRepository.GetUserByChatId(chatId) ??
				throw new ArgumentException("User is null");
			var gameState = await _gameStateRepository.GetGameStateByUserId(user.Id) ??
				throw new ArgumentException("GameState is null");
			return gameState;
		}
	}
}
