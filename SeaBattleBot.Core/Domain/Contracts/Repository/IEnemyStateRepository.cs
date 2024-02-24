using SeaBattleBot.Core.Domain.Entities;

namespace SeaBattleBot.Core.Domain.Contracts.Repository
{
	public interface IEnemyStateRepository
	{
		public Task<EnemyState> AddEnemyState(EnemyState enemyState);
		public Task<EnemyState> UpdateEnemyState(EnemyState enemyState);
		public Task<EnemyState?> GetEnemyStateById(long id);
	}
}
