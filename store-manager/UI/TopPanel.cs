using store_manager.Model;
using UI.Core;

namespace store_manager.UI
{
    internal sealed class TopPanel : Element
    {
        private readonly AssortmentsSlice _state;
        
        public TopPanel(int x, int y, int w, int h, AssortmentsSlice state) : base(x, y, w, h) 
        {
            _state = state;
        }

        private void RenderProducts(CanvasEngine canvas, int offsetX, int offsetY, int assortmentId, bool isInSelected)
        {
            var productsIds = _state.GetProductsIds(assortmentId);
            if (productsIds.Count == 0) return;

            for (int i = 0; i < productsIds.Count; i++)
            {
                var productX = offsetX + 6 * (i + 1);
                var isSelected = isInSelected && _state.SelectedProductIndex == i;

                ConsoleColor productBg = isSelected ? ConsoleColor.DarkGreen : ConsoleColor.Black;

                var productId = productsIds[i];
                var productIsNull = productId is null;
                
                var isPackaged = productIsNull ? false : _state.IsPackaged(assortmentId, (int)productId!);
                var productText = "";
               
                if (productIsNull) productText = "__";
                else if (isPackaged)
                {
                    productX -= 1;
                    productText = $"[{productId.ToString()}]";
                }
                else productText = productId.ToString();

                canvas.WriteText(productX, offsetY, productText, ConsoleColor.White, productBg);
            }
        }

        private void RenderAssortments(CanvasEngine canvas, List<int> assortmentsIds)
        { 
            for (int i = 0; i < assortmentsIds.Count; i++)
            {
                var assortmentX = X + _padding;
                var assortmentY = Y + _padding + i;
                var assortmentId = assortmentsIds[i];               
                var assortmentText = $"ВИТРИНА {assortmentId}:";
                var isSelected = _state.SelectedAssortmentIndex == i;

                ConsoleColor assortmentTitleColor = isSelected ? ConsoleColor.Magenta : ConsoleColor.White; 
                canvas.WriteText(assortmentX, assortmentY, assortmentText, assortmentTitleColor);
                RenderProducts(canvas, assortmentText.Length, assortmentY, assortmentId, isSelected);              
            }
        }

        public override void Render(CanvasEngine canvas)
        {
            var assortmentsIds = _state.GetAssortmentIds();
            canvas.DrawBox(X, Y, Width, Height, $"СПИСОК ВИТРИН");            

            if (assortmentsIds.Count == 0)
            {
                canvas.WriteText(X + _padding, Height / 2, "Витрин пока нет. Добавьте, нажав С.", ConsoleColor.DarkGray);
                return;
            }

            RenderAssortments(canvas, assortmentsIds);
        }
    }
}
