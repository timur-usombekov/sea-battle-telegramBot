using SeaBattleBot.Core.Controllers;
using SeaBattleBot.Core.Domain.Contracts.Controllers;

namespace ShipControllerTests
{
    public class ShipSizeTests
	{
		private readonly IShipController _shipController;
		public ShipSizeTests()
		{
			_shipController = new ShipController();
		}

		[Theory]
		[InlineData(1,2)]
		[InlineData(3,3)]
		[InlineData(5,9)]
		[InlineData(8,7)]
		[InlineData(1,9)]
		[InlineData(5,1)]
		[InlineData(8,4)]
		public async Task GetShipSize_Battleships(int i, int j )
		{
			byte[,] field =
			{
				{0,0,0,0,0,0,0,0,0,1 },
				{0,1,1,1,1,0,0,0,0,1 },
				{0,0,0,0,0,0,0,0,0,1 },
				{0,0,0,1,1,1,1,0,0,1 },
				{0,1,0,0,0,0,0,0,0,0 },
				{0,1,0,0,0,0,1,1,1,1 },
				{0,1,0,0,1,0,0,0,0,0 },
				{0,1,0,0,1,0,0,0,0,0 },
				{0,0,0,0,1,0,1,1,1,1 },
				{0,0,0,0,1,0,0,0,0,0 }
			};

			var ship = await _shipController.GetShipSize(field, i, j);

			Assert.True(ship == 4);
		}

		[Theory]
		[InlineData(1, 2)]
		[InlineData(3, 3)]
		[InlineData(5, 9)]
		[InlineData(8, 7)]
		[InlineData(1, 9)]
		[InlineData(5, 1)]
		[InlineData(8, 4)]
		public async Task GetShipSize_Cruisers(int i, int j)
		{
			byte[,] field =
			{
				{0,0,0,0,0,0,0,0,0,0 },
				{0,1,1,1,0,0,0,0,0,1 },
				{0,0,0,0,0,0,0,0,0,1 },
				{0,0,0,1,1,1,0,0,0,1 },
				{0,1,0,0,0,0,0,0,0,0 },
				{0,1,0,0,0,0,0,1,1,1 },
				{0,1,0,0,0,0,0,0,0,0 },
				{0,0,0,0,1,0,0,0,0,0 },
				{0,0,0,0,1,0,0,1,1,1 },
				{0,0,0,0,1,0,0,0,0,0 }
			};

			var ship = await _shipController.GetShipSize(field, i, j);

			Assert.True(ship == 3);
		}

		[Theory]
		[InlineData(1, 2)]
		[InlineData(3, 3)]
		[InlineData(5, 9)]
		[InlineData(8, 8)]
		[InlineData(1, 9)]
		[InlineData(5, 1)]
		[InlineData(8, 4)]
		public async Task GetShipSize_Destroyers(int i, int j)
		{
			byte[,] field =
			{
				{0,0,0,0,0,0,0,0,0,0 },
				{0,1,1,0,0,0,0,0,0,1 },
				{0,0,0,0,0,0,0,0,0,1 },
				{0,0,0,1,1,0,0,0,0,0 },
				{0,0,0,0,0,0,0,0,0,0 },
				{0,1,0,0,0,0,0,0,1,1 },
				{0,1,0,0,0,0,0,0,0,0 },
				{0,0,0,0,1,0,0,0,0,0 },
				{0,0,0,0,1,0,0,0,1,1 },
				{0,0,0,0,0,0,0,0,0,0 }
			};

			var ship = await _shipController.GetShipSize(field, i, j);

			Assert.True(ship == 2);
		}

		[Theory]
		[InlineData(1, 1)]
		[InlineData(3, 3)]
		[InlineData(5, 9)]
		[InlineData(8, 8)]
		[InlineData(1, 9)]
		[InlineData(5, 1)]
		[InlineData(8, 4)]
		public async Task GetShipSize_Patrols(int i, int j)
		{
			byte[,] field =
			{
				{0,0,0,0,0,0,0,0,0,0 },
				{0,1,0,0,0,0,0,0,0,1 },
				{0,0,0,0,0,0,0,0,0,0 },
				{0,0,0,1,0,0,0,0,0,0 },
				{0,0,0,0,0,0,0,0,0,0 },
				{0,1,0,0,0,0,0,0,0,1 },
				{0,0,0,0,0,0,0,0,0,0 },
				{0,0,0,0,0,0,0,0,0,0 },
				{0,0,0,0,1,0,0,0,1,0 },
				{0,0,0,0,0,0,0,0,0,0 }
			};

			var ship = await _shipController.GetShipSize(field, i, j);

			Assert.True(ship == 1);
		}
	}
}