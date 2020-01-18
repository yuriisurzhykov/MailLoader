using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Order1;

namespace Core.SpecialClasses
{
    class ListMails
    {
        public int Count { get { return mails.Count; } private set { } }
        public readonly List<Mail> mails = new List<Mail>();
        private bool isNewMails;

        public ListMails(bool isNewMails)
        {
            this.isNewMails = isNewMails;
        }

        public void Add(Mail mail)
        {
            Count++;
            mails.Add(mail);
        }

        public void DeleteAll()
        {
            Count = 0;
            mails.Clear();
        }

        public Mail Exists(Mail mail)
        {
            return mails.Find(mails => mails.MailAddress == mail.MailAddress);
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

        public override string ToString()
        {
            string returns = "";
            //if (!isNewMails)
            //{
            //    var conn = new NpgsqlConnection(Program.connectionString);
            //    conn.Open();
            //    long nextVal = (long)new NpgsqlCommand("SELECT NEXTVAL('s_mail');", conn).ExecuteScalar();
            //    conn.Close();
            //    mails.ForEach(item => {
            //        returns += "(NEXTVAL('s_mail'), '" + item.MailAddress + "', " + item.ValidState + "),\n";
            //    });
            //}
            //else
            //{
            //    mails.ForEach(item => {
            //        returns += "(NEXTVAL('s_mail'), '" + item.MailAddress + "', " + item.ValidState + "),\n";
            //    });
            //}
            mails.ForEach(item => returns += "(" + item.Id + ", '" + item.MailAddress + "', " + item.ValidState + "),\n");
            returns = returns.Remove(returns.Length - 2);
            return returns;
        }

        public string ToString(int idNews, DateTime dateTime)
        {
            string returns = "";
            string dateString = dateTime.Date.Day + "-" + dateTime.Date.Month + "-" + dateTime.Date.Year;
            mails.ForEach(item => returns += "(NEXTVAL('s_mail_news'), " + idNews + ", " + item.Id +", '" + dateString + "'),\n");
            try
            {
                returns = returns.Remove(returns.Length - 2);
            }
            catch (Exception)
            {
                return "";
            }
            return returns;
        }

        public class Mail
        {
            public int Id { get; set; }
            public string MailAddress { get; private set; }
            public bool ValidState { get; set; }

            public Mail(int id, string mail, bool validState)
            {
                this.Id = id;
                this.MailAddress = mail;
                this.ValidState = validState;
            }

            public override bool Equals(object o)
            {
                if (o is Mail)
                {
                    return MailAddress.CompareTo(((Mail)o).MailAddress) == 0;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode() + MailAddress.GetHashCode() + ValidState.GetHashCode();
            }

            //public static bool operator ==(Mail left, Mail right)
            //{
            //    return left.MailAddress.CompareTo(right.MailAddress) == 0;
            //}
            //public static bool operator !=(Mail left, Mail right)
            //{
            //    return left.MailAddress.CompareTo(right.MailAddress) != 0;
            //}

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
                return Id + ", " + MailAddress + ", " + ValidState;
            }
        }

    }
}
