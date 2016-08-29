using System;
using System.Windows.Forms;

namespace printer
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadExit += Application_ThreadExit;
            Application.ApplicationExit += Application_ApplicationExit;
            Application.Run(new Form1());
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private static void Application_ThreadExit(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}