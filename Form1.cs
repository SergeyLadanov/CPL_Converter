using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CPL_Converter
{
    public partial class Form1 : Form
    {
        private String filename = "";
        private bool StartHide = false;


        //Функция выделения директории из пути
        private String GetFileDirectory(String Path)
        {
            String[] Tmp = Path.Split('\\');
            Path = Path.Replace(Tmp[Tmp.Length - 1], "");
            return Path;
        }

        // Функция выделения имени файла из пути
        private String GetFileName(String Path)
        {
            String[] Tmp = Path.Split('\\');
            Path = Tmp[Tmp.Length - 1];
            return Path;
        }

        // Функция генерации нового имени файла
        private String GetNewFileName(String Path)
        {
            String Dir = GetFileDirectory(Path);
            String Fil = GetFileName(Path);
            Fil = Fil.Split('.')[0];
            Fil = Fil + "_new.xlsx";
            Path = Dir + Fil;
            return Path;
        }
        public Form1()
        {
            // Проверка есть ли аргументы приложения
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                filename = Environment.GetCommandLineArgs()[1];
                StartHide = true;
            }
            
            InitializeComponent();
        }
        // Обработка нажатия кнопки
        [SupportedOSPlatform("windows")]
        private void OpenButton_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                int RowCount = 0;
                CplConverter conv = new CplConverter(GetNewFileName(openFileDialog1.FileName));

                labelStatus.Text = "Обработка...";
                RowCount = conv.HandleCPL(openFileDialog1.FileName);
                labelRowCount.Text = RowCount.ToString();
                labelStatus.Text = "Выполнено";
            }

                //CplConverter conv = new CplConverter("D:\\Test.xlsx");

                //conv.HandleCPL("D:\\WorkSpace\\FrequencyRegulator\\freqreq_control\\Pick Place\\CPL.txt");
        }

        [SupportedOSPlatform("windows")]
        private void Form1_Load(object sender, EventArgs e)
        {
            // Не показывать форму при запуске
            if (StartHide)
            {
                int RowCount = 0;
                this.Visible = false;
                this.ShowInTaskbar = false;

                CplConverter conv = new CplConverter(GetNewFileName(filename));
                conv.AutoSaveEnable();
                RowCount = conv.HandleCPL(filename);

                MessageBox.Show("Файл успешно конвертирован и помещен в каталог с исходным файлом.\r\n" + 
                    "Количество обработанных строк: " + RowCount.ToString());
                this.Close();
            }
        }
    }
}
