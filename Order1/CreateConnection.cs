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

        private string password;
        private string database;
        public CreateConnection()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            password = textBox1.Text;
            database = textBox2.Text;
            if(!Program.CreateConnectionString(password, database))
            {
                MessageBox.Show("Не правильный пароль \nили имя базы данных!", "Ошибка подключения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Application.Restart();
            }
        }
    }
}
