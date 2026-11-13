using store_manager.Model;
using UI.Core;

namespace store_manager.UI
{
    internal class ProductInfo : Element
    {
        private readonly AssortmentsSlice _assortments;

        public ProductInfo(int x, int y, int width, int height, AssortmentsSlice assortments) : base(x, y, width, height)
        {
            _assortments = assortments;
        }

        public override void Render(CanvasEngine canvas)
        {
            canvas.WriteText(X, Y, _assortments.GetStringifiedSelectedProduct());
        }
    }
}
