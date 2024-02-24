using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaBattleBot.Core.Domain.Entities
{
	public class User
	{
		[Key]
		public long Id { get; set; }
		public long ChatId { get; set; }
		[ForeignKey(nameof(GameState))]
		public long GameStateId { get; set; }

		public GameState GameState { get; set; }
	}
}
