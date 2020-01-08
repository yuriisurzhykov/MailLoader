using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Npgsql;
using SpecialClasses;

namespace Order1
{
    public enum ValidState
    {
        VALID = 1, 
        INVALID = 2,
        ANY = 3
    }
    public enum NewsState
    {
        ALL,
        NOTHING,
        NOT_ENTERED
    }

    public partial class Export : Form
    {
        bool ignoreDate;

        private bool isDateSelected;
        private bool isValidSelected;
        private bool isAmountSelected;
        private bool isNewsSelected;

        private string newsName;
        DateTime selectedDate;
        ValidState validState;
        NewsState newsState;

        private string filePath;
        long amountMails = 0;


        private string connectionString = Program.connectionString;

        public Export()
        {
            InitializeComponent();
        }

        private void Export_Load(object sender, EventArgs e)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();
            var reader = new NpgsqlCommand("SELECT news_name FROM news", conn).ExecuteReader();
            int counter = 0;
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[counter].ToString());
            }
            checkBox3.Checked = false;
            checkBox3.Enabled = false;
            conn.Close();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                ignoreDate = true;
                dateTimePicker1.Enabled = false;
                validState = ValidState.INVALID;
                newsState = NewsState.NOTHING;
                radioButton2.Checked = true;
                radioButton1.Enabled = false;
                radioButton3.Enabled = false;
                checkBox3.Checked = true;
                checkBox3.Enabled = true;
                checkBox2.Checked = false;
                checkBox2.Enabled = false;
                checkBox4.Checked = false;
                checkBox4.Enabled = false;
            }
            else
            {
                ignoreDate = false;
                dateTimePicker1.Enabled = true;
                radioButton1.Enabled = true;
                radioButton3.Enabled = true;
                checkBox2.Enabled = true;
                checkBox3.Enabled = false;
                checkBox3.Checked = false;
                checkBox4.Enabled = true;
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "[^0-9]"))
            {
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
            }
            if(textBox1.Text != "")
            {
                isAmountSelected = true;
                amountMails = long.Parse(textBox1.Text);
            }
            else
            {
                isAmountSelected = false;
            }
        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            validState = ValidState.VALID;
            isValidSelected = true;
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            validState = ValidState.INVALID;
            isValidSelected = true;
        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            validState = ValidState.ANY;
            isValidSelected = true;
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                isNewsSelected = true;
                checkBox4.Checked = false;
                checkBox3.Checked = false;
                comboBox1.DroppedDown = false;
                newsState = NewsState.ALL;
            }
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                isNewsSelected = true;
                checkBox2.Checked = false;
                checkBox4.Checked = false;
                comboBox1.Enabled = false;
                newsState = NewsState.NOTHING;
            }
            else
            {
                comboBox1.Enabled = true;
            }
        }

        private void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                isNewsSelected = true;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                comboBox1.DroppedDown = true;
                newsState = NewsState.NOT_ENTERED;
            }
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            ignoreDate = false;
            isDateSelected = true;
            selectedDate = dateTimePicker1.Value;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(ignoreDate && isAmountSelected || isDateSelected && isValidSelected && isAmountSelected && isNewsSelected)
            {
                saveFileDialog1.CheckFileExists = false;
                saveFileDialog1.CheckPathExists = true;
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    path.Text = saveFileDialog1.FileName;
                    filePath = saveFileDialog1.FileName;
                    saveFileDialog1.FileName = "";
                    ExportData();
                }
            }

            else
            {
                MessageBox.Show("НЕ ВСЕ ПОЛЯ ЗАПОЛНЕНЫ!\n" + "ПОЖАЛУЙСТА ПРОВЕРЬТЕ ПРАВИЛЬОСТЬ ВВЕДЕННЫХ ДАННЫХ", 
                                "Ошибка при вводе данных", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Warning);
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isNewsSelected = true;
            newsName = comboBox1.Text;
        }

        private void LoadMails()
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string sql = "export_without_date";
            var command = new NpgsqlCommand(sql, conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new NpgsqlParameter("amount_rows", amountMails));
            var reader = command.ExecuteReader();
            List<string> forWrite = new List<string>();
            while(reader.Read())
            {
                forWrite.Add((string)reader[0]); 
            }
            if (forWrite.Count < amountMails)
            {
                var result = MessageBox.Show("Запрашиваемое количество адресов " + amountMails + "\nбольше, чем есть в базе данных. " +
                                             "\nХотите экспортировать " + forWrite.Count + " адресов?",
                                             "Выбрано слишком большое количество адресов",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (FileWorker.WriteToFile(filePath, forWrite.ToArray()))
                    {
                        MessageBox.Show("Данные успешно экспортированы!",
                                        "Успешно",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);                    }
                }
            }
            else if (FileWorker.WriteToFile(filePath, forWrite.ToArray()))
            {
                MessageBox.Show("Данные успешно экспортированы!",
                                        "Успешно",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
            }
            conn.Close();
        }

        private void LoadMails(DateTime date, ValidState state, long amount)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string sql = "export_rows";
            var command = new NpgsqlCommand(sql, conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new NpgsqlParameter("low_date", (NpgsqlTypes.NpgsqlDate)date));
            command.Parameters.Add(new NpgsqlParameter("valid_state", (int)state));
            command.Parameters.Add(new NpgsqlParameter("amount_rows", amount));
            var reader = command.ExecuteReader();
            List<string> forWrite = new List<string>();
            while (reader.Read())
            {
                forWrite.Add((string)reader[0]);
            }
            if(forWrite.Count < amount)
            {
                var result = MessageBox.Show("Запрашиваемое количество адресов " + amountMails + "\nбольше, чем есть в базе данных. " +
                                             "\nХотите экспортировать " + forWrite.Count + " адресов?",
                                             "Выбрано слишком большое количество адресов",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (FileWorker.WriteToFile(filePath, forWrite.ToArray()))
                    {
                        MessageBox.Show("Данные успешно экспортированы!", 
                                        "Успешно",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                    }
                }
            }
            else if (FileWorker.WriteToFile(filePath, forWrite.ToArray()))
            {
                MessageBox.Show("Данные успешно экспортированы!",
                                        "Успешно",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
            }
            conn.Close();
        }

        private void LoadMails(DateTime date, ValidState state, long amount, string newsName)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string sql = "export_rows";
            var command = new NpgsqlCommand(sql, conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new NpgsqlParameter("low_date", (NpgsqlTypes.NpgsqlDate)date));
            command.Parameters.Add(new NpgsqlParameter("valid_state", (int)state));
            command.Parameters.Add(new NpgsqlParameter("amount_rows", amount));
            command.Parameters.Add(new NpgsqlParameter("name_news", newsName));
            var reader = command.ExecuteReader();
            List<string> forWrite = new List<string>();
            while (reader.Read())
            {
                forWrite.Add((string)reader[0]);
            }
            if (forWrite.Count < amount)
            {
                var result = MessageBox.Show("Запрашиваемое количество адресов " + amountMails + "\nбольше, чем есть в базе данных. " +
                                             "\nХотите экспортировать " + forWrite.Count + " адресов?",
                                             "Выбрано слишком большое количество адресов",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (FileWorker.WriteToFile(filePath, forWrite.ToArray()))
                    {
                        MessageBox.Show("Данные успешно экспортированы!",
                                        "Успешно",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                    }
                }
            }
            else if (FileWorker.WriteToFile(filePath, forWrite.ToArray()))
            {
                MessageBox.Show("Данные успешно экспортированы!",
                                        "Успешно",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
            }
            conn.Close();
        }

        private void ExportData()
        {
            if (ignoreDate && isAmountSelected)
            {
                LoadMails();
            }
            
            else if (isDateSelected && isValidSelected && isAmountSelected && isNewsSelected)
            {
                switch (newsState)
                {
                    case NewsState.ALL:
                        LoadMails(selectedDate, validState, amountMails);
                        break;
                    case NewsState.NOT_ENTERED:
                        LoadMails(selectedDate, validState, amountMails, newsName);
                        break;
                }
            }
        }
        private void НазадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ВыйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void СвязьСРазработчикомToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Communication().Show();
        }
    }
}
