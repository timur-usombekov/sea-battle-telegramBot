using SeaBattleBot.Core.Controllers;
using SeaBattleBot.Core.Enums;

namespace SeaBattleBot.Core.Helpers
{
    public static class MyValidator
    {
        private const byte 
			UnvisitedShip = 1,
			VisitedShip = 2;
        private static List<string> _allPossibleMoves = new()
        {
            "A1","A2","A3","A4","A5","A6","A7","A8","A9","A10",
            "B1","B2","B3","B4","B5","B6","B7","B8","B9","B10",
            "C1","C2","C3","C4","C5","C6","C7","C8","C9","C10",
            "D1","D2","D3","D4","D5","D6","D7","D8","D9","D10",
            "E1","E2","E3","E4","E5","E6","E7","E8","E9","E10",
            "F1","F2","F3","F4","F5","F6","F7","F8","F9","F10",
            "G1","G2","G3","G4","G5","G6","G7","G8","G9","G10",
            "H1","H2","H3","H4","H5","H6","H7","H8","H9","H10",
            "I1","I2","I3","I4","I5","I6","I7","I8","I9","I10",
            "J1","J2","J3","J4","J5","J6","J7","J8","J9","J10",
        };
        public async static Task<bool> ValidateMove(string playerMove)
        {
            var move = playerMove.ToUpper();
            return _allPossibleMoves.Contains(move);
        }
		public async static Task<bool> ValidateBattlefield(byte[,] field)
		{
			List<byte> ships = new() { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
			if (FieldController.FieldSize != field.GetLength(0) || FieldController.FieldSize != field.GetLength(1))
			{
				return false;
			}
			for (int i = 0; i < FieldController.FieldSize; i++)
			{
				for (int j = 0; j < FieldController.FieldSize; j++)
				{
					if (field[i, j] == 1)
					{
						byte shipSize = await CheckShip(field, i, j);
						if (ships.Contains(shipSize))
						{
							ships.Remove(shipSize);
						}
						else
						{
							await RevertFieldToStartPosition(field);
							return false;
						}
					}
				}
			}
			await RevertFieldToStartPosition(field);
			return ships.Count == 0;
		}

		private static async Task<byte> CheckShip(byte[,] field, int i, int j)
		{
			byte shipSize = 1; // count current cell
			ShipController shipController = new ShipController();
			var dir = await shipController.GetShipDirection(field, i, j);
			field[i, j] = VisitedShip;
			if(await CheckCorners(field, i, j))
			{
				return 5; // return number bigger than the biggest ship
			}

			byte dRow = 0, dCol = 1;
			if (dir == ShipDirection.Vertical)
			{
				dRow = 1;
				dCol = 0;
			}

			int nextRow = i + dRow;
			int nextCol = j + dCol;

			while (nextRow >= 0 && nextRow < field.GetLength(0) && 
				nextCol >= 0 && nextCol < field.GetLength(1) && 
				field[nextRow, nextCol] == 1)
			{
				shipSize++;
				field[nextRow, nextCol] = VisitedShip;
				if (await CheckCorners(field, i, j))
				{
					return 5;
				}
				nextRow += dRow;
				nextCol += dCol;
			}

			return shipSize;
		}

		private static async Task<bool> CheckCorners(byte[,] field, int i, int j)
		{
			List<(int, int)> corners = new() { (-1, -1), (-1, 1), (1, -1), (1, 1) }; // up-left, up-right, down-left, down-right

			foreach (var (ci, cj) in corners)
			{
				int ni = i + ci, nj = j + cj;
				if (ni >= 0 && ni < field.GetLength(0) && nj >= 0 && nj < field.GetLength(1) && field[ni, nj] == 1)
				{
					return true; 
				}
			}
			return false;
		}

		private static async Task RevertFieldToStartPosition(byte[,] field)
		{
			for (int i = 0; i < FieldController.FieldSize; i++)
			{
				for (int j = 0; j < FieldController.FieldSize; j++)
				{
					field[i, j] = field[i, j] == VisitedShip ? UnvisitedShip : (byte)0;
				}
			}
		}
	}
}
