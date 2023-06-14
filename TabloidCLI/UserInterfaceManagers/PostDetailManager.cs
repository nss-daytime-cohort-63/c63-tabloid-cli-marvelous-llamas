﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    internal class PostDetailManager : IUserInterfaceManager
    {
        private IUserInterfaceManager _parentUI;
        private PostRepository _postRepository;
        private TagRepository _tagRepository;
        private NoteRepository _noteRepository;
        private int _postId;
        private string _connectionString; // Add the connection string field

        public PostDetailManager(IUserInterfaceManager parentUI, string connectionString, int postId)
        {
            _parentUI = parentUI;
            _postRepository = new PostRepository(connectionString);
            _tagRepository = new TagRepository(connectionString);
            _noteRepository = new NoteRepository(connectionString);
            _postId = postId;
            _connectionString = connectionString; // Assign the provided connection string
        }

        public IUserInterfaceManager Execute()
        {
            Post post = _postRepository.Get(_postId);
            Console.WriteLine($"{post.Title} Details");
            Console.WriteLine(" 1) View Tags");
            Console.WriteLine(" 2) Add Tag");
            Console.WriteLine(" 3) Remove Tag");
            Console.WriteLine(" 4) Note Management");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    View();
                    return this;
                case "2":
                    //AddTag();
                    return this;
                case "3":
                    //RemoveTag();
                    return this;
                case "4":
                    return new NoteManager(this, _connectionString, _postId); // Pass the connection string
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void View()
        {
            Post post = _postRepository.Get(_postId);
            try
            {
                foreach (Tag tag in post.Tags)
                {
                    Console.WriteLine(" " + tag);
                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("No tags.");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
