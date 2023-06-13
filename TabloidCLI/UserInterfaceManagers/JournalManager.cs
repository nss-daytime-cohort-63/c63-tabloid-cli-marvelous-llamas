using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    internal class JournalManager : IUserInterfaceManager
    {
        private IUserInterfaceManager _parentUI;
        private JournalRepository _journalRepository;

        public JournalManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _journalRepository = new JournalRepository(connectionString);
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Journal Menu");
            Console.WriteLine(" 1) List Journals");
            Console.WriteLine(" 2) Journal Details");
            Console.WriteLine(" 3) Add Journal");
            Console.WriteLine(" 4) Edit Journal");
            Console.WriteLine(" 5) Remove Journal");
            Console.WriteLine(" 0) Go Back");

            string choice = Console.ReadLine();
            switch (choice)
            {
                default:
                    Console.WriteLine("Invalid selection");
                    return this;
                case ("1"):
                    List();
                    return this;
                case ("2"):
                    Journal journal = Choose();
                    if(journal == null)
                    {
                        return this;
                    }
                    else
                    {
                        return new JournalDetailsManager(this, _connectionString, journal.Id);
                    }
                case ("3"):
                    Add();
                    return this;
                case ("4"):
                    Edit();
                    return this;
                case ("5"):
                    Remove();
                    return this;
                case ("0"):
                    return _parentUI;
            }
            
        }
            private void List()
            {
                List<Journal> _journals = _journalRepository.GetAll();

                for (int i = 0; i < _journals.Count; i++)
                {
                    Console.WriteLine($"{i}) {_journals[i].Title}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }

            private void Add()
            {
                Console.WriteLine("Enter the Journal's title.");
                string _jTitle = Console.ReadLine();
                /*Title, Id, Content, CreateDateTime*/
            }
    }
}
