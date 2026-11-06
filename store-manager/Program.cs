using Assortment;
using Products;
using QR;
using System.Runtime.CompilerServices;

namespace StoreManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IProduct prod = new PowerTool(7, "Интерскол У350", 10000, "Очень крутой инструмент, позволяющий делать отверстия");
            IProduct packagedProd = new Package(new PowerTool(5, "Интерскол У310", 10000, "Очень крутой инструмент, позволяющий делать отверстия"));         
            IProduct pack = new Package(new PowerTool(4, "Интерскол sУ310", 10000, "Очень крутой инструмент, позволяющий делать отверстия"));         

            IAssortment<IProduct> storage = (Assortment<IProduct>)10;
            IAssortment<IProduct> second = (Assortment<IProduct>)(7, 5);

            storage.Add(prod);
            storage.AddAt(5, packagedProd);

            Console.WriteLine(storage);
            storage.SortById();

            Console.WriteLine(storage);

            second[0] = storage[0];

            Console.WriteLine(second);
            Console.WriteLine(packagedProd);
            second.Replace(0, pack);
            second.Replace(0, packagedProd);

            Console.WriteLine(second);


            IAssortment<IProduct> st = new SmartAssortment<IProduct>(4, 3);
            st[2] = pack;

            Console.WriteLine(st);
            st.Id = 10;
            st.SortById();
            Console.WriteLine(st);
            //Console.WriteLine(st.Find(4));


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
