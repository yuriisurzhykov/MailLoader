using System;
using System.Windows.Forms;
using Core.SSH;

namespace Order1.Forms
{
    public partial class SSHConn : Form
    {
        private string server;
        private string port;
        private string username;
        private string password;


        public SSHConn()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            server = server_text.Text;
            port = port_text.Text;
            username = user_text.Text;
            password = password_text.Text;

            SSHConnector conn = new SSHConnector(server, username, password, port);
            conn.Open();
        }
    }
}
