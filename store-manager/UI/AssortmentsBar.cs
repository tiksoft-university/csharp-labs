using store_manager.Model;
using UI.Core;

namespace store_manager.UI
{
    internal sealed class AssortmentsBar : Element
    {
        private readonly AssortmentsSlice _state;
        
        public AssortmentsBar(int x, int y, int w, int h, AssortmentsSlice state) : base(x, y, w, h) 
        {
            _state = state;
        }

        private void RenderProducts(CanvasEngine canvas, int offsetX, int offsetY, int assortmentId)
        {
            var productsIds = _state.GetProductsIds(assortmentId);
            if (productsIds.Count == 0) return;
                      
            for (int i = 0; i < productsIds.Count; i++)
            {
                int productX = offsetX + 4 * (i + 1);
                bool isSelected = _state.GetSelectedAssortmentId() == assortmentId && _state.GetSelectedProductId() == productsIds[i];
                ConsoleColor productBg = isSelected ? ConsoleColor.DarkGreen : ConsoleColor.Black;
                canvas.WriteText(productX, offsetY, $"{productsIds[i]}", ConsoleColor.White, productBg);
            }
        }

        private void RenderAssortments(CanvasEngine canvas, List<int> assortmentsIds)
        { 
            for (int i = 0; i < assortmentsIds.Count; i++)
            {
                int assortmentX = X + _padding;
                int assortmentY = Y + _padding + i;
                int assortmentId = assortmentsIds[i];
                
                string assortmentText = $"ВИТРИНА {assortmentId}:";
                bool isSelected = _state.GetSelectedAssortmentId() == assortmentId;
                ConsoleColor assortmentTitleColor = isSelected ? ConsoleColor.Magenta : ConsoleColor.White; 

                canvas.WriteText(assortmentX, assortmentY, assortmentText, assortmentTitleColor);
                RenderProducts(canvas, assortmentText.Length, assortmentY, assortmentId);              
            }
        }

        public override void Render(CanvasEngine canvas)
        {
            var assortmentsIds = _state.GetAssortmentIds();
            var assortmentsMaxCount = _state.GetAssortmentsMaxCount();
            canvas.DrawBox(X, Y, Width, Height, $"ВИТРИНЫ (МАКСИМУМ {assortmentsMaxCount})");            

            if (assortmentsIds.Count == 0)
            {
                canvas.WriteText(X + _padding, Height / 2, "Витрин пока нет. Добавьте, нажав С.", ConsoleColor.DarkGray);
                return;
            }

            RenderAssortments(canvas, assortmentsIds);
        }
    }
}
