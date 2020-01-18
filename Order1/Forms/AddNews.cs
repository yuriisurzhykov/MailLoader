using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Order1
{
    public partial class AddNews : Form
    {
        private string connectionString = Program.connectionString;
        public AddNews()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != "")
            {
                NpgsqlConnection conn = new NpgsqlConnection(connectionString);
                conn.Open();
                var command = new NpgsqlCommand("INSERT INTO news(news_name) VALUES(@NewsName);", conn);
                var parameter = new NpgsqlParameter();
                parameter.ParameterName = "NewsName";
                parameter.Value = textBox1.Text;
                command.Parameters.Add(parameter);
                int result = command.ExecuteNonQuery();
                if(result == 1)
                {
                    MessageBox.Show("Новость успешно добавлена!");
                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении новости!");
                }
                conn.Close();
            }
        }

        private void AddNews_Load(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
