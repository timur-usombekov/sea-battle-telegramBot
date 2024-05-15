using SeaBattleBot.Core.Domain.Contracts.AI;
using SeaBattleBot.Core.Domain.Contracts.Controllers;
using SeaBattleBot.Core.Domain.Contracts.Services;
using SeaBattleBot.Core.DTOs;
using SeaBattleBot.Core.Enums;
using SeaBattleBot.Core.Exceptions;
using SeaBattleBot.Core.Helpers;

namespace SeaBattleBot.Core.Controllers
{
    public class GameController : IGameController
	{
		private readonly IFieldController _fieldController;
		private readonly IBotPlayer _bot;
		private readonly IUserService _userService;
		private readonly IGameStateService _gameStateService;

		public GameController(IFieldController fieldController, IBotPlayer bot, 
			IUserService userService, IGameStateService gameStateService) 
		{ 
			_fieldController = fieldController;
			_bot = bot;
			_userService = userService;
			_gameStateService = gameStateService;
		}

		public async Task StartNewGame(long chatId)
		{
			if(!await _userService.CheckIfPlayerExist(chatId))
				await _userService.AddNewPlayer(chatId);
			await _userService.RestartPlayerGame(chatId);
			
		}
		public async Task<MoveResult> PlayerMakeMove(long chatId, string move)
		{
			if (!await MyValidator.ValidateMove(move))
			{
				return new MoveResult() { IsSuccess = false, ErrorMessage = "This move is not possible" };
			}
			(byte i, byte j) = await Converter.ConvertMoveToIndex(move);
			var field = await _gameStateService.GetCurrentEnemyField(chatId);
			if (field == null)
			{
				throw new ArgumentException("Player field is null for move");
			}
			var moveResult = await _fieldController.MakeMove(field, i, j);
			if (moveResult.IsSuccess)
			{
				await _gameStateService.UpdateEnemyField(chatId, field);
				if(moveResult.IsHitted)
				{
					return moveResult;
				}
				await EnemyMakeMove(chatId);
				return moveResult;
			}
			return moveResult;
		}
		public async Task<bool> CreateField(long chatId, string? field = null)
		{
			await _gameStateService.UpdateEnemyField(chatId, await _fieldController.CreateRandomBattleField());
			if (field is null)
			{
				await _gameStateService.UpdatePlayerField(chatId, await _fieldController.CreateRandomBattleField());
				return true;
			}
			try
			{
				var Pfield = await _gameStateService.UpdatePlayerField(chatId, await _fieldController.CreateBattleField(field));
				if(await MyValidator.ValidateBattlefield(Pfield))
				{
					return true;
				}
				return false;
			}
			catch(InvalidFieldException)
			{
				return false;
			}
		}
		public async Task EnemyMakeMove(long chatId)
		{
			var field = await _gameStateService.GetCurrentPlayerField(chatId);
			await _bot.MakeMove(chatId, field);
		}
		public async Task<string> GetFields(long chatId)
		{
			var playerField = await _gameStateService.GetCurrentPlayerField(chatId);
			var enemyField = await _gameStateService.GetCurrentEnemyField(chatId);
			return
				"Your field:\n" +
				await _fieldController.GetFieldAsString(playerField) +
				"\n" +
				"Enemy field:\n" +
				await _fieldController.GetEnemyFieldAsString(enemyField);
		}
		public async Task<GameStatus> GetGameStatus(long chatId)
		{
			if(!await _userService.CheckIfPlayerExist(chatId))
			{
				return GameStatus.Finished;
			}
			var playerField = await _gameStateService.GetCurrentPlayerField(chatId);
			var enemyField = await _gameStateService.GetCurrentEnemyField(chatId);
			if(playerField is null || enemyField is null)
			{
				return GameStatus.NotStarted;
			}
			if(await _fieldController.FieldContainsNotDestroyedShip(playerField) &&
			await _fieldController.FieldContainsNotDestroyedShip(enemyField))
			{
				return GameStatus.InProgress;
			}
			return GameStatus.Finished;
		}
	}
}
