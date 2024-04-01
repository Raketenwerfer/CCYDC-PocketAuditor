using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Auditor_Admin_Panel.Auxiliaries
{
    public class DatabaseInitiator
    {

        public string connectionString;
        private string server, port, database, username, password;

        public DatabaseInitiator(string _server, string _db, string _username, string _password)
        {
            // Database details will be supplied upon application start-up
            // If no database connection is established, a prompt must be
            // displayed to input database details
            server = _server;
            // 3306 for network connections, 3000 for none
            port = "3306";
            database = _db;
            username = _username;
            password = _password;

            //connectionString = $"Server={server};Port={port};Database={database};User ID={username};Password={password};";
            connectionString = $"Server={server};Port={port};Database={database};User ID={username};Password={password};";
        }

        public MySqlConnection GetConnection()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString);
                return connection;
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it, show a message)
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}
