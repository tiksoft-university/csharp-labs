using store_manager.Model;
using System;
using System.Collections.Generic;
using System.Text;
using UI.Core;

namespace store_manager.UI
{
    internal class ProductLayout : Element
    {
        private readonly AssortmentsSlice _assortments;
        private readonly AppModeSlice _appMode;
        private readonly ProductInfo _infoScreen;
        //private Element _current; 

        public ProductLayout(int x, int y, int width, int height, AssortmentsSlice assortments, AppModeSlice appMode) : base(x, y, width, height)
        {
            _assortments = assortments;
            _appMode = appMode;
            _infoScreen = new(
                X + _padding,
                Y + _padding,
                0, 0, _assortments
            );
        }

        private string GetCurrentTitle() => _appMode.State switch
        {
            AppMode.REPLACING => "СОЗДАНИЕ/ЗАМЕНА ТОВАРА",
            _ => "ИНФОРМАЦИЯ О ТОВАРЕ"
        };

        public void HandleKeyboard(ConsoleKeyInfo key)
        {
           

            if (_appMode.State != AppMode.REPLACING) return;
            
            // ЗАСУНУТЬ В PRODUCTFORM
            if (key.Key == ConsoleKey.Escape)
            {
                _appMode.State = AppMode.READING;
                return;
            }
            Console.WriteLine(key.Key);
        }

        
        private void ConditionalRender(CanvasEngine canvas)
        {
            if (_appMode.State == AppMode.REPLACING) return;

            // Рендер ProductInfo
            _infoScreen.UpdateBounds(X + _padding, Y + _padding, Width, Height);
            _infoScreen.Render(canvas);
        }
            
        public override void Render(CanvasEngine canvas)
        {
            canvas.DrawBox(X, Y, Width, Height, GetCurrentTitle());
            ConditionalRender(canvas);                       
        }
    }
}
