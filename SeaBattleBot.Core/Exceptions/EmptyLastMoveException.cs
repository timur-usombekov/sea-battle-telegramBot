using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleBot.Core.Exceptions
{
	public class EmptyLastMoveException : Exception
	{
		public EmptyLastMoveException() { }
		public EmptyLastMoveException(string message) : base(message) { }
		public EmptyLastMoveException(string message, Exception inner) : base(message, inner) { }
	}
}
