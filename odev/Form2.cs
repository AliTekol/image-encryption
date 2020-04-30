using System;
using System.Windows.Forms;

namespace odev
{
    public partial class Form2 : Form
    {
        //FORM1'DEN ALINAN DEĞERİN FORM2 RICHTEXTBOX BİLEŞENİNDE GÖSTERİLMESİ
        public Form2(int[] val)
        {          
            InitializeComponent();
            for (int i = 0; i < 256; i++)
            {
                string hex = val[i].ToString("X");
                richTextBox1.AppendText(Environment.NewLine + hex);
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        //FORM2 UYGULAMADAN ÇIKIŞ
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
