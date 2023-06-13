using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using TabloidCLI.Models;

namespace TabloidCLI.Repositories
{
    public class JournalRepository : DatabaseConnector, IRepository<Journal>
    {
        public JournalRepository(string connectionString) : base(connectionString) { }


        public void Insert(Journal journal)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Journal (Title, Content, CreateDateTime )
                                                     VALUES (@title, @content, @createDateTime)";
                    cmd.Parameters.AddWithValue("@title", journal.Title);
                    cmd.Parameters.AddWithValue("@content", journal.Content);
                    cmd.Parameters.AddWithValue("@createDateTime", journal.CreateDateTime);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Journal> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT id,
                                               Title,
                                               Content,
                                               CreateDateTime
                                          FROM Journal";

                    List < Journal> journals = new List<Journal>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Journal journal = new Journal()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                        };
                        journals.Add(journal);
                    }

                    reader.Close();

                    return journals;
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
                    cmd.CommandText = @"DELETE FROM Journal WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Journal journal)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Journal 
                                           SET Title = @title,
                                               Content = @content,
                                               CreateDateTime = @createDateTime
                                         WHERE id = @id";

                    cmd.Parameters.AddWithValue("@title", journal.Title);
                    cmd.Parameters.AddWithValue("@content", journal.Content);
                    cmd.Parameters.AddWithValue("@createDateTime", journal.CreateDateTime);
                    cmd.Parameters.AddWithValue("@id", journal.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Journal Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT j.Id AS JournalId,
                                               j.Title,
                                               j.Content,
                                               j.CreateDateTime
                                          FROM Journal j
                                         WHERE j.id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    Journal journal = null;

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                            journal = new Journal()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("JournalId")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Content = reader.GetString(reader.GetOrdinal("Content")),
                                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            };
                    }

                    reader.Close();

                    return journal;
                }
            }
        }
    }
}



