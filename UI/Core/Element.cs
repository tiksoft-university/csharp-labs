using System;
using System.Collections.Generic;
using System.Text;

namespace UI.Core
{
    public readonly struct Bounds
    {
        public int Left { get; }
        public int Right { get; }
        public int Top { get; }
        public int Bottom { get; } 

        public Bounds(int x, int y, int width, int height)
        {
            Left = x;
            Right = x + width;
            Top = y;
            Bottom = y + height;            
        }
    }

    public abstract class Element
    {
        protected readonly int _padding = 2;
        private int _height;
        private int _width;
        
        protected Element(int x, int y,  int width, int height)
        {
            X = x; 
            Y = y;
            _width = width; 
            _height = height;
        }

        public int X { get; set; } 
        public int Y { get; set; }
        public int Height { 
            get {
                if(!SizeIsDynamic) return _height;
                return Units.Vh(_height);
            } 
            init; 
        }
        public int Width
        {
            get
            {
                if (!SizeIsDynamic) return _width;
                return Units.Vw(_width);
            }
            init;
        }
        protected bool SizeIsDynamic { get; init; } = true;
        public Bounds GetBounds() => new Bounds(X, Y, Width, Height);

        //public void ChangeSize(int height, int width)
        //{
        //    if (Width == width && Height == height) return;
        //    Height = height;
        //    Width = width;
        //}

        public abstract void Render(CanvasEngine canvas);
    }
}
