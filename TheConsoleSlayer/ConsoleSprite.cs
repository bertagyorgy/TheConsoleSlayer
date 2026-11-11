using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheConsoleSlayer
{
    internal class ConsoleSprite
    {
        public ConsoleColor Background {  get; set; }
        public ConsoleColor Foreground { get; set; }
        public char Glyph { get; set; }
        public ConsoleSprite(ConsoleColor background, ConsoleColor foreground, char glyph)
        {
            Background = background;
            Foreground = foreground;
            Glyph = glyph;
        }
    }
}
