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
        private static ListMails mails = new ListMails();
        private static List<ListMails.Mail> downloadedMails = new List<ListMails.Mail>();

        public static int AddMails(NpgsqlConnection conn, string[] mails, bool valid)
        {
            //Открываем соединение, если оно не открыто
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
            //Выгружаем все адреса из таблицы e_mails в список в ОЗУ
            DownloadMails(conn);
            int counterCoinciding = 0; // количество совпадений адресов(переданных в метод и находящихся в БД)
            foreach (var mail in mails)
            {
                //Проверяем адрес на существование в БД(то есть в списке, который только что получили)
                ListMails.Mail idMail = SqlCommander.mails.Exists(new ListMails.Mail(0, mail, valid));
                
                if (idMail != null)
                {
                    //Увеличиваем счетчик количества совпадений адресов
                    counterCoinciding++;
                    //Если есть, то преверяем валидность этого адреса и валидность указанную в методе
                    //если true, то меняем значение в БД
                    if (!idMail.validState && valid)
                    {
                        string sqlCommand = "UPDATE e_mails SET valid_state = true WHERE id_mail = " + idMail.id + ";";
                        var command = new NpgsqlCommand(sqlCommand, conn).ExecuteNonQuery();
                    }
                }
                else // Если не существует то добавляем в массив данных 
                {
                    int nextVal = (int)new NpgsqlCommand("NEXTVAL(s_mail)").ExecuteScalar();
                    downloadedMails.Add(new ListMails.Mail(nextVal, mail, valid));
                }
            }
            //int? idMail = SqlCommander.mails.Exists(new ListMails.Mail(0, "asuehfiuoawuov@dvijs.zxhauihfa", valid));
            //if (idMail != null)
            //{
            //    //counterCoinciding++;
            //    //string sqlQuery = "INSERT INTO e_mails(mail, valid_state) VALUES(" + mail + ", " + valid + "); ";
            //    //var command = new NpgsqlCommand(sqlQuery, conn);
            //}
            //else
            //{
            //    for(int i = 0; i < 3; i++)
            //    {
            //        Console.WriteLine("INSERT INTO e_mails VALUES(NEXTVAL('s_mail'), "+ "sd1@{0}.ua" +")", i);
            //    }
            //    //string sqlQuery = "INSERT INTO e_mails VALUES(NEXTVAL('s_mail')," + mail + ", " + valid + ");";
            //    //var command = new NpgsqlCommand(sqlQuery, conn);
            //}
            return counterCoinciding;
        }

        public static int InsertMails(NpgsqlConnection conn, string[] mails, bool valid, DateTime date, string news)
        {
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
            int counterCoinciding = 0;
            foreach(var mail in mails)
            {
                int? idMail = IsMailExists(conn, mail);
                if (idMail != null)
                {
                    counterCoinciding++;
                    string sqlQuery = "INSERT INTO e_mails(mail, valid_state) VALUES(" + mail + ", " + valid + "); ";
                    var command = new NpgsqlCommand(sqlQuery, conn);
                }
                else
                {
                    string sqlQuery = "INSERT INTO e_mails VALUES(NEXTVAL('s_mail')," + mail + ", " + valid + ");";
                    var command = new NpgsqlCommand(sqlQuery, conn);
                }
                int idNews = FindIdMailByName(conn, news);
                if(news != "")
                {
                    string sqlQuery = "INSERT INTO mails_news(id_news, id_mail, mailing_date) VALUES(" + idNews + "," + idMail + ", " + date + ");";
                    var command = new NpgsqlCommand();
                }
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
            int ? result = -1;
            result = (int)command.ExecuteScalar();

            return result;
        }


        private static void DownloadMails(NpgsqlConnection conn)
        {
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
            string sqlCommand = "SELECT * FROM e_mails";
            var command = new NpgsqlCommand(sqlCommand, conn);
            var rows = command.ExecuteReader();
            while(rows.Read())
            {
                mails.Add(new ListMails.Mail((int)rows[0], (string)rows[1], (bool)rows[2]));
                Console.WriteLine(mails[mails.Count - 1]);
            }
        }

        public class ListNews<T> where T : News
        {
            private List<T> newses = new List<T>();

            public void Add(T news)
            {
                newses.Add(news);
            }

            public News Find(string newsName)
            {
                return newses.Find(news => news.NewsName == newsName);
            }
        }

        public class News
        {
            public int Id { get; private set; }
            public string NewsName { get; private set; }

            public News(int id, string newsName)
            {
                Id = id;
                NewsName = newsName;
            }
        }

        private class ListMails
        {
            public int Count { get { return mails.Count; } private set { } } 
            private List<Mail> mails = new List<Mail>();

            public void Add(Mail mail)
            {
                Count++;
                mails.Add(mail);
            } 

            public Mail Exists(Mail mail)
            {
                return mails.Find(mails => mails.mail == mail.mail);
            }

            public Mail this[int index]
            {
                get
                {
                    return index >= 0 && index < Count ? mails[index] : null;
                }
                set
                {
                    mails[index] = value;
                }
            }

            public class Mail
            {
                public int id { get; private set; }
                public string mail { get; private set; }
                public bool validState { get; set; }

                public Mail(int id, string mail, bool validState)
                {
                    this.id = id;
                    this.mail = mail;
                    this.validState = validState;
                }

                public override bool Equals(object o)
                {
                    if(o is Mail)
                    {
                        return mail.CompareTo(((Mail)o).mail) == 0;
                    }
                    return false;
                }

                public override int GetHashCode()
                {
                    return id.GetHashCode() + mail.GetHashCode() + validState.GetHashCode();
                }

                public static bool operator ==(Mail left, Mail right)
                {
                    return left.mail.CompareTo(right.mail) == 0;
                }
                public static bool operator !=(Mail left, Mail right)
                {
                    return left.mail.CompareTo(right.mail) != 0;
                }

                public static implicit operator Mail(object[] obj)
                {
                    foreach (var item in obj)
                    {
                        Console.WriteLine(item);
                    }
                    return new Mail((int)obj[0], (string)obj[1], (bool)obj[2]);
                }
                public override string ToString()
                {
                    return id + ", " + mail + ", " + validState;
                }
            }

        }
    }
}
