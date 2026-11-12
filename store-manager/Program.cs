using store_manager;

namespace StoreManager
{
    internal class Program
    {       
        static void Main(string[] args)
        {

            var app = new Application();
            app.Run();



            //Console.CursorVisible = false;

            //while (true)
            //{
            //    Console.SetCursorPosition(0, 0);
            //    Console.Write("W/S: Assortment | A/D: Позиция | E: ID");
            //    Console.Write(packagedProd);
            //}


            //ISmartProduct prod = new SmartProduct(7, "Интерскол У350", 10000, "Очень крутой инструмент, позволяющий делать отверстия");
            //IProduct packagedProd = new Package(new PowerTool(5, "Интерскол У310", 10000, "Очень крутой инструмент, позволяющий делать отверстия"));         
            //SmartPackage pack = new SmartPackage(new SmartProduct(4, "Интерскол sУ310", 10000, "Очень крутой инструмент, позволяющий делать отверстия"));         

            //SmartAssortment<ISmartProduct> st = (1, 3);

            //st[2] = pack;

            //Console.WriteLine(st);
            //st.Id = 10;
            //st.SortById();
            //Console.WriteLine(st);


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
