using Products;
using QR;

namespace StoreManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IProduct prod = new PowerTool(1, "Интерскол У350", 10000, "Очень крутой инструмент, позволяющий делать отверстия");
            IProduct packagedProd = new Package(new PowerTool(1, "Интерскол У310", 10000, "Очень крутой инструмент, позволяющий делать отверстия"));

            IProduct[] products = [prod, packagedProd];

            foreach (var product in products)
            {
                Console.WriteLine(product);
                product.Id++;
                product.Price++;
                Console.WriteLine(product);
            }
        }
    }
}
