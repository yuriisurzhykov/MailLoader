using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Order1
{
    public partial class CreateConnection : Form
    {
        private string server;
        private string user_id;
        private string password;
        private string database;
        public CreateConnection()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            server = server_box.Text;
            user_id = user_name.Text;
            password = password_box.Text;
            database = db_name.Text;
            if(!Program.CreateConnectionString(server, user_id, password, database))
            {
                MessageBox.Show("Не правильный пароль \nили имя базы данных!", "Ошибка подключения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Application.Restart();
            }
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }
    }
}
