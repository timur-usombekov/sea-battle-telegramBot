using Microsoft.EntityFrameworkCore;
using SeaBattleBot.Core.Domain.Contracts.Repository;
using SeaBattleBot.Core.Domain.Entities;
using SeaBattleBot.Infrastructure.Context;

namespace SeaBattleBot.Infrastructure.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationContext _context;
		public UserRepository()
		{
			_context = new();
		}
		public async Task<User> AddUser(User user)
		{
			_context.Users.Add(user);
			await _context.SaveChangesAsync();
			return user;
		}

		public async Task<User?> GetUserByChatId(long chatId)
		{
			return await _context.Users.FirstOrDefaultAsync(user => user.ChatId == chatId);
		}

		public async Task<GameState?> GetUserGameState(long userId)
		{
			var user = await _context.Users.Include(user => user.GameState).FirstOrDefaultAsync(user => user.Id == userId);
			if(user == null || user.GameState == null) { return null; }
			return user.GameState;
		}
	}
}
