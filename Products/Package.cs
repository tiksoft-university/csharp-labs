using QR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products
{
    public class Package : Product
    {
        /// <summary>
        /// Упакованный продукт
        /// </summary>
        private IProduct _product;

        /// <summary>
        /// Конструктор упаковки
        /// </summary>
        public Package(IProduct product) : 
            base(
                product.Id,
                product.Name,
                product.Price * (decimal)1.1, 
                new PackageQRController()
        ) {
            _product = product;
        }

        public override string Type { get => $"Упаковка: {_product.Type}";}

        public override string Info { get => _product.Info; }
    }
}
