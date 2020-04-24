using System;
using Core.Loading;
using System.Windows.Forms;

namespace Order1.Forms
{
    public partial class ImportLoadingForm : Form, ILoadingForm
    {
        private ImportLoadingForm instance = null;

        public int Step { get ; set ; }
        public string[] Messages { get; set; }

        public ImportLoadingForm()
        {
            InitializeComponent();
        }

        private void ImportLoadingBar_Load(object sender, EventArgs e)
        {
            Messages = textBox1.Lines;
        }

        public ImportLoadingForm GetInstance()
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

        public static implicit operator ImportLoadingForm(LoadingProcessForm v)
        {
            throw new NotImplementedException();
        }
    }
}
