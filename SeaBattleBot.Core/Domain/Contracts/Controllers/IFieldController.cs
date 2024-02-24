using SeaBattleBot.Core.DTOs;

namespace SeaBattleBot.Core.Domain.Contracts.Controllers
{
    public interface IFieldController
    {
        public Task<byte[,]> CreateBattleField(string playerField);
        public Task<byte[,]> CreateRandomBattleField();
        public Task<string> GetFieldAsString(byte[,] field);
        public Task<string> GetEnemyFieldAsString(byte[,] field);
        public Task<MoveResult> MakeMove(byte[,] field, byte i, byte j);
        public Task<bool> FieldContainsNotDestroyedShip(byte[,] field);
    }
}
