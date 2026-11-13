using Products;
using store_manager.Model;
using UI.Core;

namespace store_manager.UI
{
    internal class ProductForm : Element
    {

        private readonly AssortmentsSlice _assortments;
        private readonly AppModeSlice _appMode;

        private string[] labels = ["ID", "Название", "Цена", "Описание", "Упаковать? (y/*)"];
        private const int FieldCount = 5;
        private string _id = "";
        private string _name = "";
        private string _price = "";
        private string _info = "";
        private string _needPackage = "";
        private int _currentField = 0;   // ← 0/1/2 — какое поле в фокусе

        public ProductForm(int x, int y, int width, int height, AssortmentsSlice assortments, AppModeSlice appMode) : base(x, y, width, height)
        {
            _assortments = assortments;
            _appMode = appMode;
        }

        private string GetCurrentField() => _currentField switch
        {
            0 => _id,
            1 => _name,
            2 => _price,
            3 => _info,
            4 => _needPackage,
            _ => ""
        };

        private string GetFieldByIndex(int index) => index switch
        {
            0 => _id,
            1 => _name,
            2 => _price,
            3 => _info,
            4 => _needPackage,
            _ => ""
        };


        private void ResetForm()
        {
            _id = "";
            _name = "";
            _price = "";
            _info = "";
            _needPackage = "";
        }

        private void UpdateForm(string value)
        {
            switch (_currentField)          
            {
                case 0: _id = value;  break;
                case 1: _name = value;  break;
                case 2: _price = value;  break;
                case 3: _info = value;  break;
                case 4: _needPackage = value;  break;
                default: break;
            }
        }

        private void CloseForm()
        {
            ResetForm();
            _appMode.State = AppMode.READING;
        }

        private void SubmitForm()
        {
            var needPackage = _needPackage.ToLower() == "y";
            Product product = new PowerTool(
                Int32.Parse(_id),
                _name,
                Decimal.Parse(_price),
                _info                
            );

            if(!needPackage)
            {
                _assortments.ReplaceSelectedProduct(product);
                return;
            }

            _assortments.ReplaceSelectedProduct(new Package(product));
        }

        public void HandleKeyboard(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {                
                case ConsoleKey.Escape:
                    CloseForm();
                    break;                
                case ConsoleKey.Enter:
                    // Не пропускаем, если пусто в текущем поле
                    if (GetCurrentField() == "") return;                                        
                    
                    // Отправка формы
                    if (_currentField == FieldCount - 1)
                    {
                        // Отправка формы
                        SubmitForm();
                        CloseForm();
                    }

                    // Переходим к следующему
                    _currentField++;
                    break;
                case ConsoleKey.Backspace:
                    var currentValue = GetCurrentField();
                    var newValue = currentValue.Remove(currentValue.Length - 1);

                    if(currentValue.Length > 0)
                    {
                        UpdateForm(newValue);
                        return;
                    }                    
                    break;

                default:
                    
                    var currentSymbol = key.KeyChar;
                    UpdateForm(GetCurrentField() + currentSymbol);
                    break;
            }
        }

        public override void Render(CanvasEngine canvas)
        {
            for (int i = 0; i < FieldCount; i++)
            {
                var label = labels[i];

                canvas.WriteText(X, Y + i, $"{label}: ", i == _currentField ? ConsoleColor.Cyan : ConsoleColor.White);
                canvas.WriteText(X + label.Length + 2, Y + i, GetFieldByIndex(i));
            }
            
        }
    }
}
