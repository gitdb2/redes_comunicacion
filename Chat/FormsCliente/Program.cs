using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Chat
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Login login = new Login();
            DialogResult res = login.ShowDialog();
            if (res == DialogResult.OK)
            {
                Application.Run(new VentanaPrincipalCliente());
            }
            else
            {
                Application.Exit();
            }
        }
    }
}
