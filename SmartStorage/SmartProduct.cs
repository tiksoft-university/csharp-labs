using Products;

namespace SmartStorage
{
    public sealed class SmartProduct : PowerTool, ISmartProduct
    {

        /// <summary>Конструктор умного продукта</summary>
        public SmartProduct(int id, string name, decimal price, string info = "Информация о товаре") : base(id, name, price, info) { }

        /// <summary>Идентификатор</summary>
        public int Id { 
            get => base.Id; 
            set {
                if (base.Id == value) return;
                EventIdPriceOrChanged?.Invoke(this, new EventIdPriceChangedArgs(value, Price));
            } 
        }

        /// <summary>Цена</summary>
        public decimal Price
        {
            get => base.Price;
            set
            {
                if (base.Price == value) return;
                EventIdPriceOrChanged?.Invoke(this, new EventIdPriceChangedArgs(Id, value));
            }
        }




        /// <summary>Событие изменения ID или цены продукта</summary>
        public event EventHandler<EventIdPriceChangedArgs>? EventIdPriceOrChanged;  
    }
}
