using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualProject
{
    class SelectMenu
    {
        static string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
        public static UserOptionList Menu(List<string> ListOfOptions)
        {
            int currentOption = 0;
            ConsoleKeyInfo currentKeyPressed;

            do
            {
                InputOutputAnimationControl.QuasarScreen("Not Registered");
                for (int option = 0; option < ListOfOptions.Count; option++)
                {
                    Console.ForegroundColor = (option == currentOption) ? ConsoleColor.Green : ConsoleColor.White;
                    Console.Write(ListOfOptions[option] + "\n");
                }
                currentKeyPressed = Console.ReadKey();

                if (currentKeyPressed.Key == ConsoleKey.UpArrow)
                {
                    if (currentOption == 0)
                    {
                        currentOption = ListOfOptions.Count - 1;
                    }
                    else
                    {
                        currentOption--;
                    }
                }
                else if (currentKeyPressed.Key == ConsoleKey.DownArrow)
                {
                    if (currentOption == ListOfOptions.Count -1)
                    {
                        currentOption = 0;
                    }
                    else
                    {
                        currentOption++;
                    }
                }
            }
            while (currentKeyPressed.Key != ConsoleKey.Enter);
            InputOutputAnimationControl.QuasarScreen("Not Registered");
            Console.ForegroundColor = ConsoleColor.White;

            return new UserOptionList()
            {
                NameOfChoice = ListOfOptions[currentOption],
                IndexOfChoice = currentOption
            };

        }
    }

    public struct UserOptionList
    {
        public string NameOfChoice;
        public int IndexOfChoice;
    }
}
