using System;
using System.Windows.Forms;
using System.IO;
using Npgsql;
using Core.SSH;
using Order1.Forms;

namespace Order1
{
    static class Program
    {
        public static string connectionString = "Server=188.127.231.217;User Id=postgres;Password=kjLKnklKJH8768;Database=root;";

        private static string[] password_database = new string[4];
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

        public static void CreateConnectionString()
        {
            try
            {
                // Читаем их файла параметры для строки поключения
                password_database = File.ReadAllLines(Environment.SpecialFolder.ApplicationData + "conn.txt");
                CreateConnectionString(password_database[0], 
                                       password_database[1],
                                       password_database[2],
                                       password_database[3]);
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

        /// <summary>
        /// Попытка создать поключение к БД
        /// </summary>
        /// <param name="server"> Адрес сервера подключения</param>
        /// <param name="user_id"> Имя пользователя БД</param>
        /// <param name="password"> Пароль к БД</param>
        /// <param name="database"> Название БД</param>
        /// <returns>
        /// true - подключение удалось
        /// false - подключение не удалось
        /// </returns>
        public static bool CreateConnectionString(string server, string user_id, string password, string database)
        {
            //Попытка подключения к БД
            try
            {
                //Создаем строку подключения
                connectionString = "Server=" + server + ";" +
                                   "User Id=" + user_id + ";" +
                                   "Password=" + password + ";" +
                                   "Database=" + database + ";";
                Console.WriteLine(connectionString);
                //Пробуем подключиться к БД
                try
                {
                    NpgsqlConnection conn = new NpgsqlConnection(connectionString);
                    conn.Open();
                    conn.Close();
                    FileStream fs = new FileStream(Environment.SpecialFolder.ApplicationData + "conn.txt", FileMode.Create);
                    fs.Close();
                    // Записываем в файл настройки для подключения к БД
                    File.WriteAllLines(Environment.SpecialFolder.ApplicationData + "conn.txt",
                                       new string[] { server, user_id, password, database });
                }
                catch(System.Net.Sockets.SocketException)
                {
                    MessageBox.Show("Время ожидания подключения к серверу истекло\n" +
                                    "Проверьте соединение с интернетом и повторите запуск приложения!",
                                    "Время ожидания истекло",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                }
                catch (TimeoutException)
                {
                    MessageBox.Show("Время ожидания подключения к серверу истекло\n" +
                                    "Проверьте соединение с интернетом и повторите запуск приложения!", 
                                    "Время ожидания истекло", 
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Warning);
                }
                return true;
            }
            catch(PostgresException) // Если что-то пошло не так, значит неправильно 
                                     // указан 1 из 4-х параметров строки подключения
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
