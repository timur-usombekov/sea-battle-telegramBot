using SeaBattleBot.Core.Domain.Entities;
using SeaBattleBot.Core.Enums;

namespace SeaBattleBot.Core.Domain.Contracts.Services
{
	public interface IEnemyStateService
	{
		public Task<(byte?,byte?)> UpdateLastHittedMove(long chatId, byte? row, byte? column);
		public Task<(byte?,byte?)> UpdateFirstHittedMove(long chatId, byte? row, byte? column);
		public Task<ShipDirection?> UpdateLastMoveShipDirection(long chatId, ShipDirection? shipDirection);
		public Task<EnemyState> GetCurrentEnemyState(long chatId);
		public Task<bool> UpdaLastMoveHitted(long chatId, bool lastMoveHitted);
	}
}
