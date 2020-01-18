using System;
using System.Collections;
using System.Collections.Generic;
using Npgsql;

namespace Core.SpecialClasses
{
    static class SqlCommander
    {
        /*
         *Попробовать сделать так. Выгружать всю базу данных на компьютер, и уже здесь лопатить, 
         * и делать все проверки, и потом уже делать обычный insert по критериям. По другому не получиться ускорить.
         */
        private static ListMails mails = new ListMails(false);
        private static ListMails downloadedMails = new ListMails(true);
        private static ListNews listNews = new ListNews();

        public static int AddMails(NpgsqlConnection conn, List<string> mails, bool valid)
        {
            //Открываем соединение, если оно не открыто
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
            //Выгружаем все адреса из таблицы e_mails в список в ОЗУ
            int x = DownloadMails(conn);
            int counterCoinciding = 0; // количество совпадений адресов(переданных в метод и находящихся в БД)
            mails.ForEach(item => {
                ListMails.Mail idMail = SqlCommander.mails.Exists(new ListMails.Mail(0, item, valid));
                if (idMail != null)
                {
                    //Увеличиваем счетчик количества совпадений адресов
                    counterCoinciding++;
                    //Если есть, то преверяем валидность этого адреса и валидность указанную в методе
                    //если true, то меняем значение в БД
                    if (!idMail.ValidState && valid)
                    {
                        var conn2 = new NpgsqlConnection(conn.ConnectionString);
                        conn2.Open();
                        string sqlCommand = "UPDATE e_mails SET valid_state = true WHERE id_mail = " + idMail.Id + ";";
                        var command = new NpgsqlCommand(sqlCommand, conn2).ExecuteNonQuery();
                        conn2.Close();
                    }
                }
                else // Если не существует то добавляем в массив данных 
                {
                    var conn2 = new NpgsqlConnection(conn.ConnectionString);
                    conn2.Open();
                    long nextVal = (long)new NpgsqlCommand("SELECT NEXTVAL('s_mail');", conn2).ExecuteScalar();
                    downloadedMails.Add(new ListMails.Mail((int)nextVal, item, valid));
                    conn2.Close();
                }
            });

            #region Код на всякий случай
            //for (int i = 0; i < mails.Count; i++)
            //{
            //    //Проверяем адрес на существование в БД(то есть в списке, который только что получили)
            //    ListMails.Mail idMail = SqlCommander.mails.Exists(new ListMails.Mail(0, mails[i], valid));

            //    if (idMail != null)
            //    {
            //        //Увеличиваем счетчик количества совпадений адресов
            //        counterCoinciding++;
            //        //Если есть, то преверяем валидность этого адреса и валидность указанную в методе
            //        //если true, то меняем значение в БД
            //        if (!idMail.ValidState && valid)
            //        {
            //            var conn2 = new NpgsqlConnection(conn.ConnectionString);
            //            conn2.Open();
            //            string sqlCommand = "UPDATE e_mails SET valid_state = true WHERE id_mail = " + idMail.Id + ";";
            //            var command = new NpgsqlCommand(sqlCommand, conn2).ExecuteNonQuery();
            //            conn2.Close();
            //        }
            //    }
            //    else // Если не существует то добавляем в массив данных 
            //    {
            //        var conn2 = new NpgsqlConnection(conn.ConnectionString);
            //        conn2.Open();
            //        long nextVal = (long)new NpgsqlCommand("SELECT NEXTVAL('s_mail');", conn2).ExecuteScalar();
            //        downloadedMails.Add(new ListMails.Mail((int)nextVal, mails[i], valid));
            //        conn2.Close();
            //    }
            //}
            #endregion

            if (downloadedMails.Count != 0)
            {
                string insertQuery = "INSERT INTO e_mails VALUES" + downloadedMails.ToString() + ";";
                var conn2 = new NpgsqlConnection(conn.ConnectionString);
                conn2.Open();
                var command = new NpgsqlCommand(insertQuery, conn2).ExecuteNonQuery();
                conn2.Close();
            }
            return counterCoinciding;
        }

        public static int AddMails(NpgsqlConnection conn, List<string> mails, bool valid, DateTime date, string news)
        {
            int counterCoinciding = AddMails(conn, mails, valid);
            DownloadNews(conn);
            ListNews.News news1 = listNews.Find(news);

            //for (int i = 0; i < SqlCommander.mails.Count; i++)
            //{
            //    ListNews.NewsMails newsMails = listNews.Find(SqlCommander.mails[i].Id, news1.Id);
            //    if (newsMails != null && newsMails.DateTime < date)
            //    {
            //        string sqlQuery = "UPDATE mails_news SET mailing_date = " + date.ToString() + " WHERE id_relation = " + newsMails.IdRel + ";";
            //        var conn2 = new NpgsqlConnection(conn.ConnectionString);
            //        conn2.Open();
            //        _ = new NpgsqlCommand(sqlQuery, conn2).ExecuteNonQuery();
            //        conn2.Close();
            //    }
            //}

            //for (int i = 0; i < downloadedMails.Count; i++)
            //{
            //    string getNextValQuery = "SELECT NEXTVAL('s_mail_news');";
            //    var conn2 = new NpgsqlConnection(conn.ConnectionString);
            //    conn2.Open();
            //    int nextVal = (int)new NpgsqlCommand(getNextValQuery, conn2).ExecuteScalar();
            //    string sqlInsertQuery = "INSERT INTO mails_news VALUES " + ";";
            //    conn.Close();
            //}

            Console.WriteLine(SqlCommander.mails.mails.Count + "  SqlCommander.mails.mails.");
            string sqlInsertQuery = "INSERT INTO mails_news VALUES\n";
            string dateTimeString = date.Date.Day + "-" + date.Date.Month + "-" + date.Date.Year;
            bool relationWasAdd = false;
            //Анализ адресов которые уже были в базе
            SqlCommander.mails.mails.ForEach(item =>
            {
                ListNews.NewsMails newsMails = listNews.Find(item.Id, news1.Id);
                if (newsMails != null && newsMails.DateTime < date)
                {
                    Console.WriteLine(item.MailAddress + " Was in DB");
                    string sqlQuery = "UPDATE mails_news SET mailing_date = '" + dateTimeString + "' WHERE id_relation = " + newsMails.IdRel + ";";
                    var conn2 = new NpgsqlConnection(conn.ConnectionString);
                    conn2.Open();
                    _ = new NpgsqlCommand(sqlQuery, conn2).ExecuteNonQuery();
                    conn2.Close();
                }
                else
                {
                    relationWasAdd = true;
                    Console.WriteLine(item.Id);
                    sqlInsertQuery += "(NEXTVAL('s_mail_news'), " +  news1.Id + ", " + item.Id + ", '" + dateTimeString + "'),\n";
                }
            });

            Console.WriteLine(downloadedMails.mails.Count);
            //Анализ адресов которых не было в базе
            if(downloadedMails.mails.Count != 0)
            {
                var conn2 = new NpgsqlConnection(conn.ConnectionString);
                conn2.Open();
                sqlInsertQuery += downloadedMails.ToString(news1.Id, date) + ";";
                Console.WriteLine(sqlInsertQuery);
                new NpgsqlCommand(sqlInsertQuery, conn2).ExecuteNonQuery();
                conn.Close();
            }
            else if(relationWasAdd)
            {
                sqlInsertQuery = sqlInsertQuery.Remove(sqlInsertQuery.Length - 2);
                var conn2 = new NpgsqlConnection(conn.ConnectionString);
                conn2.Open();
                sqlInsertQuery += downloadedMails.ToString(news1.Id, date) + ";";
                Console.WriteLine(sqlInsertQuery);
                new NpgsqlCommand(sqlInsertQuery, conn2).ExecuteNonQuery();
                conn.Close();
            }
            return counterCoinciding;
        }

        private static int FindIdMailByName(NpgsqlConnection conn, string newsName)
        {
            return (int)new NpgsqlCommand("SELECT id_news FROM news WHERE news_name = " + newsName + ";", conn).ExecuteScalar();
        }

        private static int? IsMailExists(NpgsqlConnection conn, string mail)
        {
            Console.WriteLine("SELECT id_mail FROM e_mails WHERE mail = " + mail + ";");
            var command = new NpgsqlCommand("SELECT id_mail FROM e_mails WHERE mail = '" + mail + "';", conn);
            int? result = -1;
            result = (int)command.ExecuteScalar();

            return result;
        }

        private static void DownloadNews(NpgsqlConnection conn)
        {
            var connection = new NpgsqlConnection(conn.ConnectionString);
            connection.Open();
            string sqlCommand = "SELECT * FROM mails_news;";
            var commandResult = new NpgsqlCommand(sqlCommand, connection).ExecuteReader();
            while (commandResult.Read())
            {
                listNews.Add(new ListNews.NewsMails((int)commandResult[0],
                                                    (int)commandResult[1],
                                                    (int)commandResult[2],
                                                    (DateTime)commandResult[3]));
            }
            var connection2 = new NpgsqlConnection(conn.ConnectionString);
            connection2.Open();
            sqlCommand = "SELECT * FROM news;";
            commandResult = new NpgsqlCommand(sqlCommand, connection2).ExecuteReader();
            while (commandResult.Read())
            {
                listNews.Add(new ListNews.News((int)commandResult[0], (string)commandResult[2]));
            }
            connection.Close();
            connection2.Close();
        }

        private static int DownloadMails(NpgsqlConnection conn)
        {
            var connection = new NpgsqlConnection(conn.ConnectionString);
            connection.Open();
            string sqlCommand = "SELECT * FROM e_mails";
            var command = new NpgsqlCommand(sqlCommand, connection);
            var rows = command.ExecuteReader();
            while (rows.Read())
            {
                mails.Add(new ListMails.Mail((int)rows[0], (string)rows[1], (bool)rows[2]));
                Console.WriteLine(mails[mails.Count - 1]);
            }
            connection.Close();
            return mails.Count;
        }

        public static void DeleteAll()
        {
            mails.DeleteAll();
            downloadedMails.DeleteAll();
            listNews.DeleteAll();
        }
    }
}
