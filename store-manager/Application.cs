using store_manager.Model;
using store_manager.UI;
using UI.Core;

namespace store_manager
{
    enum AppState
    {
        READING
    }
    internal class Application
    {
        // STATE
        private readonly AssortmentsSlice _assortmentsState = new();
        private AppState _appState = AppState.READING;

        // UI
        private readonly CanvasEngine _canvas = new();
        private readonly TopPanel _topPanel;

        public Application()
        {
            _topPanel = new(0, 0, 70, 21, _assortmentsState);
        }

        public void Run()
        {
            while (true)
            {

                // Отрисовка секции складов
                _canvas.Clear();
                _topPanel.Render(_canvas);

                _canvas.DrawBox(0, Units.Vh(21) + 1, Units.Vw(70), Units.Vh(80) - 1, "ИНФОРМАЦИЯ О ТОВАРЕ");
                _canvas.WriteText(3, Units.Vh(21) + 3, _assortmentsState.GetStringifiedSelectedProduct());
                _canvas.DrawBox(Units.Vw(70) + 1, 0, Units.Vw(65), Units.Vh(80) - 1, "ИНСТРУКЦИЯ ПО ИСПОЛЬЗОВАНИЮ");
                _canvas.Render();
                _canvas.RefreshSize();

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true);

                    switch (_appState)
                    {
                        case AppState.READING:
                            switch (key.Key)
                            {
                                /*Изменение позиции просмариваемого товара в витрине*/
                                case ConsoleKey.D: _assortmentsState.SelectedProductIndex++; break;
                                case ConsoleKey.A: _assortmentsState.SelectedProductIndex--; break;

                                /*Изменение позиции просматриваемой витрины*/
                                //case ConsoleKey.W: _assortmentsState.; break;
                                //case ConsoleKey.S: _assortmentsState.SelectedProductIndex--; break;
                            }
                            break;
                        default:
                            break;
                    }                  
                }

                Thread.Sleep(80);
            }
        }
    }
}
