namespace SeaBattleBot.Core.DTOs
{
	public class MoveResult
	{
		public bool IsSuccess { get; set; }
		public bool IsHitted { get; set; }
		public string? ErrorMessage { get; set; }
	}
}
