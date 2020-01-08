using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Order1
{
    class DatabaseCreator
    {
        private string createTables = "CREATE SEQUENCE s_mail;" +
                                      "CREATE SEQUENCE s_news;" +
                                      "CREATE SEQUENCE s_mail_news;" +
                                      "create table e_mails" +
                                      "(" +
                                          "id_mail INT PRIMARY KEY DEFAULT NEXTVAL('s_mail')," +
                                          "mail VARCHAR," +
                                          "valid_state BOOL DEFAULT false NOT NULL); " +
                                      "create table news" +
                                      "(" +
                                          "id_news INT PRIMARY KEY DEFAULT NEXTVAL('s_news')," +
                                          "date_news DATE DEFAULT CURRENT_DATE," +
                                          "news_name VARCHAR" +
                                      ");" +
                                      "create table mails_news" +
                                      "(" +
                                          "id_relation INT DEFAULT NEXTVAL('s_mail_news')," +
                                          "id_news INT REFERENCES news(id_news)," +
                                          "id_mail INT REFERENCES e_mails(id_mail)," +
                                          "mailing_date DATE," +
                                          "PRIMARY KEY(id_news, id_mail, id_relation)" +
                                      ");";
        private string createMailTrigger = "CREATE OR REPLACE FUNCTION add_mail() RETURNS TRIGGER " +
            "AS $$ " +
            "DECLARE id_mail_old INT; " +
            "BEGIN " +
                "SELECT M.id_mail INTO id_mail_old FROM e_mails M " +
                "WHERE M.mail = NEW.mail; " +
                "IF NOT FOUND THEN  " +
                    "RETURN NEW; " +
                "ELSE IF(SELECT M.valid_state FROM e_mails M WHERE M.id_mail = id_mail_old) = FALSE AND NEW.valid_state = TRUE THEN " +
                    "UPDATE e_mails M " +
                    "SET valid_state = NEW.valid_state " +
                    "WHERE id_mail = id_mail_old; " +
                "END IF; RETURN OLD; " +
            "END IF; " +
            "END; " +
            "$$ LANGUAGE 'plpgsql'; " +
            "CREATE TRIGGER insert_mail " +
            "BEFORE INSERT ON e_mails " +
            "FOR EACH ROW EXECUTE PROCEDURE add_mail();";


        private static string connectionString = Program.connectionString;

    }
}
