using Microsoft.Extensions.DependencyInjection;
using SeaBattleBot.Core.AI;
using SeaBattleBot.Core.Controllers;
using SeaBattleBot.Core.Domain.Contracts.AI;
using SeaBattleBot.Core.Domain.Contracts.Controllers;
using SeaBattleBot.Core.Domain.Contracts.Repository;
using SeaBattleBot.Core.Domain.Contracts.Services;
using SeaBattleBot.Core.Services;
using SeaBattleBot.Infrastructure.Repository;

namespace SeaBattleBot.Extensions
{
	public static class ConfigureServices
	{
		public static void ConfigureControllerWithRepositories(IServiceCollection services)
		{
			services.AddScoped<IFieldController, FieldController>();
			services.AddScoped<IGameController, GameController>();
			services.AddScoped<IShipController, ShipController>();
			services.AddScoped<IBotPlayer, BotAI>();

			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IGameStateRepository, GameStateRepository>();
			services.AddScoped<IEnemyStateRepository, EnemyStateRepository>();

			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IGameStateService, GameStateService>();
			services.AddScoped<IEnemyStateService, EnemyStateService>();
		}
	}
}
