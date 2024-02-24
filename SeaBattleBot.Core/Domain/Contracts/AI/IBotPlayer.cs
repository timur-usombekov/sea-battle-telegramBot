namespace SeaBattleBot.Core.Domain.Contracts.AI
{
    public interface IBotPlayer
    {
        public Task<(byte, byte)?> MakeMove(long chatId ,byte[,] field);
    }
}
