using SGBFormAplication;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace SGBAplication
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CultureInfo.CurrentCulture = new CultureInfo("ru-RU")
            {
                NumberFormat = new CultureInfo("en-GB").NumberFormat
            };
            Application.Run(new Form1());
        }
    }
}
