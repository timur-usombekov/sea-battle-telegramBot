using SeaBattleBot.Core.Controllers;
using SeaBattleBot.Core.Domain.Contracts.Controllers;

namespace ShipControllerTests
{
    public class SetCrossAroundShipTests
	{
		private readonly IShipController _shipController;
		public SetCrossAroundShipTests()
		{
			_shipController = new ShipController();
		}

		[Fact]
		public async Task SetCrossAroundShip_DestroyedCruiser()
		{
			var inputField = new byte[10, 10]
			{
				{1, 0, 0, 0, 0, 1, 1, 0, 0, 0},
				{1, 0, 1, 0, 0, 0, 0, 0, 1, 0},
				{1, 0, 1, 0, 3, 3, 3, 0, 1, 0},
				{1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
			};
			var expectedField = new byte[10, 10]
			{
				{1, 0, 0, 0, 0, 1, 1, 0, 0, 0},
				{1, 0, 1, 2, 2, 2, 2, 2, 1, 0},
				{1, 0, 1, 2, 3, 3, 3, 2, 1, 0},
				{1, 0, 0, 2, 2, 2, 2, 2, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
			};
			await _shipController.SetCrossAroundShip(inputField, 2, 5);
			Assert.Equal(expectedField, inputField);
		}
	}
}
