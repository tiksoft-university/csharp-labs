using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UI.Core;
using static System.Net.Mime.MediaTypeNames;

namespace UI.Components
{
    internal class TextContent : Component
    {
        public string Text { get; set; }
        public TextContent(int x, int y, string text) : base(x, y, text.Length, text.Count(s => s == '\n')+1)
        {
            Text = text;
        }               
        public override void Render(CanvasEngine canvas)
        {
            var renderingY = Y;
            for (int i = 0; i < Text.Length; i++)
            {
                if (Text[i] == '\n' && i != Text.Length) {
                    renderingY++;
                    X = X;
                    continue;
                }
                //canvas.SetPixel(X + i, renderingY, Text[i]);
            }
        }
    }
}
