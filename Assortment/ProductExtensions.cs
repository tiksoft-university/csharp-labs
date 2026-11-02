using Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assortment
{
    public static class ProductExtensions       
    {
        /// <summary>
        /// Метод расширения для установки текста кода по Id склада, товару и его позиции на складе
        /// </summary>
        public static void SetCode(this IProduct product, int assortmentId, int index)
        {
            product.QRCode.Text = $"{assortmentId}:{index}:{product.Id}:{product.Price}";
        }

        /// <summary>
        /// Метод расширения для установки текста кода по складу и позиции товара на складе
        /// </summary>
        public static void SetCode<T>(this IProduct product, IAssortment<T> assortment, int index)
        {
            product.SetCode(assortment.Id, index);
        }
    }
}
