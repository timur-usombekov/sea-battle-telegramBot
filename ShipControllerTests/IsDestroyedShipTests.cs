using SeaBattleBot.Core.Controllers;
using SeaBattleBot.Core.Domain.Contracts.Controllers;

namespace ShipControllerTests
{
    public class IsDestroyedShipTests
	{
		private readonly IShipController _shipController;
		public IsDestroyedShipTests()
		{
			_shipController = new ShipController();
		}

		[Theory]
		[InlineData(1, 2)]
		[InlineData(3, 3)]
		[InlineData(5, 9)]
		[InlineData(8, 7)]
		[InlineData(1, 9)]
		[InlineData(5, 1)]
		[InlineData(8, 4)]
		public async Task IsShipDestroyed_Battleships(int i, int j)
		{
			byte[,] field =
			{
				{0,0,0,0,0,0,0,0,0,3 },
				{0,3,3,3,3,0,0,0,0,3 },
				{0,0,0,0,0,0,0,0,0,3 },
				{0,0,0,3,3,3,3,0,0,3 },
				{0,3,0,0,0,0,0,0,0,0 },
				{0,3,0,0,0,0,3,3,3,3 },
				{0,3,0,0,3,0,0,0,0,0 },
				{0,3,0,0,3,0,0,0,0,0 },
				{0,0,0,0,3,0,3,3,3,3 },
				{0,0,0,0,3,0,0,0,0,0 }
			};

			Assert.True(await _shipController.IsShipDestroyed(field, i, j));
		}

		[Theory]
		[InlineData(1, 2)]
		[InlineData(3, 3)]
		[InlineData(5, 9)]
		[InlineData(8, 7)]
		[InlineData(1, 9)]
		[InlineData(5, 1)]
		[InlineData(8, 4)]
		public async Task IsShipDestroyed_Cruisers(int i, int j)
		{
			byte[,] field =
			{
				{0,0,0,0,0,0,0,0,0,0 },
				{0,3,3,3,0,0,0,0,0,3 },
				{0,0,0,0,0,0,0,0,0,3 },
				{0,0,0,3,3,3,0,0,0,3 },
				{0,3,0,0,0,0,0,0,0,0 },
				{0,3,0,0,0,0,0,3,3,3 },
				{0,3,0,0,0,0,0,0,0,0 },
				{0,0,0,0,3,0,0,0,0,0 },
				{0,0,0,0,3,0,0,3,3,3 },
				{0,0,0,0,3,0,0,0,0,0 }
			};

			Assert.True(await _shipController.IsShipDestroyed(field, i, j));
		}

		[Theory]
		[InlineData(1, 2)]
		[InlineData(3, 3)]
		[InlineData(5, 9)]
		[InlineData(8, 8)]
		[InlineData(1, 9)]
		[InlineData(5, 1)]
		[InlineData(8, 4)]
		public async Task IsShipDestroyed_Destroyers(int i, int j)
		{
			byte[,] field =
			{
				{0,0,0,0,0,0,0,0,0,0 },
				{0,3,3,0,0,0,0,0,0,3 },
				{0,0,0,0,0,0,0,0,0,3 },
				{0,0,0,3,3,0,0,0,0,0 },
				{0,0,0,0,0,0,0,0,0,0 },
				{0,3,0,0,0,0,0,0,3,3 },
				{0,3,0,0,0,0,0,0,0,0 },
				{0,0,0,0,3,0,0,0,0,0 },
				{0,0,0,0,3,0,0,0,3,3 },
				{0,0,0,0,0,0,0,0,0,0 }
			};

			Assert.True(await _shipController.IsShipDestroyed(field, i, j));
		}

		[Theory]
		[InlineData(1, 1)]
		[InlineData(3, 3)]
		[InlineData(5, 9)]
		[InlineData(8, 8)]
		[InlineData(1, 9)]
		[InlineData(5, 1)]
		[InlineData(8, 4)]
		public async Task IsShipDestroyed_Patrols(int i, int j)
		{
			byte[,] field =
			{
				{0,0,0,0,0,0,0,0,0,0 },
				{0,3,0,0,0,0,0,0,0,3 },
				{0,0,0,0,0,0,0,0,0,0 },
				{0,0,0,3,0,0,0,0,0,0 },
				{0,0,0,0,0,0,0,0,0,0 },
				{0,3,0,0,0,0,0,0,0,3 },
				{0,0,0,0,0,0,0,0,0,0 },
				{0,0,0,0,0,0,0,0,0,0 },
				{0,0,0,0,3,0,0,0,3,0 },
				{0,0,0,0,0,0,0,0,0,0 }
			};

			Assert.True(await _shipController.IsShipDestroyed(field, i, j));
		}
	}
}
