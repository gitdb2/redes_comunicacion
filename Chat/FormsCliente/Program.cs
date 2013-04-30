using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Chat
{
    static class Program
    {
        private static log4net.ILog log;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
            log4net.GlobalContext.Properties["serverName"] = "Cliente";
            log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            
            log.Info("Inicio del Cliente");

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
