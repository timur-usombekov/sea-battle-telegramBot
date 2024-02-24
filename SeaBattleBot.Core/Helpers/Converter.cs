using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleBot.Core.Helpers
{
    public static class Converter
    {
        public static Task<(byte, byte)> ConvertMoveToIndex(string move)
        {
            char letter = move[0];
            byte number = byte.Parse(move.Substring(1));

            byte letterIndex = (byte)(char.ToUpper(letter) - 'A');
            byte numberIndex = (byte)(number - 1);

            return Task.FromResult((letterIndex, numberIndex));

        }
    }
}
