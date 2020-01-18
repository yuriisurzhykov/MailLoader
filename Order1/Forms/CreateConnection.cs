using System;
using Renci.SshNet;
using Renci.SshNet.Sftp;
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

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void User_name_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void Server_box_TextChanged(object sender, EventArgs e)
        {

        }

        private void Db_name_TextChanged(object sender, EventArgs e)
        {

        }

        private void Password_box_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void Label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
