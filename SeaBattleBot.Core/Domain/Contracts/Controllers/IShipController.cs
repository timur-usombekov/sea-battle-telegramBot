using SeaBattleBot.Core.Enums;

namespace SeaBattleBot.Core.Domain.Contracts.Controllers
{
    public interface IShipController
    {
        public Task<byte> GetShipSize(byte[,] field, int i, int j);
        public Task<ShipDirection> GetShipDirection(byte[,] field, int i, int j);
        public Task<bool> IsShipDestroyed(byte[,] field, int i, int j);
        public Task SetCrossAroundShip(byte[,] field, int i, int j);
    }
}
