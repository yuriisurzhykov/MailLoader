using System;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.Windows.Forms;
using Order1;

namespace Core.SSH
{
    class SSHConnector
    {
        private string _server = "188.127.231.217";
        private string _user = "root";
        private string _pass = "kjLKnklKJH8768";
        private string _port = "5432";

        public SSHConnector(string server, string user, string password, string port)
        {
            this._server = server;
            this._user = user;
            this._pass = password;
            this._port = port;
        }

        public bool Open()
        {
            try
            {
                using (var client = new SshClient(_server, _user, _pass)) // establishing ssh connection to server where MySql is hosted
                {
                    client.Connect();
                    if (client.IsConnected)
                    {
                        var portForwarded = new ForwardedPortLocal("127.0.0.1", 3306, "127.0.0.1", 3306);
                        client.AddForwardedPort(portForwarded);
                        portForwarded.Start();
                        client.Disconnect();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка подключения к защищенному серверу!",
                                    "Ошибка подключения!",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
