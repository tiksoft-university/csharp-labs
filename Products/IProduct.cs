using QR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products
{
    public interface IProduct
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя товара
        /// </summary>
        public string Name { get; }
       
        /// <summary>
        /// Цена товара
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Тип товара
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// QR-код товара
        /// </summary>
        public IQRController QRCode { get; }

        /// <summary>
        /// Информация о товаре
        /// </summary>
        public string Info { get; }
    }
}
