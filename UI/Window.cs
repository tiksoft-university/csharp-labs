using UI.Core;

namespace UI
{
    public class Window
    {
        public static void Run()
        {
            ScreenBuffer screen = new ScreenBuffer(Console.WindowWidth, Console.WindowHeight - 1);

            string text = "sas";

            while (true)
            {                
                screen.Resize(Console.WindowWidth, Console.WindowHeight - 1);
                screen.Render();

                foreach (var symbol in text)
                {
                    
                }


                Thread.Sleep(100);
            }
        }
    }
}
