using System;
using System.Collections.Generic;
using System.Drawing;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    public class ColorManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;

        public ColorManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;

        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Color Menu");
            Console.WriteLine(" 1) Choose Background color");
            Console.WriteLine(" 2) Choose Foreground color");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Background();
                    return this;
                case "2":
                    Foreground();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }
        private void Background()
        {
            Console.WriteLine("Here is a list of your background color options:");
            foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
            {
                Console.WriteLine($"{color}");
            }
            Console.Write("Type out the color you want:");
            string colorChoice = Console.ReadLine();
            ConsoleColor consoleColor = ConsoleColor.White;
            try
            {
                consoleColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorChoice, true);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid choice");
            }

            Console.BackgroundColor = consoleColor;
        }
    

        private void Foreground()
        {
            Console.WriteLine("Here is a list of your foreground color options:");
            foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
            {
                Console.WriteLine($"{color}");
            }
            Console.Write("Type out the color you want:");
            string colorChoice = Console.ReadLine();
            ConsoleColor consoleColor = ConsoleColor.White;
            try
            {
                consoleColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorChoice, true);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid choice");
            }

            Console.ForegroundColor = consoleColor;
        }
    }
}

