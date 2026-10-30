using QR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products
{
    public abstract class Product : IProduct
    {
        /// <summary>
        /// Хранение ID
        /// </summary>
        private int _id;

        /// <summary>
        /// Хранение цены
        /// </summary>
        private decimal _price;

        /// <summary>
        /// Хранение информации по QR
        /// </summary>
        private IQRController _qr;

        /// <summary>
        /// Информация по QR-коду
        /// </summary>
        public IQRController QRCode { get => _qr; }

        /// <summary>
        /// Создание продукта. QR-код полиморфный для наследников 
        /// </summary>
        protected Product(int id, string name, decimal price, IQRController qr) {
            _qr = qr;
            Id = id;
            Name = name;
            Price = price;
            
        }

        /// <summary>
        /// Обновление инфы в QR-коде
        /// </summary>
        protected virtual void UpdateQr() => _qr.Text = $"{_id}:{_price}";

        /// <summary>
        /// Установка/Получение ID
        /// </summary>
        public virtual int Id {
            get => _id;
            set { 
                _id = value;
                UpdateQr();
            }
        }

        /// <summary>
        /// Установка/Получение цены
        /// </summary>
        public virtual decimal Price
        {
            get => _price;
            set {
                _price = value; 
                UpdateQr(); 
            }
        }
         
        /// <summary>
        /// Наименование товара
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Получение типа товара
        /// </summary>
        public abstract string Type { get; }
                   
        /// <summary>
        /// Получение информации о товаре
        /// </summary>
        public abstract string Info { get; }


        public override string ToString()
        {
            return $"{Type} {Name} {Price}руб\n{Info}\n{QRCode}";
        }
    }
}
