using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noted
{
    internal class DataLayer
    {
        public class NoteRepository
        {
            private string connectionString = "Host=localhost;Username=postgres;Password=root;Database=Noted";

            public List<Note> GetNotes()
            {
                List<Note> notes = new List<Note>();

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM notes";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Note note = new Note
                                {
                                    NoteId = Convert.ToInt32(reader["note_id"]),
                                    WindowId = Convert.ToInt32(reader["window_id"]),
                                    Title = reader["title"].ToString(),
                                    Content = reader["content"].ToString(),
                                    CreatedAt = Convert.ToDateTime(reader["created_at"])
                                };

                                notes.Add(note);
                            }
                        }
                    }
                }

                return notes;
            }
            public Note GetNoteById(int noteId)
            {
                Note note = null;

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM notes WHERE note_id = @NoteId";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NoteId", noteId);

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                note = new Note
                                {
                                    NoteId = Convert.ToInt32(reader["note_id"]),
                                    WindowId = Convert.ToInt32(reader["window_id"]),
                                    Title = reader["title"].ToString(),
                                    Content = reader["content"].ToString()
                                    // Дополнительные поля, если они есть
                                };
                            }
                        }
                    }
                }

                return note;
            }
        }
    }

   }
