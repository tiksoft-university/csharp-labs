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
            UpdateBounds(x, y, width, height);
        }

        public int X { get; set; } 
        public int Y { get; set; }
        protected bool SizeIsDynamic { get; init; } = true;

        public int Height { 
            get 
            {
                if(!SizeIsDynamic) return _height;
                return Units.Vh(_height);
            } 
            set => _height = value; 
        }
        public int Width
        {
            get
            {
                if (!SizeIsDynamic) return _width;
                return Units.Vw(_width);
            }
            set => _width = value;
        }

        public void UpdateBounds(int x, int y, int width, int height)
        {
            if (X == x && Y == y && width == _width && height == _height) return;
            X = x; Y = y;
            _width = width; _height = height;
        }

        public Bounds GetBounds() => new Bounds(X, Y, Width, Height);

        public abstract void Render(CanvasEngine canvas);
    }
}
