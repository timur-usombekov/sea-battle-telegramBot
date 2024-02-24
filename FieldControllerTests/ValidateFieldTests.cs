using SeaBattleBot.Core.Helpers;

namespace ShipControllerTests
{
	public class ValidateFieldTests
	{
		[Fact]
		public async Task ValidateBattlefield_CorrectField()
		{
			var field = new byte[10, 10]
			{
				{1, 0, 0, 0, 0, 1, 1, 0, 0, 0},
				{1, 0, 1, 0, 0, 0, 0, 0, 1, 0},
				{1, 0, 1, 0, 1, 1, 1, 0, 1, 0},
				{1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
			};
			Assert.True(await MyValidator.ValidateBattlefield(field));
		}

		[Fact]
		public async Task ValidateBattlefield_ExtraPatrol()
		{
			var field = new byte[10, 10]
			{
				{1, 0, 0, 0, 0, 1, 1, 0, 0, 0},
				{1, 0, 1, 0, 0, 0, 0, 0, 1, 0},
				{1, 0, 1, 0, 1, 1, 1, 0, 1, 0},
				{1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 1, 0, 0, 0, 0, 0, 0, 0, 0}
			};
			Assert.False(await MyValidator.ValidateBattlefield(field));
		}

		[Fact]
		public async Task ValidateBattlefield_ExtraDestroyer()
		{
			var field = new byte[10, 10]
			{
				{1, 0, 0, 0, 0, 1, 1, 0, 0, 0},
				{1, 0, 1, 0, 0, 0, 0, 0, 1, 0},
				{1, 0, 1, 0, 1, 1, 1, 0, 1, 0},
				{1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 1, 1, 0},
				{0, 1, 0, 0, 0, 0, 0, 0, 0, 0}
			};
			Assert.False(await MyValidator.ValidateBattlefield(field));
		}

		[Fact]
		public async Task ValidateBattlefield_CornersContact()
		{
			var field = new byte[10, 10]
			{
				{1, 0, 0, 0, 0, 1, 1, 0, 0, 0},
				{1, 0, 1, 0, 0, 0, 0, 0, 1, 0},
				{1, 0, 1, 0, 1, 1, 1, 0, 1, 0},
				{1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
				{0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
			};
			Assert.False(await MyValidator.ValidateBattlefield(field));
		}

		[Fact]
		public async Task ValidateBattlefield_TooLongHorizontalShip()
		{
			var field = new byte[10, 10]
			{
				{1, 0, 0, 0, 0, 1, 1, 0, 0, 0},
				{1, 0, 1, 0, 0, 0, 0, 0, 1, 0},
				{1, 0, 1, 0, 1, 1, 1, 0, 1, 0},
				{1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 1, 1, 1, 1, 1, 1, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
			};
			Assert.False(await MyValidator.ValidateBattlefield(field));
		}

		[Fact]
		public async Task ValidateBattlefield_TooLongVerticalShip()
		{
			var field = new byte[10, 10]
			{
				{1, 0, 0, 0, 0, 1, 1, 0, 0, 0},
				{1, 0, 1, 0, 0, 0, 0, 0, 1, 0},
				{1, 0, 1, 0, 1, 1, 1, 0, 1, 0},
				{1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 1, 0, 0, 1, 0},
				{0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 1, 0, 0, 1, 0},
				{0, 0, 0, 1, 0, 1, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 1, 0, 0, 1, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
			};
			Assert.False(await MyValidator.ValidateBattlefield(field));
		}

		[Fact]
		public async Task ValidateBattlefield_EmptyField()
		{
			var field = new byte[10, 10]
			{
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
			};
			Assert.False(await MyValidator.ValidateBattlefield(field));
		}

		[Fact]
		public async Task ValidateBattlefield_EveryCellIsShip()
		{
			var field = new byte[10, 10]
			{
				{1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
				{1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
				{1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
				{1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
				{1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
				{1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
				{1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
				{1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
				{1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
				{1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
			};
			Assert.False(await MyValidator.ValidateBattlefield(field));
		}

		[Fact]
		public async Task ValidateBattlefield_TooManyBattleships()
		{
			var field = new byte[10, 10]
			{
				{0, 1, 1, 1, 1, 0, 1, 1, 1, 1},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{1, 0, 1, 0, 1, 0, 1, 0, 1, 0},
				{1, 0, 1, 0, 1, 0, 1, 0, 1, 0},
				{1, 0, 1, 0, 1, 0, 1, 0, 1, 0},
				{1, 0, 1, 0, 1, 0, 1, 0, 1, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{1, 1, 1, 1, 0, 0, 1, 1, 1, 1},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{1, 1, 1, 1, 0, 0, 1, 1, 1, 1}
			};
			Assert.False(await MyValidator.ValidateBattlefield(field));
		}

		[Fact]
		public async Task ValidateBattlefield_SingleShipsContact()
		{
			var field = new byte[10, 10]
			{
				{1, 0, 0, 0, 0, 1, 1, 0, 0, 0},
				{1, 0, 1, 0, 0, 0, 0, 0, 1, 0},
				{1, 0, 1, 0, 1, 1, 1, 0, 1, 0},
				{1, 0, 0, 0, 0, 1, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
			};
			Assert.False(await MyValidator.ValidateBattlefield(field));
		}

		[Fact]
		public async Task ValidateBattlefield_DoubleShipsContact()
		{
			var field = new byte[10, 10]
			{
				{1, 0, 0, 0, 0, 1, 1, 0, 0, 0},
				{1, 1, 1, 0, 0, 0, 0, 0, 1, 0},
				{1, 0, 1, 0, 1, 1, 1, 0, 1, 0},
				{1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
				{0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 1, 0, 0, 1, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
			};
			Assert.False(await MyValidator.ValidateBattlefield(field));
		}
	}
}
