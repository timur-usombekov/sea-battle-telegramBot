using SeaBattleBot.Core.Controllers;
using SeaBattleBot.Core.Domain.Contracts.AI;
using SeaBattleBot.Core.Domain.Contracts.Controllers;
using SeaBattleBot.Core.Domain.Contracts.Services;
using SeaBattleBot.Core.Enums;
using SeaBattleBot.Core.Exceptions;

namespace SeaBattleBot.Core.AI
{
    public class BotAI: IBotPlayer
	{
		private readonly IFieldController _fieldController;
		private readonly IShipController _shipController;
		private readonly IEnemyStateService _enemyStateService;
		private readonly IGameStateService _gameStateService;
		private Random _random;
		public BotAI(IFieldController fieldController, IShipController shipController, 
			IEnemyStateService enemyStateService, IGameStateService gameStateService)
		{
			_fieldController = fieldController;
			_shipController = shipController;
			_enemyStateService = enemyStateService;
			_gameStateService = gameStateService;
			_random = new Random();
		}

		public async Task<(byte, byte)?> MakeMove(long chatId, byte[,] field)
		{
			if(!await _fieldController.FieldContainsNotDestroyedShip(field)) 
				return null;
			var state = await _enemyStateService.GetCurrentEnemyState(chatId);
			if (!state.LastMoveHitted)
			{
				return await ShootInRandomPosition(chatId, field);
			}
			if (state.LastHittedMoveColumn is null && state.LastHittedMoveRow is null)
				throw new EmptyLastMoveException("Bot's last move was null for attack ship");
			return await AttackShip(chatId, field, state.LastHittedMoveRow, state.LastHittedMoveColumn);
		}
		public async Task<(byte, byte)?> AttackShip(long chatId, byte[,] field, byte? i, byte? j)
		{
			var state = await _enemyStateService.GetCurrentEnemyState(chatId);
			if (state.LastMoveShipDirection is null)
			{
				return await TryFindShipDirection(chatId, field, i, j);
			}

			List<(int, int)> dir = state.LastMoveShipDirection == ShipDirection.Vertical ?  
				new() { (1, 0), (-1, 0) } : new() { (0, -1), (0, 1) }; 
				
			var attackedPos = await TryAttackShipWithDirection(chatId, field, (byte)i, (byte)j, dir);
			if (attackedPos is null)
			{
				await MakeMove(chatId, field);
			}
			return attackedPos;
		}
		public async Task<(byte, byte)?> TryAttackShipWithDirection(long chatId, byte[,] field, byte i, byte j, List<(int, int)> directionsList)
		{
			var shuffledDir = directionsList.OrderBy(x => _random.Next()).ToList();
			bool botAttacked = false;
			foreach ((int di, int dj) in shuffledDir)
			{
				int ni = i + di, nj = j + dj;
				if (ni >= 0 && ni < field.GetLength(0) && nj >= 0 && nj < field.GetLength(1)
					&& await IsAttackableCell(field[ni, nj]))
				{
					var res = await _fieldController.MakeMove(field, (byte)ni, (byte)nj);
					await _gameStateService.UpdatePlayerField(chatId, field);
					botAttacked = res.IsSuccess;
					if (res.IsSuccess && res.IsHitted)
					{
						await _enemyStateService.UpdateLastHittedMove(chatId, (byte)ni, (byte)nj);
						if (await _shipController.IsShipDestroyed(field, ni, nj))
						{
							await _enemyStateService.UpdaLastMoveHitted(chatId, false);
							await _enemyStateService.UpdateLastMoveShipDirection(chatId, null);
							await MakeMove(chatId, field);
							return ((byte)ni, (byte)nj);
						}
						await _enemyStateService.UpdaLastMoveHitted(chatId, true);
						await MakeMove(chatId, field);
						return ((byte)ni, (byte)nj);
					}
				}
			}
			var state = await _enemyStateService.GetCurrentEnemyState(chatId);
			if (botAttacked == false)
			{
				await _enemyStateService.UpdateLastHittedMove(chatId, state.FirstHittedMoveRow, state.FirstHittedMoveColumn);
				return null;
			}
			return ((byte)state.LastHittedMoveRow, (byte)state.LastHittedMoveColumn);
		}
		public async Task<(byte, byte)> TryFindShipDirection(long chatId, byte[,] field, byte? i, byte? j)
		{
			(byte di, byte dj) = await FindAttackableCell(field, (byte)i, (byte)j);
			var result = await _fieldController.MakeMove(field, di, dj);
			await _gameStateService.UpdatePlayerField(chatId, field);
			if (result.IsSuccess && result.IsHitted)
			{
				await _enemyStateService.UpdateLastHittedMove(chatId, di, dj);
				if (await _shipController.IsShipDestroyed(field, di, dj))
				{
					await _enemyStateService.UpdaLastMoveHitted(chatId, false);
					await _enemyStateService.UpdateLastMoveShipDirection(chatId, null);
					await MakeMove(chatId, field);
					return (di, dj);
				}
				await _enemyStateService.UpdaLastMoveHitted(chatId, true);
				await _enemyStateService.UpdateLastMoveShipDirection(chatId, await _shipController.GetShipDirection(field, di, dj));
				await MakeMove(chatId, field);
			}
			return (di, dj);
		}
		public async Task<(byte, byte)> FindAttackableCell(byte[,] field, byte i, byte j)
		{
			List<(int, int)> dir = new() { (-1, 0), (1, 0), (0, -1), (0, 1) }; // up, down, left, right
			var shuffledDir = dir.OrderBy(x => _random.Next()).ToList();
			foreach ((int di,int dj) in shuffledDir)
			{
				int ni = i + di, nj = j + dj;
				if (ni >= 0 && ni < field.GetLength(0) && nj >= 0 && nj < field.GetLength(1)
					&& await IsAttackableCell(field[ni, nj]))
				{
					return ((byte)ni, (byte)nj);
				}
			}
			throw new ArgumentException("Atackable cells around ship doesn't exist");
		}
		public async Task<(byte, byte)> ShootInRandomPosition(long chatId, byte[,] field)
		{
			while (true)
			{
				byte i = (byte)_random.Next(0, FieldController.FieldSize);
				byte j = (byte)_random.Next(0, FieldController.FieldSize);
				var result = await _fieldController.MakeMove
					(field, i, j);
				if (result.IsSuccess)
				{
					await _enemyStateService.UpdaLastMoveHitted(chatId, false);
					await _gameStateService.UpdatePlayerField(chatId, field);
					if (result.IsHitted)
					{
						await _enemyStateService.UpdateLastHittedMove(chatId, i, j);
						await _enemyStateService.UpdateFirstHittedMove(chatId, i, j);
						await _enemyStateService.UpdaLastMoveHitted(chatId, !await _shipController.IsShipDestroyed(field, i, j));
						await MakeMove(chatId, field);
					}
					return (i, j);
				}
			}
		}
		private async Task <bool> IsAttackableCell(byte cell)
		{
			switch (cell) 
			{ 
				case 0:
				case 1:
					return true;
				default: 
					return false;
			}
		}
	}
}
