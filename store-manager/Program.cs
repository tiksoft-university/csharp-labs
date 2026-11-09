using Assortment;
using Products;
using QR;
using SmartStorage;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using UI;
using UI.Core;
using UI.SizeUnits;

namespace StoreManager
{
    internal class Program
    {       


        static void Main(string[] args)
        {
            IProduct packagedProd_1 = new Package(new PowerTool(5, "Интерскол У310", 10000, "Очень крутой инструмент, позволяющий делать отверстия"));         
            IProduct packagedProd_2 = new Package(new PowerTool(6, "Интерскол У311", 10000, "Очень крутой инструмент, позволяющий делать отверстия"));         
            IProduct packagedProd_3 = new Package(new PowerTool(7, "Интерскол У312", 10000, "Очень крутой инструмент, позволяющий делать отверстия"));

            IProduct[] arr = [packagedProd_1, packagedProd_2, packagedProd_3]; 
            var canv = new CanvasEngine();
            int sas = 1;

            int[] list = [1, 2, 3];

            while (true)
            {

                if (Console.KeyAvailable)
                {
                    // Аргумент true (intercept) запрещает консоли самой печатать 
                    // нажатый символ на экране поверх твоих рамок
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    // Реакция на конкретные клавиши
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.A:
                            sas--;
                            break;
                        case ConsoleKey.D:
                            sas++;
                            break;                        
                    }
                }
                //canv.WriteLine(0, 0, canv.Width);
                //canv.WriteLine(0, 7, canv.Width);               
                //canv.WriteText(3, 2, $"Витрина {sas}: [10] [11] [12] [13] [14] [-] [-] [-] [-] [14] [-] [14]");
                //canv.WriteText(3, 3, "Витрина 2: [10] [11] [12] [13] [14] [-] [-] [-] [-] [14] [-] [14]");
                //canv.WriteText(3, 4, "Витрина 3: [10] [11] [12] [13] [14] [-] [-] [-] [-] [14] [-] [14]");                               
                //canv.WriteLine(0, 8, canv.Width / 2 - 1);

                canv.DrawBox(0, 0, SizeUnits.Vw(100), SizeUnits.Vh(20), "Шапка");

                for (int i = 0; i < list.Length; i++)
                {
                    int textX = 5 * i + 2;
                    int textY = 1;
                    ConsoleColor color = i == sas ? ConsoleColor.Red : ConsoleColor.White;

                    canv.WriteText(textX, textY, $"{10 + list[i]}", color);
                }

                canv.DrawBox(0, SizeUnits.Vh(20) + 1, SizeUnits.Vw(65), SizeUnits.Vh(80) - 1, "Тело_1");
                canv.DrawBox(SizeUnits.Vw(65) + 1, SizeUnits.Vh(20) + 1, SizeUnits.Vw(65), SizeUnits.Vh(80) - 1, "Тело_1");

                canv.WriteText(2, SizeUnits.Vh(20) + 3, arr[sas].ToString());

                canv.Render();
                canv.RefreshSize();           

            }

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
