using Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStorage
{
    public sealed class SmartPackage : Package, ISmartProduct, IDisposable
    {
        private ISmartProduct _product;

        /// <summary>Конструктор умной упаковки</summary>
        public SmartPackage(ISmartProduct product) : base(product) {
            _product = product;
            _product.EventIdPriceOrChanged += OnProductDataChange;
        }

        /// <summary>Идентификатор</summary>
        public int Id
        {
            get => base.Id;
            set
            {
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

        public void OnProductDataChange(object? sender, EventIdPriceChangedArgs e)
        {            
            Id = e.Id;
            Price = e.Price * (decimal)1.1;
        }        
          
        /// <summary>Событие изменения</summary>
        public event EventHandler<EventIdPriceChangedArgs>? EventIdPriceOrChanged;        

        /// <summary>Отписка от события при финализации</summary>
        public void Dispose()
        {
             _product.EventIdPriceOrChanged -= OnProductDataChange;
        }
    }
}
