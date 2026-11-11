using System;
using System.Collections.Generic;
using System.Text;

namespace UI.Core
{
    public static class Units
    {
        public static int Vw(int percents) => (int)((Console.WindowWidth) * percents / 100f);
        public static int Vh(int percents) => (int)((Console.WindowHeight - 1) * percents / 100f);
    }
}
