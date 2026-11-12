using UI.Core;

namespace store_manager.UI
{
    internal sealed class HelpPanel : Element
    {
        private readonly string[] _hotkeys;

        public HelpPanel(int x, int y, int w, int h) : base(x, y, w, h) 
        {
            _hotkeys = [
                "+ W/S — Навигация по витринам",
                "+ A/D — Навигация по товарам",
                "+ I — Сортировка по ID",
                "+ O — Сортировка по имени",
                "+ B — Упаковать/Распаковать",                              
                "+ R — Удалить товар"                              
            ];
        }

        public override void Render(CanvasEngine canvas)
        {
            canvas.DrawBox(X, Y, Width, Height, $"УПРАВЛЕНИЕ");

            for (int i = 0; i < _hotkeys.Length; i++)
            {
                var hotKeyX = X + _padding;
                var hotKeyY = Y + _padding + i;
                canvas.WriteText(hotKeyX, hotKeyY, _hotkeys[i]);
            }
        }
    }
}
