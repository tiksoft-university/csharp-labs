using System;
using System.Collections.Generic;
using System.Text;

namespace UI.Core
{
    class ScreenBuffer
    {
        private char[,] _pixels;
        
        public int Height { get; private set; }
        public int Width { get; private set; }

        public ScreenBuffer(int height, int width)
        {
            Console.CursorVisible = false;

            Height = height;
            Width = width;
            _pixels = new char[Width, Height];
            Clear();
        }
        
        public void Clear()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    SetPixel(x, y, '0');
                }                
            }
        }

        public void SetPixel(int x, int y, char value) => _pixels[x, y] = value;
        

        public void Resize(int width, int height)
        {
            if (width == Width && height == Height) return;

            Height = height;
            Width = width;
            
            _pixels = new char[Width, Height];
            Clear();
        }

        public void Render()
        {
            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Console.Write(_pixels[x, y]);
                }
                Console.WriteLine();
            }            
        }
    }
}
