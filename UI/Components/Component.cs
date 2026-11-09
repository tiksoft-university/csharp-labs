using System;
using System.Collections.Generic;
using System.Text;
using UI.Core;

namespace UI.Components
{
    internal abstract class Component
    {      
        protected Component(int x, int y,  int width, int height)
        {
            X = x; Y = y;
            Width = width; Height = height;
        }

        public int X { get; set; } 
        public int Y { get; set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        public abstract void Render(CanvasEngine canvas);
    }
}
