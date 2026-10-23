using QR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products
{
    internal interface IProduct
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя товара
        /// </summary>
        public int Name { get; }
       
        /// <summary>
        /// Цена товара
        /// </summary>
        public int Price { get; }

        /// <summary>
        /// Тип товара
        /// </summary>
        public int Type { get; }

        /// <summary>
        /// QR-код товара
        /// </summary>
        public IQRController QRCode { get; }
    }
}
