using System;
using System.Collections.Generic;
using System.Text;

namespace UI.Core
{
    internal struct Cell
    {
        public char Symbol { get; set; }     
        public ConsoleColor TextColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }
        public Cell(char symbol = ' ', ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black) {
            Symbol = symbol;
            TextColor = foreground;
            BackgroundColor = background;
        }

        public override string ToString() => $"{Symbol}";       
    }
}
