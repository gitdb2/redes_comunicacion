using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using log4net;
using ClientImplementation;

namespace Chat
{
    static class Program
    {
        private static ILog log;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
            log4net.GlobalContext.Properties["serverName"] = "client";
            log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Info("arranca");

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
