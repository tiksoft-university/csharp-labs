using UI.Core;

namespace store_manager.UI
{
    internal sealed class AssortmentsBar : Element
    {           
        public AssortmentsBar(
            int x, 
            int y, 
            int width, 
            int height
        ) : base(x, y, width, height) {}


        private void RenderAssortment(CanvasEngine canvas)
        {

        }

        public override void Render(CanvasEngine canvas)
        {
            canvas.DrawBox(X, Y, Width, Height, "ВИТРИНЫ (МАКСИМУМ 3)");
            
            List<int> ass = [1, 2];

            if (ass.Count == 0)
            {
                canvas.WriteText(X + _padding, Height / 2, "Витрин пока нет. Добавьте, нажав С.", ConsoleColor.DarkGray);
                return;
            }

            for (int i = 0; i < ass.Count; i++)
            {
                int asrtX = X + _padding;
                int asrtY = Y + _padding + i;
                int asrtNum = ass[i];
                string asrtText = $"ВИТРИНА {asrtNum}:";

                canvas.WriteText(asrtX, asrtY, asrtText);
                                
                for (int j = 0; j < 10; j++)
                {
                    int productX = asrtText.Length + 4 * (j + 1);
                    int productY = asrtY;
                    canvas.WriteText(productX, productY, $"--");
                }
            }
        }
    }
}
