using System;
using TabloidCLI.UserInterfaceManagers;

namespace TabloidCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            const string CONNECTION_STRING = @"Data Source=localhost\SQLEXPRESS07;Database=TabloidCLI;Integrated Security=True";

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
