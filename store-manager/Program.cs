using Assortment;
using Products;
using QR;
using SmartStorage;
using store_manager.Model;
using store_manager.UI;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using UI;
using UI.Core;

namespace StoreManager
{
    internal class Program
    {       
        static void Main(string[] args)
        {

            /* ###### STATE START ######*/
            IProduct packagedProd_1 = new Package(new PowerTool(5, "Интерскол У310", 10000, "Очень крутой инструмент, позволяющий делать отверстия"));         
            IProduct packagedProd_2 = new Package(new PowerTool(6, "Интерскол У311", 10000, "Очень крутой инструмент, позволяющий делать отверстия"));         
            IProduct packagedProd_3 = new Package(new PowerTool(7, "Интерскол У312", 10000, "Очень крутой инструмент, позволяющий делать отверстия"));

            IProduct[] arr = [packagedProd_1, packagedProd_2, packagedProd_3];
                
            int[] list = [1, 2, 3];

            int sas = 1;
            bool is_Adding = !false;

            /* ###### STATE START ######*/


            AssortmentsSlice assortmentsSlice = new();


            /* ###### STATE END ###### */



            /* ###### COMPONENTS START ######*/
            var canv = new CanvasEngine();
            AssortmentsBar assortmentsBar = new(0, 0, 100, 21, assortmentsSlice);

            /* ###### COMPONENTS END ######*/

            while (true)
            {                
                // RENDER СЕКЦИИ СКЛАДОВ
                assortmentsBar.Render(canv);

                // СЕКЦИЯ СКЛАДОВ
                if (is_Adding) {
                    
                    canv.DrawBox(0, Units.Vh(21) + 1, Units.Vw(70), Units.Vh(80) - 1, "ТЕКУЩИЙ ЭЛЕМЕНТ");
                    canv.WriteText(3, Units.Vh(21) + 3, assortmentsSlice.GetSelectedProductString());
                }
                
                canv.DrawBox(Units.Vw(70) + 1, Units.Vh(21) + 1, Units.Vw(65), Units.Vh(80) - 1, "ИНСТРУКЦИЯ");              
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
