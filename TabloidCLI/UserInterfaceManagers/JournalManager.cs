using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
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
                        {
                            Console.WriteLine("=====================================");
                            Console.WriteLine($"{journal.Id}) {journal.Title}");
                            Console.WriteLine("-------------------------------------");
                            Console.WriteLine(journal.Content);
                        }
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        return this;
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
            private Journal Choose()
            {
                Console.WriteLine("Enter the corresponding number of the journal you would like to choose.");
                List<Journal> _selectJournals = _journalRepository.GetAll();
                for(int i = 0; i < _selectJournals.Count; i++)
                {
                    Console.WriteLine($"{i}) {_selectJournals[i].Title}");
                }

                int _chosenJournal = int.Parse(Console.ReadLine());
                return _journalRepository.Get(_chosenJournal);
            }
            private void Add()
            {
                Console.WriteLine("Enter the new journal's title.");
                string _jTitle = Console.ReadLine();
                /*Title, Id, Content, CreateDateTime*/
                Console.WriteLine("Enter the new journal's content.");
                string _jContent = Console.ReadLine();
                Journal _newJournal = new Journal(){
                    Title = _jTitle,
                    Content = _jContent,
                    CreateDateTime = DateTime.Now
                };
                _journalRepository.Insert(_newJournal);
            }

            private void Edit()
            {
                Console.WriteLine("Select a journal to edit.");
                Journal _editJournal = Choose();

                Console.WriteLine("Enter a new title for the journal. (Leave blank for no change)");
                string _editTitle = Console.ReadLine();
                if(!string.IsNullOrWhiteSpace(_editTitle))
                {
                    _editJournal.Title = _editTitle;
                }
                Console.WriteLine("Enter new content for the journal. (Leave blank for no change)");
                string _editContent = Console.ReadLine();
                if(!string.IsNullOrWhiteSpace(_editContent))
                {
                    _editJournal.Content = _editContent;
                }
                _journalRepository.Update(_editJournal);
                Console.WriteLine("Journal has been updated. Press any key to continue...");
                Console.ReadKey();
            }
            private void Remove() 
            {
                Console.WriteLine("Select the journal you would like to delete.");
                Journal _journalToDelete = Choose();
                _journalRepository.Delete(_journalToDelete.Id);
                Console.WriteLine("Post has been deleted. Press any key to continue.");
                Console.ReadKey();
            }
    }
}
