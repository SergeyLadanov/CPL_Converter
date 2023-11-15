using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPL_Converter
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>


        // [DllImport("kernel32.dll")]
        // public static extern bool FreeConsole();


        // static Program()
        // {
        //    FreeConsole();
        // }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
