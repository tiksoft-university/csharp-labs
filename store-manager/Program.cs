using Assortment;
using Products;
using QR;

namespace StoreManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IProduct prod = new PowerTool(7, "Интерскол У350", 10000, "Очень крутой инструмент, позволяющий делать отверстия");
            IProduct packagedProd = new Package(new PowerTool(5, "Интерскол У310", 10000, "Очень крутой инструмент, позволяющий делать отверстия"));         

            IAssortment<IProduct> storage = (Assortment<IProduct>)10;
            IAssortment<IProduct> second = (Assortment<IProduct>)5;
            second.Id = 2;

            storage.Add(prod);
            storage.AddAt(5, packagedProd);

            Console.WriteLine(storage);

            storage.SortById();


            Console.WriteLine(storage);

            second[0] = storage[0];


            Console.WriteLine(second);

            //foreach (var product in products)
            //{
            //    Console.WriteLine(product);
            //    product.Id++;
            //    product.Price++;
            //    Console.WriteLine(product);
            //}
        }
    }
}
