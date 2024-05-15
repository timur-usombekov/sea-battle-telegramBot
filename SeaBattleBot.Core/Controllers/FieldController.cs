using System.Text;
using System.Text.RegularExpressions;
using SeaBattleBot.Core.Enums;
using SeaBattleBot.Core.Exceptions;
using SeaBattleBot.Core.DTOs;
using SeaBattleBot.Core.Domain.Contracts.Controllers;

namespace SeaBattleBot.Core.Controllers
{
    public class FieldController: IFieldController
    {
        public const byte FieldSize = 10;
        private readonly IShipController _shipController;
        private Random _random;
        public FieldController(IShipController shipController)
        {
            _shipController = shipController;
            _random = new Random();
        }
        public async Task<byte[,]> CreateBattleField(string playerField)
        {
            StringBuilder builder = new StringBuilder(playerField);
            builder.Replace("🚢", "1");
            builder.Replace("🌊", "0");
            playerField = builder.ToString();

			var lines = playerField.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.TrimEntries);

			if (!await CheckFieldLines(lines))
            {
                throw new InvalidFieldException("Field lines is not valid");
            }
            byte[,] field = new byte[FieldSize, FieldSize];
            for (int i = 0; i < FieldSize; i++)
            {
                for (int j = 0; j < FieldSize; j++)
                {
                    field[i, j] = lines[i][j] == '1' ? (byte)1 : (byte)0;
                }
            }

            return field;
        }
		public async Task<byte[,]> CreateRandomBattleField()
		{
            List<byte> ships = new List<byte>() { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
			byte[,] field = new byte[FieldSize, FieldSize];

			foreach (byte ship in ships) 
            {
                await PlaceShip(field, ship);
			}
            await RevertFieldToStartPosition(field);
            return field;
		}
		public async Task<string> GetFieldAsString(byte[,] field)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < field.GetLength(0); i++)
            {
                switch (i)
                {
                    case 0:
                        result.Append("🚫1️⃣2️⃣3️⃣4️⃣5️⃣6️⃣7️⃣8️⃣9️⃣🔟\n");
                        result.Append(" Ⓐ ");
                        break;
                    case 1:
                        result.Append(" Ⓑ ");
                        break;
                    case 2:
                        result.Append(" Ⓒ ");
                        break;
                    case 3:
                        result.Append(" Ⓓ ");
                        break;
                    case 4:
                        result.Append(" Ⓔ ");
                        break;
                    case 5:
                        result.Append(" Ⓕ ");
                        break;
                    case 6:
                        result.Append(" Ⓖ ");
                        break;
                    case 7:
                        result.Append(" Ⓗ ");
                        break;
                    case 8:
                        result.Append(" Ⓘ ");
                        break;
                    case 9:
                        result.Append(" Ⓙ ");
                        break;
                    default:
                        break;
                }
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    switch (field[i, j])
                    {
                        case 0: // Represent empty cell.
                            result.Append("🌊");
                            break;
                        case 1: // Represent cell with ship.
                            result.Append("🚢");
                            break;
                        case 2: // Represent unattackable cell.
                            result.Append("✖️");
                            break;
                        case 3: // Represent cell with ship that was hit.
                            result.Append("💥");
                            break;
                        default:
                            break;
                    }
                }
                result.AppendLine();
            }
            return result.ToString();
        }
		public async Task<string> GetEnemyFieldAsString(byte[,] field)
		{
			StringBuilder result = new StringBuilder();

			for (int i = 0; i < field.GetLength(0); i++)
			{
				switch (i)
				{
					case 0:
						result.Append("🚫1️⃣2️⃣3️⃣4️⃣5️⃣6️⃣7️⃣8️⃣9️⃣🔟\n");
						result.Append(" Ⓐ ");
						break;
					case 1:
						result.Append(" Ⓑ ");
						break;
					case 2:
						result.Append(" Ⓒ ");
						break;
					case 3:
						result.Append(" Ⓓ ");
						break;
					case 4:
						result.Append(" Ⓔ ");
						break;
					case 5:
						result.Append(" Ⓕ ");
						break;
					case 6:
						result.Append(" Ⓖ ");
						break;
					case 7:
						result.Append(" Ⓗ ");
						break;
					case 8:
						result.Append(" Ⓘ ");
						break;
					case 9:
						result.Append(" Ⓙ ");
						break;
					default:
						break;
				}
				for (int j = 0; j < field.GetLength(1); j++)
				{
					switch (field[i, j])
					{
						case 0: // Represent empty cell.
						case 1: // Represent cell with ship.
							result.Append("🌊");
							break;
						case 2: // Represent unattackable cell.
							result.Append("✖️");
							break;
						case 3: // Represent cell with ship that was hit.
							result.Append("💥");
							break;
						default:
							break;
					}
				}
				result.AppendLine();
			}
			return result.ToString();
		}
		public async Task<MoveResult> MakeMove(byte[,]field, byte i, byte j)
        {
            switch (field[i, j])
            {
                case 0: // Represent empty cell.
					field[i, j] = 2;
					return new MoveResult() { IsSuccess = true };
				case 1: // Represent cell with ship.
					field[i, j] = 3;
                    if (await _shipController.IsShipDestroyed(field, i, j))
					{
                        await _shipController.SetCrossAroundShip(field, i, j);
					}
					return new MoveResult() { IsSuccess = true, IsHitted = true };
                case 2: // Represent unattackable cell.
				case 3: // Represent cell with ship that was hit.
					return new MoveResult() { IsSuccess = false, ErrorMessage = "This cell has already been attacked, choose another move" };
				default:
                    throw new ArgumentException("Unknown cell inditificator");
            }
        }
		public async Task<bool> FieldContainsNotDestroyedShip(byte[,] field)
		{
			foreach (var item in field)
			{
				if(item == 1)
					return true;
			}
			return false;
		}

		private async Task<bool> CheckFieldLines(string[] lines)
        {
            if(lines.Length != FieldSize) 
                return false;
            for (int i = 0; i < FieldSize; i++)
            {
                lines[i] = Regex.Replace(lines[i], @"\s", "");
				if (!Regex.IsMatch(lines[i], @"[01]{" + FieldSize + @"}") || lines[i].Length != FieldSize)
				{
					return false;
				}
			}
			return true;
        }
		private async Task PlaceShip(byte[,] field, byte shipSize)
		{
            byte i, j; 
            ShipDirection direction;
            while (true)
            {
		        i = (byte)_random.Next(0, FieldSize);
		        j = (byte)_random.Next(0, FieldSize);
		        direction = Enum.GetValues<ShipDirection>()[_random.Next(0, Enum.GetValues<ShipDirection>().Length)];
		        if (await CanPlaceShip(field, i, j, direction, shipSize))
		        {
			        for (int di = 0; di < shipSize; di++)
			        {
				        if (direction == ShipDirection.Vertical)
                        {
					        field[i + di, j] = 1;
                        }
                        else
                        {
					        field[i, j + di] = 1;
                        }
			        }
                    await _shipController.SetCrossAroundShip(field, i, j);
                    break;
		        }
			}

		}
		private async Task<bool> CanPlaceShip(byte[,] field, byte i, byte j, ShipDirection direction, int shipSize)
		{
			for (int di = 0; di < shipSize; di++)
			{
				if (direction == ShipDirection.Vertical)
				{
					if (i + di >= FieldSize || field[i + di, j] != 0)
						return false;
				}
				else
				{
					if (j + di >= FieldSize || field[i, j + di] != 0)
						return false;
				}
			}
			return true;
		}
		private async Task RevertFieldToStartPosition(byte[,] field)
		{
			for (int i = 0; i < field.GetLength(0); i++)
			{
				for (int j = 0; j < field.GetLength(1); j++)
				{
					field[i, j] = field[i, j] == 2 ? (byte)0 : field[i, j];
				}
			}
		}
	}
}
