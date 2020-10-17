using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CPL_Converter
{
    public partial class Form1 : Form
    {
        private String filename = "";
        private bool StartHide = false;
        public Form1()
        {
            // Проверка есть ли аргументы приложения
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                filename = Environment.GetCommandLineArgs()[1];
                StartHide = true;
            }
            
            InitializeComponent();
            label1.Text = filename;



        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            CplConverter conv = new CplConverter("D:\\Test.xlsx");
            
            conv.HandleCPL("D:\\WorkSpace\\FrequencyRegulator\\freqreq_control\\Pick Place\\CPL.txt");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Не показывать форму при запуске
            if (StartHide)
            {
                this.Visible = false;
                this.ShowInTaskbar = false;

                MessageBox.Show("Файл успешно конвертирован и помещен в каталог с исходным файлом.");
                this.Close();
            }
        }
    }
}
