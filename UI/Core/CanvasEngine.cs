using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace UI.Core
{
    
    public sealed class CanvasEngine
    {
        private readonly static int WINDOW_HEIGHT_OFFSET = 1;

        /// <summary>Буферная матрица ячеек</summary>
        private Cell[,] _cellsMap;

        /// <summary>Высота холста</summary>
        public int Height { get; private set; }
        
        /// <summary>Ширина холста</summary>
        public int Width { get; private set; }

        public CanvasEngine()
        {
            Console.CursorVisible = false;

            Height = Console.WindowHeight - WINDOW_HEIGHT_OFFSET;
            Width = Console.WindowWidth;
            
            _cellsMap = new Cell[Width, Height];
            Clear();
        }

        /// <summary>Очистка буфера. Заполняет матрицу пробелами</summary>
        public void Clear()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {                    
                    SetCell(
                        x, 
                        y, 
                        ' ', 
                        ConsoleColor.White, 
                        ConsoleColor.Black
                    );
                }                
            }
        }

        /// <summary>Рисует текст на заданных координатах</summary>
        
        public void WriteText(int x, int y, string text, ConsoleColor textColor = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
        {
            var renderingY = y;
            var renderingX = x;

            for (int i = 0; i < text.Length; i++)
            {                
                if (text[i] == '\n')
                {
                    if(renderingY < Height - 1) renderingY++;
                    renderingX = x;
                    continue;
                }

                SetCell(renderingX++, renderingY, text[i], textColor, background);
            }
        }

        /// <summary>Рисует линию. Ориентация линии - isHorizontal</summary>
        public void WriteLine(int x, int y, int width, bool isHorizontal = true)
        {
            if (width > Width) width = Width;

            StringBuilder line = new StringBuilder();

            if(isHorizontal)
            {
                for (int i = 0; i < width; i++) line.Append('─');               

                WriteText(x, y, line.ToString());
                return;
            }

            for (int i = 0; i < width; i++)
            {
                SetCell(x, y + i, '│');
            }                        
        }

        /// <summary>Рисует прямоугольник. Может иметь название</summary>
        public void DrawBox(int x, int y, int width, int height, string title = "") => WriteBox(x, y, width, height, title);        
        public void DrawBox(int x, int y, string width, int height, string title = "")
        {
            if (!width.EndsWith("v")) return;

            int realWidth = (int)(Width * Int32.Parse(width.Replace("v", "")) / 100f);
            WriteBox(x, y, realWidth, height, title);
        }

        public void DrawBox(int x, int y, int width, string height, string title = "")
        {
            if (!height.EndsWith("v")) return;

            int realHeight = (int)(Height * Int32.Parse(height.Replace("v", "")) / 100f);
            WriteBox(x, y, width, realHeight, title);
        }

        public void DrawBox(int x, int y, string width, string height, string title = "")
        {
            if (!height.EndsWith("v") || !width.EndsWith("v")) return;

            int realWidth = (int)(Width * Int32.Parse(width.Replace("v", "")) / 100f);
            int realHeight = (int)(Height * Int32.Parse(height.Replace("v", "")) / 100f);
            WriteBox(x, y, realWidth, realHeight, title);
        }


        /// <summary>Рисует прямоугольник. Может иметь название</summary>
        private void WriteBox(int x, int y, int width, int height, string title = "") {
            
            // Верхняя горизонтальная линия
            WriteLine(x + 1, y, width - 1);
            // Нижняя горизонтальная линия
            WriteLine(x + 1, y + height, width - 1);
            // Левая вертикальная линия
            WriteLine(x, y + 1, height - 1, false);
            // Правая вертикальная линия
            WriteLine(x + width, y + 1, height - 1, false);
            // Верхние углы
            SetCell(x, y, '┌'); SetCell(x + width, y, '┐');
            // Нижние углы
            SetCell(x, y + height, '└'); SetCell(x + width, y + height, '┘');

            if (title != "")
            {
                WriteText(x + 3, y, title);
            }                        
        }
               
        public void SetCell(int x, int y, char value, ConsoleColor textColor = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black) 
        {
            if (x < 0) x = 0; if (x >= Width) x = Width - 1;
            if (y < 0) y = 0;  if (y >= Height) y = Height - 1;

            _cellsMap[x, y].Symbol = value;
            _cellsMap[x, y].TextColor = textColor;
            _cellsMap[x, y].BackgroundColor = background;
        }         

        public void RefreshSize()
        {
            int newWidth = Console.WindowWidth;
            int newHeight = Console.WindowHeight - 1;

            bool sizeNotChanged = newWidth == Width && newHeight == Height;
            if (sizeNotChanged) return;

            Width = newWidth; Height = newHeight;         
            _cellsMap = new Cell[newWidth, newHeight];
            Clear();           
        }

        public void Render()
        {           
            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {   
                    bool needResetColor = false;

                    if (_cellsMap[x, y].BackgroundColor != ConsoleColor.Black)
                    {
                        Console.BackgroundColor = _cellsMap[x, y].BackgroundColor;
                        needResetColor = true;
                    }

                    if (_cellsMap[x, y].TextColor != ConsoleColor.White)
                    {
                        Console.ForegroundColor = _cellsMap[x, y].TextColor;
                        needResetColor = true;
                    }

                    Console.Write(_cellsMap[x, y]);

                    if(needResetColor )
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }                                       
                }

                Console.WriteLine();
            }            
        }
    }
}
