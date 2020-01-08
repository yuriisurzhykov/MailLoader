using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace SpecialClasses
{
    /*
     * Разделить программу на классы: импортер и экспортер. 
     * Каждый из них сделать так, чтобы он не был привязан к конкретный БД.
     * Сделать это таким образом: создается интерфейс, которые описывает подключение, импорт, 
     * экспорт и т.п. Далее создается класс который реализует подключение именно к базе данных PostgreSQL. И он 
     * работает с классами Importer и Exporter. Только вот как сделать, чтобы эти 2 класса не были зависимы от конкретной БД ??
     * Можно при старте программы сделать, чтобы создавался экземпляр класса Importer и в него закидывался нужный класс, 
     * который реализует 
     */
    class Importer
    {
        public class ImportInstance
        {
            private string mail;
            private DateTime date;
            private string news;
            private bool valid;

            NpgsqlParameter param1 = new NpgsqlParameter();

            public ImportInstance(string mail, bool valid)
            {
                this.mail = mail;
                this.valid = valid;
            }

            public ImportInstance(string mail, bool valid, string news, DateTime date) : this(mail, valid)
            {
                this.news = news;
                this.date = date;
            }

            public string Mail { get { return mail; } set { mail = value; } }
            public DateTime Date { get { return date; } set { date = value; } }
            public string News { get { return news; } set { news = value; } }
            public bool Valid { get { return valid; } set { valid = value; } }
        }

        //public bool UploadData(ImportInstance importInstance)
        //{

        //}
    }
}

