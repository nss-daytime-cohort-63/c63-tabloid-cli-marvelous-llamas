using System;
using TabloidCLI.UserInterfaceManagers;

namespace TabloidCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            MainMenuManager mainMenu = new MainMenuManager();
            IUserInterfaceManager ui = mainMenu;
            while (ui != null)
            {
                ui = ui.Execute();
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

