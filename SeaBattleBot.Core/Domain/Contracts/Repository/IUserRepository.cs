using SeaBattleBot.Core.Domain.Entities;

namespace SeaBattleBot.Core.Domain.Contracts.Repository
{
	public interface IUserRepository
	{
		public Task<User> AddUser(User user);
		public Task<User?> GetUserByChatId(long chatId);
		public Task<GameState?> GetUserGameState(long userId);
	}
}
