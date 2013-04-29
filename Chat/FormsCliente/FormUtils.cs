using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Chat
{
    public class FormUtils
    {

        public static void AjustarTamanoColumnas(ListView listView)
        {
            for (int i = 0; i < listView.Columns.Count - 1; i++) 
                listView.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);
            listView.Columns[listView.Columns.Count - 1].Width = -2;
        }

        public static bool TxtBoxTieneDatos(TextBox textBox)
        {
            return textBox.Text != null && !textBox.Text.Trim().Equals("");
        }

        public static bool HayFilaElegida(ListView listView) 
        {
            return listView.SelectedItems.Count != 0;
        }

        public static string ObtenerIPLocal()
        {
            IPHostEntry host;
            string ipLocal = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipLocal = ip.ToString();
                }
            }
            return ipLocal;
        }

    }
}
