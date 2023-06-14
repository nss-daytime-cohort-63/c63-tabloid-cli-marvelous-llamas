using System;
using System.Collections.Generic;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    public class NoteManager : IUserInterfaceManager
    {
     

        private readonly NoteRepository _noteRepository;
        private readonly IUserInterfaceManager parent;
        private readonly int _postId;

        public NoteManager(IUserInterfaceManager Parent, string _connectionString, int PostId)
        {
            _noteRepository = new NoteRepository(_connectionString);
            parent = Parent;
            _postId = PostId;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Note Management");

            Console.WriteLine(" 1) List Notes");
            Console.WriteLine(" 2) Add Note");
            Console.WriteLine(" 3) Edit Note");
            Console.WriteLine(" 4) Remove Note");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ListNotes();
                    return this;
                case "2":
                    AddNote();
                    return this;
                case "3":
                    EditNote();
                    return this;
                case "4":
                    RemoveNote();
                    return this;
                case "0":
                    return parent;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void ListNotes()
        {
            List<Note> notes = _noteRepository.GetAll();

            if (notes.Count == 0)
            {
                Console.WriteLine("No notes found.");
                return;
            }

            Console.WriteLine("Notes:");
            foreach (Note note in notes)
            {
                Console.WriteLine($"- {note.Title}");
                Console.WriteLine($"- {note.CreateDateTime}");
                Console.WriteLine($"- {note.Content}");
                Console.WriteLine($"===================================");
            }
        }

        private void AddNote()
        {
            Console.WriteLine("Creating a new note:");
            Console.Write("Enter title: ");
            string title = Console.ReadLine();
            Console.Write("Enter content: ");
            string content = Console.ReadLine();

            Note newNote = new Note
            {
                Title = title,
                Content = content,
                CreateDateTime = DateTime.Now,
                PostId = _postId
            };

            _noteRepository.Insert(newNote);

            Console.WriteLine("Note created successfully.");
        }

        private void EditNote()
        {
            Console.WriteLine("Editing a note:");

            List<Note> notes = _noteRepository.GetAll();
            if (notes.Count == 0)
            {
                Console.WriteLine("No notes found.");
                return;
            }

            Console.WriteLine("Available Notes:");
            for (int i = 0; i < notes.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {notes[i].Title}");
            }

            Console.Write("Enter the number of the note to edit: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int noteIndex) && noteIndex > 0 && noteIndex <= notes.Count)
            {
                Note selectedNote = notes[noteIndex - 1];

                Console.WriteLine($"Editing Note: {selectedNote.Title}");
                Console.WriteLine("Enter new title (leave empty to keep the existing title): ");
                string newTitle = Console.ReadLine();
                Console.WriteLine("Enter new content (leave empty to keep the existing content): ");
                string newContent = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(newTitle))
                {
                    selectedNote.Title = newTitle;
                }

                if (!string.IsNullOrWhiteSpace(newContent))
                {
                    selectedNote.Content = newContent;
                }

                _noteRepository.Update(selectedNote);

                Console.WriteLine("Note edited successfully.");
            }
            else
            {
                Console.WriteLine("Invalid note number.");
            }
        }


        private void RemoveNote()
        {
            Console.WriteLine("Removing a note:");

            List<Note> notes = _noteRepository.GetAll();
            if (notes.Count == 0)
            {
                Console.WriteLine("No notes found.");
                return;
            }

            Console.WriteLine("Available Notes:");
            for (int i = 0; i < notes.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {notes[i].Title}");
            }

            Console.Write("Enter the number of the note to remove: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int noteIndex) && noteIndex > 0 && noteIndex <= notes.Count)
            {
                Note selectedNote = notes[noteIndex - 1];

                Console.WriteLine($"Removing Note: {selectedNote.Title}");

                Console.Write("Are you sure you want to remove this note? (Y/N): ");
                string confirmation = Console.ReadLine();

                if (confirmation.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    _noteRepository.Delete(selectedNote.Id);
                    Console.WriteLine("Note removed successfully.");
                }
                else
                {
                    Console.WriteLine("Removal canceled.");
                }
            }
            else
            {
                Console.WriteLine("Invalid note number.");
            }
        }

    }
}
