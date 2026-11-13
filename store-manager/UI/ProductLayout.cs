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
        private readonly ProductForm _formScreen;

        public ProductLayout(int x, int y, int width, int height, AssortmentsSlice assortments, AppModeSlice appMode) : base(x, y, width, height)
        {
            int layoutX = X + _padding;
            int layoutY = Y + _padding;

            _assortments = assortments;
            _appMode = appMode;
            _infoScreen = new(layoutX, layoutY ,0, 0, _assortments);
            _formScreen = new(layoutX, layoutY, 0, 0, _assortments, _appMode);
        }

        private string GetCurrentTitle() => _appMode.State switch
        {
            AppMode.REPLACING => "СОЗДАНИЕ/ЗАМЕНА ТОВАРА",
            _ => "ИНФОРМАЦИЯ О ТОВАРЕ"
        };

        public void HandleKeyboard(ConsoleKeyInfo key)
        {          
            if (_appMode.State != AppMode.REPLACING) return;
            _formScreen.HandleKeyboard(key);            
        }

        private void RenderCurrent(CanvasEngine canvas, Element element)
        {
            element.UpdateBounds(X + _padding, Y + _padding, Width, Height);
            element.Render(canvas);
        }
            
        public override void Render(CanvasEngine canvas)
        {
            canvas.DrawBox(X, Y, Width, Height, GetCurrentTitle());

            Element current = _appMode.State == AppMode.REPLACING ? _formScreen : _infoScreen;
            RenderCurrent(canvas, current);
        }
    }
}
