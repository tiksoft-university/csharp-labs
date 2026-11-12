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
        private readonly HelpPanel _helpPanel;

        public Application()
        {
            _topPanel = new(0, 0, 70, 21, _assortmentsState);
            _helpPanel = new(0, 21 + 1, 30, 79);
        }

        private void KeyboardStateMachineHandle(ConsoleKeyInfo key)
        {
            switch (_appState)
            {
                case AppState.READING:
                    switch (key.Key)
                    {
                        /* Изменение позиции просмариваемого товара в витрине */
                        case ConsoleKey.A: 
                            _assortmentsState.SelectedProductIndex--; 
                            break;
                        case ConsoleKey.D: 
                            _assortmentsState.SelectedProductIndex++; 
                            break;                        

                        /* Изменение позиции просматриваемой витрины */
                        case ConsoleKey.W: 
                            _assortmentsState.SelectedAssortmentIndex--; 
                            break;
                        case ConsoleKey.S: 
                            _assortmentsState.SelectedAssortmentIndex++; 
                            break;

                        /* Отсортировать по Id */
                        case ConsoleKey.I:
                            _assortmentsState.SortProductsById();
                            break;
                        /* Отсортировать по Имени */
                        case ConsoleKey.O:
                            _assortmentsState.SortProductsByName();
                            break;

                        /* Упаковать/Распаковать */
                        case ConsoleKey.B:
                            _assortmentsState.TogglePackage();
                            break;


                    }
                    break;
                default:
                    break;
            }
        }

        public void Run()
        {
            while (true)
            {

                _canvas.Clear();
                _topPanel.Render(_canvas);
              
                var topPanelBounds = _topPanel.GetBounds();

                _helpPanel.SetPositionAndSize(topPanelBounds.Right + 1, topPanelBounds.Bottom + 1, 30, 80);
                _helpPanel.Render(_canvas);

                _canvas.DrawBox(_topPanel.GetBounds().Right + 1, 0, Units.Vw(30), _topPanel.Height, "ДОПОЛНИТЕЛЬНО");


                //_canvas.DrawBox(Units.Vw(70) + 1, 0, Units.Vw(65), Units.Vh(80) - 1, "ИНСТРУКЦИЯ ПО ИСПОЛЬЗОВАНИЮ");

                _canvas.DrawBox(0, Units.Vh(21) + 1, Units.Vw(70), Units.Vh(80) - 1, "ИНФОРМАЦИЯ О ТОВАРЕ");
                _canvas.WriteText(3, Units.Vh(21) + 3, _assortmentsState.GetStringifiedSelectedProduct());
             
                _canvas.Render();
                _canvas.RefreshSize();

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true);
                    KeyboardStateMachineHandle(key);                    
                }

                Thread.Sleep(80);
            }
        }
    }
}
