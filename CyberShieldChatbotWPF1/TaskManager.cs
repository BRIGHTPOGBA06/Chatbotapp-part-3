using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CyberShieldChatbotWPF1
{
    public class CyberTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Reminder { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }

    public class TaskManager
    {
        private readonly string connectionString = "Server=localhost;Database=CyberShieldDB;Uid=root;Pwd=your_password;";

        public bool AddTask(string title, string description, string reminder)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Tasks (Title, Description, ReminderDate) VALUES (@Title, @Description, @Reminder)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@Reminder", reminder);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DB Error: {ex.Message}");
                return false;
            }
        }

        public List<CyberTask> GetAllTasks()
        {
            var tasks = new List<CyberTask>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Id, Title, Description, ReminderDate, IsCompleted FROM Tasks";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tasks.Add(new CyberTask
                                {
                                    Id = reader.GetInt32("Id"),
                                    Title = reader.GetString("Title"),
                                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString("Description"),
                                    Reminder = reader.IsDBNull(reader.GetOrdinal("ReminderDate")) ? "No reminder" : reader.GetString("ReminderDate"),
                                    IsCompleted = reader.GetBoolean("IsCompleted")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DB Read Error: {ex.Message}");
            }
            return tasks;
        }
    }
}