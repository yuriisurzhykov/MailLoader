using System;
using System.Windows.Forms;
using Npgsql;
using System.IO;

namespace Order1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Import importWin = new Import();
            importWin.Show();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            new Export().Show();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(Program.connectionString);
                conn.Open();
            }
            catch(PostgresException)
            {
                Program.connectionString = "Server=localhost;User Id=postgres;Password=;Database=Order_News;";
                try
                {
                    NpgsqlConnection conn = new NpgsqlConnection(Program.connectionString);
                    conn.Open();
                }
                catch (PostgresException)
                {
                    Program.connectionString = "Server=localhost;User Id=postgres;Password=;Database=postgres;";
                }
            }
        }

        private void ВыходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            new AddNews().Show();
        }

        private void СменитьБазуДанныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Reconnection();
        }

        private void СвязьСРазработчикомToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Communication().Show();
        }
    }
}
