using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AlavaSoft.Class;

namespace AlavaSoft
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        public static fLogIn MainLogInPage = null;
        public static fAlavaSoft ofAlavaSoft = null;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //--Instantiation Section--
            MainLogInPage = new fLogIn(true);
            ofAlavaSoft = new fAlavaSoft();

            //--Run Program--
            ofAlavaSoft.WindowState = FormWindowState.Maximized;
            Application.Run(ofAlavaSoft);
        }

        
    }
}
