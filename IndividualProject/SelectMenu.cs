using System;
using System.Collections.Generic;

namespace IndividualProject
{
    class SelectMenu
    {        
        //Creates the Vertical options Menu
        public static UserOptionList MenuColumn(List<string> ListOfOptions, string currentUser, string message)
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            int currentOption = 0;
            ConsoleKeyInfo currentKeyPressed;
            do
            {
                //Resets the console when a key is pressed and the next option is highlighted
                OutputControl.QuasarScreen(currentUser);                
                Console.WriteLine(message);
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
            OutputControl.QuasarScreen(currentUser);
            
            Console.ForegroundColor = ConsoleColor.White;

            return new UserOptionList()
            {
                option = ListOfOptions[currentOption],
                tempOption = currentOption
            };
        }

        //Creates the Horizontal options Menu
        public static UserOptionList MenuRow(List<string> ListOfOptions, string currentUser, string message)
        {
            int currentOption = 0;
            ConsoleKeyInfo currentKeyPressed;

            do
            {
                OutputControl.QuasarScreen(currentUser);
                Console.WriteLine(message);                
                for (int option = 0; option < ListOfOptions.Count; option++)
                {
                    Console.ForegroundColor = (option == currentOption) ? ConsoleColor.Green : ConsoleColor.White;                    
                    Console.Write(ListOfOptions[option] + "\t\t");
                }
                currentKeyPressed = Console.ReadKey();

                if (currentKeyPressed.Key == ConsoleKey.LeftArrow)
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
                else if (currentKeyPressed.Key == ConsoleKey.RightArrow)
                {
                    if (currentOption == ListOfOptions.Count - 1)
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
            OutputControl.QuasarScreen(currentUser);

            Console.ForegroundColor = ConsoleColor.White;

            return new UserOptionList()
            {
                option = ListOfOptions[currentOption],
                tempOption = currentOption
            };
        }
    }

    public struct UserOptionList
    {
        public string option;
        public int tempOption;
    }
}
    