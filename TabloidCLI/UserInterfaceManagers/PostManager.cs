using System;
using System.Collections.Generic;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    public class PostManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private readonly PostRepository _postRepository;
        private readonly AuthorRepository _authorRepository; // Add this field
        private readonly BlogRepository _blogRepository; // Add this field
        private readonly string _connectionString;

        public PostManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _postRepository = new PostRepository(connectionString);
            _authorRepository = new AuthorRepository(connectionString); // Instantiate AuthorRepository
            _blogRepository = new BlogRepository(connectionString); // Instantiate BlogRepository
            _connectionString = connectionString;
        }


        public IUserInterfaceManager Execute()
        {
            // Display the Post Management Menu options
            Console.WriteLine("Post Management Menu");
            Console.WriteLine(" 1) List Posts");
            Console.WriteLine(" 2) View Post Details");
            Console.WriteLine(" 3) Add Post");
            Console.WriteLine(" 4) Edit Post");
            Console.WriteLine(" 5) Remove Post");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Post post = Choose("Which post would you like to view?");
                    ViewDetails(post);
                    return new PostDetailManager(this, _connectionString, post.Id);
                case "3":
                    Add();
                    return this;
                case "4":
                    Edit();
                    return this;
                case "5":
                    Remove();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void List()
        {
            // Retrieve all posts from the repository
            List<Post> posts = _postRepository.GetAll();

            // Display each post
            foreach (Post post in posts)
            {
                Console.WriteLine(post.Title);
            }
        }

        private void ViewDetails(Post post)
        {
            // Choose a post to view its details
            if (post == null)
            {
                return;
            }

            // Display the selected post's details
            Console.WriteLine();
            Console.WriteLine($"Title: {post.Title}");
            Console.WriteLine($"URL: {post.URL}");
            Console.WriteLine($"Publish Date: {post.PublishDateTime}");
            Console.WriteLine($"Author: {post.Author.FullName}");
            Console.WriteLine($"Blog: {post.Blog.Title}");
        }

        private void Add()
        {
            // Create a new post
            Console.WriteLine("New Post");
            Post post = new Post();

            // Prompt the user to provide the post details
            Console.Write("Title: ");
            post.Title = Console.ReadLine();

            Console.Write("URL: ");
            post.URL = Console.ReadLine();

            Console.Write("Publish Date (yyyy-mm-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime publishDate))
            {
                post.PublishDateTime = publishDate;
            }
            else
            {
                Console.WriteLine("Invalid Publish Date");
                return;
            }

            // Select an author for the post
            Author selectedAuthor = SelectAuthor();
            if (selectedAuthor == null)
            {
                Console.WriteLine("Invalid Author Selection");
                return;
            }
            post.Author = selectedAuthor;

            // Select a blog for the post
            Blog selectedBlog = SelectBlog();
            if (selectedBlog == null)
            {
                Console.WriteLine("Invalid Blog Selection");
                return;
            }
            post.Blog = selectedBlog;

            // Insert the new post into the repository
            _postRepository.Insert(post);
        }


        private void Edit()
        {
            // Choose a post to edit
            Post postToEdit = Choose("Which post would you like to edit?");
            if (postToEdit == null)
            {
                return;
            }

            // Prompt the user to provide the updated post details
            Console.WriteLine();
            Console.Write("New title (blank to leave unchanged: ");
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                postToEdit.Title = title;
            }
            Console.Write("New URL (blank to leave unchanged: ");
            string URL = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(URL))
            {
                postToEdit.URL = URL;
            }
            Console.Write("New publish date (yyyy-mm-dd) (blank to leave unchanged: ");
            string publishDateInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(publishDateInput) && DateTime.TryParse(publishDateInput, out DateTime publishDate))
            {
                postToEdit.PublishDateTime = publishDate;
            }


            // Update the post in the repository
            _postRepository.Update(postToEdit);
        }

        private void Remove()
        {
            // Choose a post to remove
            Post postToDelete = Choose("Which post would you like to remove?");
            if (postToDelete != null)
            {
                // Delete the selected post from the repository
                _postRepository.Delete(postToDelete.Id);
            }
        }

        private Post Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a Post:";
            }

            Console.WriteLine(prompt);

            // Retrieve all posts from the repository
            List<Post> posts = _postRepository.GetAll();

            // Display each post with a numeric index
            for (int i = 0; i < posts.Count; i++)
            {
                Post post = posts[i];
                Console.WriteLine($" {i + 1}) {post.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return posts[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }
        private Author SelectAuthor()
        {
            Console.WriteLine("Select an Author:");

            // Retrieve all authors from the repository
            List<Author> authors = _authorRepository.GetAll();

            // Display each author with a numeric index
            for (int i = 0; i < authors.Count; i++)
            {
                Author author = authors[i];
                Console.WriteLine($" {i + 1}) {author.FullName}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return authors[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }

        private Blog SelectBlog()
        {
            Console.WriteLine("Select a Blog:");

            // Retrieve all blogs from the repository
            List<Blog> blogs = _blogRepository.GetAll();

            // Display each blog with a numeric index
            for (int i = 0; i < blogs.Count; i++)
            {
                Blog blog = blogs[i];
                Console.WriteLine($" {i + 1}) {blog.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return blogs[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }

    }
}
