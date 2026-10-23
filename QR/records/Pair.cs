using System;
using System.Collections.Generic;
using System.Text;

namespace QR
{
    internal record Pair(int X, int Y)
    {
        public static implicit operator Pair((int x, int y) pos) => new(pos.x, pos.y);
    }

}
