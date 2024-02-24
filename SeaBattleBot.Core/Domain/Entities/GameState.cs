using SeaBattleBot.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaBattleBot.Core.Domain.Entities
{
	public class GameState
	{
		[Key]
		public long Id { get; set; }
		[ForeignKey(nameof(EnemyState))]
		public long EnemyStateId { get; set; }
		public string? PlayerFieldAsJson { get; set; }
		public string? EnemyFieldAsJson { get; set; }
		public GameStatus GameStatus { get; set; }

		public EnemyState EnemyState { get; set; }
	}
}
