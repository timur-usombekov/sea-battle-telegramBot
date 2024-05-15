using SeaBattleBot.Core.AI;
using SeaBattleBot.Core.Controllers;
using SeaBattleBot.Core.Domain.Contracts.Controllers;
using SeaBattleBot.Core.Domain.Contracts.Services;
using NSubstitute;
using SeaBattleBot.Core.Services;

namespace BotAITests
{
	public class FindAtackableCellTests
	{
		private readonly BotAI _botAI; 
		public FindAtackableCellTests()
		{
            var shipController = new ShipController();
			var fieldController = new FieldController(shipController);
			var enemyStateService = Substitute.For<IEnemyStateService>();
			var gameStateService = Substitute.For<IGameStateService>();
            _botAI = new BotAI(fieldController, shipController, enemyStateService, gameStateService);
        }
        [Fact]
		public async Task FindAtackableCell_OneAtackableCellAround()
		{
			var field = new byte[5, 5]
			{
				{0, 0, 0, 0, 0},
				{0, 2, 2, 0, 0},
				{0, 2, 3, 2, 0},
				{0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0},
			};
			Assert.Equal<(byte, byte)>((3, 2), await _botAI.FindAttackableCell(field, 2, 2));
		}
		[Fact]
		public async Task FindAtackableCell_TwoAtackableCellAround()
		{
			var field = new byte[5, 5]
			{
				{0, 0, 0, 0, 0},
				{0, 2, 2, 0, 0},
				{0, 0, 3, 2, 0},
				{0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0},
			};
			List<(byte, byte)> someVariants = new() { (2, 1), (3, 2) };
			Assert.Contains(await _botAI.FindAttackableCell(field, 2, 2), someVariants);
		}
		[Fact]
		public async Task FindAtackableCell_ThreeAtackableCellAround()
		{
			var field = new byte[5, 5]
			{
				{0, 0, 0, 0, 0},
				{0, 2, 0, 0, 0},
				{0, 0, 3, 2, 0},
				{0, 2, 0, 0, 0},
				{0, 0, 0, 0, 0},
			};
			List<(byte, byte)> someVariants = new() { (2, 1), (3, 2), (1, 2) };
			Assert.Contains(await _botAI.FindAttackableCell(field, 2, 2), someVariants);
		}
		[Fact]
		public async Task FindAtackableCell_FourAtackableCellAround()
		{
			var field = new byte[5, 5]
			{
				{0, 0, 0, 0, 0},
				{0, 2, 0, 0, 0},
				{0, 0, 3, 0, 0},
				{0, 2, 0, 0, 0},
				{0, 0, 0, 0, 0},
			};
			List<(byte, byte)> someVariants = new() { (2, 1), (3, 2), (1, 2), (2, 3) };
			Assert.Contains(await _botAI.FindAttackableCell(field, 2, 2), someVariants);
		}
		[Fact]
		public async Task FindAtackableCell_ZeroAtackableCellAround()
		{
			var field = new byte[5, 5]
			{
				{0, 0, 0, 0, 0},
				{0, 2, 2, 2, 0},
				{0, 2, 3, 2, 0},
				{0, 2, 2, 2, 0},
				{0, 0, 0, 0, 0},
			};
			await Assert.ThrowsAsync<ArgumentException>(async () =>
			{
				await _botAI.FindAttackableCell(field, 2, 2);
			});
		}
	}
}