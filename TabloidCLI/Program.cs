using System;
using TabloidCLI.UserInterfaceManagers;

namespace TabloidCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            const string CONNECTION_STRING = @"Server=127.0.0.1; Database=TabloidCLI; User Id=sa; Password=MyPass@word;integrated security=true;TrustServerCertificate=true; Trusted_Connection=false;";

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

