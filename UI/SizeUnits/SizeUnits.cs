using System;
using System.Collections.Generic;
using System.Text;

namespace UI.SizeUnits
{
    public static class SizeUnits
    {
        public static int Vw(int percents) => (int)((Console.WindowWidth) * percents / 100f);
        public static int Vh(int percents) => (int)((Console.WindowHeight - 1) * percents / 100f);
    }
}
