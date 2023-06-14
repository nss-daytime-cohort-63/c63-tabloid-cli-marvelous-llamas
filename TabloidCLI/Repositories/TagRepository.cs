﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Data.SqlClient;
using TabloidCLI.Models;
using TabloidCLI.Repositories;
using TabloidCLI.UserInterfaceManagers;

namespace TabloidCLI
{
    public class TagRepository : DatabaseConnector, IRepository<Tag>
    {
        public TagRepository(string connectionString) : base(connectionString) { }

        public List<Tag> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT id, Name FROM Tag";
                    List<Tag> tags = new List<Tag>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Tag tag = new Tag()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                        };
                        tags.Add(tag);
                    }

                    reader.Close();

                    return tags;
                }
            }
        }

        public Tag Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT t.Id AS TagId,
                                               t.Name
                                          FROM Tag t
                                         WHERE t.id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    Tag tag = null;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (tag == null)
                        {
                            tag = new Tag()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("TagId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            };
                        }
                    }

                    reader.Close();

                    return tag;
                }
            }
        }

        public void Insert(Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Tag ( Name )
                                                     VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", tag.Name);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Tag 
                                           SET Name = @name
                                         WHERE id = @id";

                    cmd.Parameters.AddWithValue("@name", tag.Name);
                    cmd.Parameters.AddWithValue("@id", tag.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Tag WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public SearchResults<Author> SearchAuthors(string tagName)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT a.id,
                                               a.FirstName,
                                               a.LastName,
                                               a.Bio
                                          FROM Author a
                                               LEFT JOIN AuthorTag at on a.Id = at.AuthorId
                                               LEFT JOIN Tag t on t.Id = at.TagId
                                         WHERE t.Name LIKE @name";
                    cmd.Parameters.AddWithValue("@name", $"%{tagName}%");
                    SqlDataReader reader = cmd.ExecuteReader();

                    SearchResults<Author> results = new SearchResults<Author>();
                    while (reader.Read())
                    {
                        Author author = new Author()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            Bio = reader.GetString(reader.GetOrdinal("Bio")),
                        };
                        results.Add(author);
                    }

                    reader.Close();

                    return results;
                }
            }

        }
        public SearchResults<Blog> SearchBlogs(string tagName)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT b.Id,
                                       b.Title,
                                       b.URL
                                  FROM Blog b
                                       LEFT JOIN BlogTag bt ON b.Id = bt.BlogId
                                       LEFT JOIN Tag t ON t.Id = bt.TagId
                                 WHERE t.Name LIKE @name";
                    cmd.Parameters.AddWithValue("@name", $"%{tagName}%");
                    SqlDataReader reader = cmd.ExecuteReader();

                    SearchResults<Blog> results = new SearchResults<Blog>();
                    while (reader.Read())
                    {
                        Blog blog = new Blog()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            URL = reader.GetString(reader.GetOrdinal("URL")),
                        };
                        results.Add(blog);
                    }

                    reader.Close();

                    return results;
                }
            }
        }

        public SearchResults<Post> SearchPosts(string tagName)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT p.Id,
                                       p.Title,
                                       p.URL,
                                       CONVERT(NVARCHAR(50), p.PublishDateTime, 20) AS PublishDateTime
                                  FROM Post p
                                       LEFT JOIN PostTag pt ON p.Id = pt.PostId
                                       LEFT JOIN Tag t ON t.Id = pt.TagId
                                 WHERE t.Name LIKE @name";
                    cmd.Parameters.AddWithValue("@name", $"%{tagName}%");
                    SqlDataReader reader = cmd.ExecuteReader();

                    SearchResults<Post> results = new SearchResults<Post>();
                    while (reader.Read())
                    {
                        Post post = new Post()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            URL = reader.GetString(reader.GetOrdinal("URL")),
                            PublishDateTime = DateTime.Parse(reader.GetString(reader.GetOrdinal("PublishDateTime"))),
                        };
                        results.Add(post);
                    }
                    reader.Close();
                    return results;
                }
            }
        }

        public SearchResults<object> SearchAll(string tagName)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT 'Author' AS Type, a.Id, a.FirstName, a.LastName, CAST(a.Bio AS VARCHAR(MAX)) AS Bio
                                    FROM Author a
                                    LEFT JOIN AuthorTag at ON a.Id = at.AuthorId
                                    LEFT JOIN Tag t ON t.Id = at.TagId
                                    WHERE t.Name LIKE @name

                                    UNION
                                    
                                    SELECT 'Blog' AS Type, b.Id, b.Title, b.URL, NULL AS Bio
                                    FROM Blog b
                                    LEFT JOIN BlogTag bt ON b.Id = bt.BlogId
                                    LEFT JOIN Tag t ON t.Id = bt.TagId
                                    WHERE t.Name LIKE @name

                                    UNION

                                    SELECT 'Post' AS Type, p.Id, p.Title, p.URL, NULL AS Bio
                                    FROM Post p
                                    LEFT JOIN PostTag pt ON p.Id = pt.PostId
                                    LEFT JOIN Tag t ON t.Id = pt.TagId
                                    WHERE t.Name LIKE @name";

                    cmd.Parameters.AddWithValue("@name", $"%{tagName}%");
                    SqlDataReader reader = cmd.ExecuteReader();

                    SearchResults<object> results = new SearchResults<object>();
                    while (reader.Read())
                    {
                        string type = reader.GetString(reader.GetOrdinal("Type"));

                        if (type == "Author")
                        {
                            Author author = new Author()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Bio = reader.GetString(reader.GetOrdinal("Bio")),
                            };
                            results.Add(author);
                        }
                        else if (type == "Blog")
                        {
                            Blog blog = new Blog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                URL = reader.GetString(reader.GetOrdinal("URL")),
                            };
                            results.Add(blog);
                        }
                        else if (type == "Post")
                        {
                            Post post = new Post()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                URL = reader.GetString(reader.GetOrdinal("URL")),
                            };
                            results.Add(post);
                        }
                    }

                    reader.Close();

                    return results;
                }
            }
        }
    }
}

