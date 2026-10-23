using Products;
using QR;

namespace StoreManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IQRController.OUTPUT_MODE = OutputMode.Full;
            var qr = new PackageQRController();            
            
            while (true)
            {
                Console.WriteLine("Input the text for get QR: ");
                qr.Text = Console.ReadLine();             
                Console.WriteLine(qr);
            }
        }
    }
}
