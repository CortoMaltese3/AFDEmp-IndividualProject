using System;

namespace IndividualProject
{
    class ColorAndAnimationControl
    {                        
        public static void UniversalLoadingOuput(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(message);
            DotsBlinking();
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
            Console.ResetColor();
        }

        public static void DotsBlinking()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            for (int blink = 0; blink < 5; blink++)
            {
                switch (blink)
                {
                    case 0: Console.Write("."); break;
                    case 1: Console.Write("."); break;
                    case 2: Console.Write("."); break;
                    case 3: Console.Write("."); break;
                    case 4: Console.Write("."); break;
                }
                System.Threading.Thread.Sleep(200);
                Console.SetCursorPosition(Console.CursorLeft + 0, Console.CursorTop + 0);
            }
            Console.ResetColor();
        }
        
        public static void ColoredText(string text, ConsoleColor frontColor)
        {
            Console.ForegroundColor = frontColor;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.Black;
        }
    }
}