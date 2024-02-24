using SeaBattleBot.Core.Domain.Contracts.Repository;
using SeaBattleBot.Core.Domain.Entities;
using SeaBattleBot.Infrastructure.Context;

namespace SeaBattleBot.Infrastructure.Repository
{
	public class EnemyStateRepository: IEnemyStateRepository
	{
		private readonly ApplicationContext _context;
		public EnemyStateRepository()
		{
			_context = new();
		}

		public async Task<EnemyState> AddEnemyState(EnemyState enemyState)
		{
			_context.EnemiesState.Add(enemyState);
			await _context.SaveChangesAsync();
			return enemyState;
		}

		public async Task<EnemyState?> GetEnemyStateById(long id)
		{
			return await _context.EnemiesState.FindAsync(id);
		}

		public async Task<EnemyState> UpdateEnemyState(EnemyState enemyState)
		{
			_context.EnemiesState.Update(enemyState);
			await _context.SaveChangesAsync();
			return enemyState;
		}
	}
}
