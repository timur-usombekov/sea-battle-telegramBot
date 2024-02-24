using SeaBattleBot.Core.Domain.Entities;

namespace SeaBattleBot.Core.Domain.Contracts.Services
{
	public interface IUserService
	{
		public Task<bool> CheckIfPlayerExist(long chatId);
		public Task<User> AddNewPlayer(long chatId);
		public Task<bool> RestartPlayerGame(long chatId);
	}
}
