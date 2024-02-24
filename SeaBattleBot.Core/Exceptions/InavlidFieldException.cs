using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleBot.Core.Exceptions
{
	public class InvalidFieldException : Exception
	{
		public InvalidFieldException() { }
		public InvalidFieldException(string message) : base(message) { }
		public InvalidFieldException(string message, Exception inner) : base(message, inner) { }
	}
}
