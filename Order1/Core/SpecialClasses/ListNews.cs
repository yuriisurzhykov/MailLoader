using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SpecialClasses
{
    class ListNews
    {
        private List<News> newses = new List<News>();
        private List<NewsMails> newsMails = new List<NewsMails>();

        public void Add(News news)
        {
            newses.Add(news);
        }

        public void DeleteAll()
        {
            newses.Clear();
            newsMails.Clear();
        }

        public void Add(NewsMails newsMails)
        {
            this.newsMails.Add(newsMails);
        }

        /// <summary>
        /// Возвращает объект класса News по названию новости
        /// </summary>
        /// <param name="newsName">Название новости по которому идет поиск</param>
        /// <returns></returns>
        public News Find(string newsName)
        {
            return newses.Find(news => news.NewsName == newsName);
        }

        private class NewsMailsComparer : IComparer<NewsMails>
        {
            public int Compare(NewsMails x, NewsMails y)
            {
                if (x.IdMail == y.IdMail && x.IdNews == y.IdNews)
                    return 0;
                return -1;
            }
        }

        /// <summary>
        /// Возвращает объект класса NewsMails из таблицы news_mail по id новости и id почты
        /// </summary>
        /// <param name="idNews">id новости</param>
        /// <param name="idMail">id почты</param>
        /// <returns></returns>
        public NewsMails Find(int idNews, int idMail)
        {
            return newsMails.Find(item => item.IdMail == idMail && item.IdNews == idNews);
            //try
            //{
            //    return newsMails[newsMails.BinarySearch(new NewsMails(0, idMail, idNews, DateTime.Now), new NewsMailsComparer())];
            //}
            //catch(Exception)
            //{
            //    return null;
            //}
        }

        private void sel()
        {
            /*
              CREATE OR REPLACE FUNCTION add_connect() RETURNS TRIGGER
              AS $$
              DECLARE
              id_rel INT;
              BEGIN
                  SELECT id_relation INTO id_rel FROM mails_news MN
                  WHERE MN.id_news = NEW.id_news
                  AND MN.id_mail = NEW.id_mail;
                  IF NOT FOUND THEN
                          RETURN NEW;
                  ELSE
                      IF((SELECT MN.mailing_date FROM mails_news MN WHERE MN.id_relation = id_rel)::DATE - NEW.mailing_date::DATE < 0) THEN
                          UPDATE mails_news
                          SET mailing_date = NEW.mailing_date
                          WHERE id_relation = id_rel;
                      END IF;
                  RETURN OLD;
                  END IF;
              END;
              $$ LANGUAGE 'plpgsql';
            */
        }

        public class NewsMails
        {
            public int IdRel { get; set; }
            public int IdMail { get; set; }
            public int IdNews { get; set; }
            public DateTime DateTime { get; set; }

            public NewsMails(int idRel, int idmail, int idNews, DateTime date)
            {
                IdRel = idRel;
                IdMail = idmail;
                IdNews = idNews;
                DateTime = date;
            }

            public override bool Equals(object obj)
            {
                if(obj is NewsMails)
                {
                    NewsMails temp = obj as NewsMails;
                    return IdNews == temp.IdNews && IdMail == temp.IdMail;
                }
                return false;
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
    }
}
