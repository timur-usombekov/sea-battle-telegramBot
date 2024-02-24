using SeaBattleBot.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaBattleBot.Core.Domain.Entities
{
	public class EnemyState
	{
		[Key]
		public long Id { get; set; }
		public byte? LastHittedMoveRow {  get; set; }
		public byte? LastHittedMoveColumn {  get; set; }
		public byte? FirstHittedMoveRow { get; set; }
		public byte? FirstHittedMoveColumn { get; set; }
		public ShipDirection? LastMoveShipDirection { get; set; }
		public bool LastMoveHitted {  get; set; }
	}
}
