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
        public static void SetAssortmentCode(this IProduct product, int assortmentId, int index)
        {
            if (product == null || assortmentId < 0 || index < 0) return;

            product.QRCode.Text = $"{assortmentId}:{index}:{product.Id}:{product.Price}";
        }

        /// <summary>
        /// Метод расширения для установки текста кода по складу и позиции товара на складе
        /// </summary>
        public static void SetAssortmentCode<T>(this T product, IAssortment<T> assortment) 
            where T : class, IProduct
        {
            if (product == null) return;

            var index = 0;

            product.SetAssortmentCode(assortment.Id, index);
        }
    }
}
