using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Loading;

namespace Order1.Forms
{
    public partial class LoadingProcessForm : Form, ILoadingForm
    {
        public int Step { get; set; }
        public string[] Messages { get; set; }

        private LoadingProcessForm instance = null;

        public LoadingProcessForm()
        {
            InitializeComponent();
        }

        private void LoadingProcessForm_Load(object sender, EventArgs e)
        {

        }

        public LoadingProcessForm GetInstance()
        {
            if (instance == null)
                instance = this;
            return instance;
        }

        public void AddMessage(string message)
        {
            textBox1.Text += message;
        }

        public void ChangeProcessName(string processName)
        {
            processTextBox.Text = processName;
        }

        public void MoveHendler()
        {

        }
    }
}
