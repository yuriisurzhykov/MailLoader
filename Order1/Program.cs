using System;
using System.Windows.Forms;
using System.IO;
using Npgsql;

namespace Order1
{
    static class Program
    {
        public static string connectionString = "Server=localhost;User Id=postgres;Password=12345678;Database=Order_News;";
        private static string server = "Server=localhost;";
        private static string user = "User Id=postgres;";

        private static string[] password_database = new string[2];
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CreateConnectionString();
        }

        private static void CreateConnectionString()
        {
            try
            {
                password_database = File.ReadAllLines(Environment.SpecialFolder.ApplicationData + "conn.txt");
                CreateConnectionString(password_database[0], password_database[1]);
                Application.Run(new Form1());
            }
            catch (FileNotFoundException)
            {
                Application.Run(new CreateConnection());
            }
            catch(IndexOutOfRangeException)
            {
                Application.Run(new CreateConnection());
            }
        }

        public static bool CreateConnectionString(string password, string database)
        {
            try
            {
                connectionString = server + user + "Password=" + password + ";" + "Database=" + database + ";";
                Console.WriteLine(connectionString);
                NpgsqlConnection conn = new NpgsqlConnection(connectionString);
                conn.Open();
                conn.Close();
                FileStream fs = new FileStream(Environment.SpecialFolder.ApplicationData + "conn.txt", FileMode.Create);
                fs.Close();
                File.WriteAllLines(Environment.SpecialFolder.ApplicationData + "conn.txt", new string[] { password, database});
                return true;
            }
            catch(PostgresException)
            {
                return false;
            }
            catch(NpgsqlException)
            {
                return false;
            }
        }

        public static void Reconnection()
        {
            File.Delete(Environment.SpecialFolder.ApplicationData + "conn.txt");
            Application.Restart();
        }
    }
}
