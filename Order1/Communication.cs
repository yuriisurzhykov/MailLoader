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
    public partial class Communication : Form
    {
        public Communication()
        {
            InitializeComponent();
        }

        private void Communication_Load(object sender, EventArgs e)
        {
            label1.Text = "Если у вас возникли какие-либо\n" +
                          "вопросы или проблемы во время\n" +
                          "использования программы просьба\n" +
                          "написать на почту\n" +
                          "surzhickov.yura37@gmail.com";
        }
    }
}
