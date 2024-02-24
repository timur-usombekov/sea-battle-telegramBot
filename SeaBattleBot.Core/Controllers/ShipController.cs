using SeaBattleBot.Core.Domain.Contracts.Controllers;
using SeaBattleBot.Core.Enums;

namespace SeaBattleBot.Core.Controllers
{
    public class ShipController: IShipController
	{
		public async Task<ShipDirection> GetShipDirection(byte[,] field, int i, int j)
		{
			if (!await IsShip(field[i, j]))
			{
				throw new ArgumentException("Current cell is not a ship");
			}
			var up = i - 1;
			var down = i + 1;
			if ((up >= 0 && up < field.GetLength(0) && await IsShip(field[up, j])) ||
				(down >= 0 && down < field.GetLength(0) && await IsShip(field[down, j])))
			{
				return ShipDirection.Vertical;
			}
			return ShipDirection.Horizontal;
		}
		public async Task<byte> GetShipSize(byte[,] field, int i, int j)
		{
			byte shipSize = 0; 
			if (!await IsShip(field[i, j]))
			{
				throw new ArgumentException("Current cell is not a ship");
			}
			(i, j) = await FindShipEdge(field, i, j);
			var dir = await GetShipDirection(field, i, j);
			if(await IsShip(field[i,j]))  // count current cell
			{
				shipSize++;
			}

			int[] dRows = { 0, 0 }, dCols = { -1, 1 }; // default for horizontal
			if (dir == ShipDirection.Vertical)
			{
				dRows = new int[] { -1, 1};
				dCols = new int[] { 0, 0};
			}

			for (int k = 0; k < 2; k++) // check both directions
			{
				int nextRow = i + dRows[k];
				int nextCol = j + dCols[k];

				while (nextRow >= 0 && nextRow < field.GetLength(0) && 
					nextCol >= 0 && nextCol < field.GetLength(1) && 
					await IsShip(field[nextRow, nextCol]))
				{
					shipSize++;
					nextRow += dRows[k];
					nextCol += dCols[k];
				}
			}

			return shipSize;
		}
		public async Task<bool> IsShipDestroyed(byte[,] field, int i, int j)
		{
			var size = await GetShipSize(field, i, j);
			(i, j) = await FindShipEdge(field, i, j);
			var dir = await GetShipDirection(field, i, j);
			var countOfDestroyedCells = 0;
			if(await IsDestroyedShipCell(field[i, j])) // Count current cell
			{
				countOfDestroyedCells++;
			}

			int[] dRows = { 0, 0 }, dCols = { -1, 1 }; // default for horizontal
			if (dir == ShipDirection.Vertical)
			{
				dRows = new int[] { -1, 1 };
				dCols = new int[] { 0, 0 };
			}
			for (int k = 0; k < 2; k++) // check both directions
			{
				int nextRow = i + dRows[k];
				int nextCol = j + dCols[k];

				while (nextRow >= 0 && nextRow < field.GetLength(0) &&
					nextCol >= 0 && nextCol < field.GetLength(1) &&
					await IsShip(field[nextRow, nextCol]) && await IsDestroyedShipCell(field[nextRow, nextCol]))
				{
					nextRow += dRows[k];
					nextCol += dCols[k];
					countOfDestroyedCells++;
				}
			}

			return countOfDestroyedCells == size;
		}
		public async Task SetCrossAroundShip(byte[,] field, int i, int j)
		{
			(i, j) = await FindShipEdge(field, i, j);
			var dir = await GetShipDirection(field, i, j);

			int[] dRows = { 0, 0 }, dCols = { -1, 1 }; // default for horizontal
			if (dir == ShipDirection.Vertical)
			{
				dRows = new int[] { -1, 1 };
				dCols = new int[] { 0, 0 };
			}

			await SetCrossAroundCell(field, i, j); // include start cell 
			for (int k = 0; k < 2; k++) // check both directions
			{
				int nextRow = i + dRows[k];
				int nextCol = j + dCols[k];

				while (nextRow >= 0 && nextRow < field.GetLength(0) &&
					nextCol >= 0 && nextCol < field.GetLength(1) &&
					await IsShip(field[nextRow, nextCol]))
				{
					await SetCrossAroundCell(field, nextRow, nextCol);
					nextRow += dRows[k];
					nextCol += dCols[k];
				}
			}
		}

		private async Task SetCrossAroundCell(byte[,] field, int i, int j)
		{
			List<(int, int)> allDirection = new()
			{
				(-1, 0), (1, 0), (0, -1), (0, 1), // up, down, left, right
				(-1, -1), (-1, 1), (1, -1), (1, 1)  // up-left, up-right, down-left, down-right
			};

			foreach (var (ci, cj) in allDirection)
			{
				int ni = i + ci, nj = j + cj;
				if (ni >= 0 && ni < field.GetLength(0) && nj >= 0 && nj < field.GetLength(1) &&
					!await IsShip(field[ni, nj]))
				{
					field[ni, nj] = 2;
				}
			}
		}
		private async Task<(byte, byte)> FindShipEdge(byte[,] field, int i, int j)
		{
			var dir = await GetShipDirection(field, i, j);
			int dRow = 0, dCol = -1; // default for horizontal
			if (dir == ShipDirection.Vertical)
			{
				dRow = -1;
				dCol = 0;
			}

			int nextRow = i + dRow;
			int nextCol = j + dCol;

			if (nextRow >= 0 && nextRow < field.GetLength(0) && nextCol >= 0 && nextCol < field.GetLength(1) && !await IsShip(field[nextRow, nextCol]))
			{
				return ((byte)i, (byte)j);
			}

			while (nextRow >= 0 && nextRow < field.GetLength(0) && nextCol >= 0 && nextCol < field.GetLength(1) && await IsShip(field[nextRow, nextCol]))
			{
				nextRow += dRow;
				nextCol += dCol;
			}

			return ((byte)(nextRow - dRow), (byte)(nextCol - dCol));
		}
		private async Task<bool> IsShip(byte cell)
		{
			switch (cell)
			{
				case 1: 
				case 3:
					return true;
				default:
					return false;
			}
		}
		private async Task<bool> IsDestroyedShipCell(byte cell)
		{
			switch (cell)
			{
				case 3:
					return true;
				default:
					return false;
			}
		}
	}
}
