﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClientImplementation;
using uy.edu.ort.obligatorio.Commons;
using FormsCliente;
using System.Threading;

namespace Chat
{
    public enum SearchStatus { NEW, WAITING_SERVERS, ALL_SERVES, WAITING_RESULTS, ALL_RESULTS, NO_SERVERS, NO_RESULTS }
    public partial class BuscarArchivo : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<string, ServerInfo> serversToSearch = new Dictionary<string, ServerInfo>();

        private int serversToProccess = 0;

        private Dictionary<string, MultiplePayloadFrameDecoded[]> resultsByServer = new Dictionary<string, MultiplePayloadFrameDecoded[]>();
        private List<FileObject> searchResult = new List<FileObject>();


        private string HashQuery { get; set; }
        private SearchStatus searchStatus;

        private string pattern = "";

        private ClientHandler.ServerListReceivedDelegate serverListReceivedDelegate;
        private ClientHandler.SearchFilesReceivedDelegate searchFilesReceivedDelegate;
        private ListViewColumnSorter lvwColumnSorter;

        private void ClearResults()
        {
            GenerateHashQuery("INIT");
            serversToSearch.Clear();
            searchResult.Clear();
            resultsByServer.Clear();
            serversToProccess = 0;
            listaArchivos.Items.Clear();

          
        }

        private void RefreshScreen()
        {
        }

        private string GenerateHashQuery(string pattern)
        {
            HashQuery = StringUtils.CalculateMD5Hash(String.Format("{0}|{1}|{2}", ClientHandler.GetInstance().Login, pattern, "" + DateTime.Now));
            return HashQuery;
        }

        public BuscarArchivo()
        {
            InitializeComponent();
            GenerateHashQuery("INIT");
            searchStatus = SearchStatus.NEW;

            serverListReceivedDelegate = new ClientHandler.ServerListReceivedDelegate(EventServerListReceivedResponse);
            ClientHandler.GetInstance().ServerListReceivedEvent += serverListReceivedDelegate;

            searchFilesReceivedDelegate = new ClientHandler.SearchFilesReceivedDelegate(OnSearchFilesResultReceived);
            ClientHandler.GetInstance().SearchFilesReceivedEvent += searchFilesReceivedDelegate;
            lvwColumnSorter = new ListViewColumnSorter();
            listaArchivos.ListViewItemSorter = lvwColumnSorter;
        }

        private void OnSearchFilesResultReceived(object sender, SearchFilesEventArgs arg)
        {
            this.BeginInvoke((Action)(delegate
            {
                log.DebugFormat("OnSearchFilesResultReceived arg: {0}", arg.Response);

                MultiplePayloadFrameDecoded payload = arg.Response;
                //login + ARROBA_SEPARATOR + queryHash + ARROBA_SEPARATOR + Settings.GetInstance().GetProperty("server.name", "DEFAULT_SERVER");
                string[] destination = payload.Destination.Split('@');
                string responseHashQuery = destination[1];
                string serverName = destination[2];

                if (HashQuery.Equals(responseHashQuery)) //si es para la consulta actual, sino se descarta
                {
                    log.DebugFormat("LLegó un payload deresultado de busqueda del server: {0}, con datos{1}",serverName, payload.ToString());
                    if (payload.IsError)
                    {
                        log.Error(payload.Payload);
                        ClearResults();
                        MessageBox.Show(payload.Payload);
                        btnBuscar.Enabled = true;
                    }
                    else
                    {

                      
                        lock (resultsByServer)
                        {
                            if (resultsByServer[serverName] == null)
                            {
                                resultsByServer[serverName] = new MultiplePayloadFrameDecoded[payload.PartsTotal];
                            }
                        }

                        resultsByServer[serverName][payload.PartsCurrent-1] = payload;
                        if (payload.IsLastpart())
                        {
                            lock (this)
                            {
                                serversToProccess--;
                            }
                            StringBuilder filesSB = new StringBuilder();
                            lock (resultsByServer[serverName])
                            {
                                foreach (var item in resultsByServer[serverName])
                                {
                                    filesSB.Append(item.Payload);
                                }
                            }
                            string[] infoArr = filesSB.ToString().Split('|');
                            foreach (var fileStr in infoArr)
                            {
                                if (fileStr.Length > 0)//si tiene resultados
                                {
                                    FileObject file = FileObject.FromNetworkString(fileStr);
                                    file.Server = serverName;

                                    lock (searchResult)
                                    {
                                        searchResult.Add(file);
                                    }
                                }
                            }


                            lock (this)
                            {
                                if (serversToProccess < 1)
                                {
                                    log.DebugFormat("Estan todos los resultados de busquedas");
                                    ProcesarResults();
                                }
                            }
                           

                        }
                        else
                        {
                            log.DebugFormat("No terminaron de llegar los resultados, set timeout de busqueda");
                        }
                    }
                }
                else
                {
                    log.DebugFormat("Descarto un payload de resultado de busqueda: {0}", payload.ToString());
                }

            }));

        }

        private void ProcesarResults()
        {
            bool resultsWereFound = searchResult.Count>0;
            listaArchivos.Items.Clear();
            foreach (var file in searchResult)
            {
                    ListViewItem lvi = new ListViewItem(file.Name);
                    lvi.Tag = file;
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, ""+file.Size));
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, file.Server));
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, file.Owner));
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, file.Hash));
                    listaArchivos.Items.Add(lvi);
            }
            FormUtils.AjustarTamanoColumnas(listaArchivos);
            btnBuscar.Enabled = true;
            btnDescargar.Enabled = resultsWereFound;
        }

        private void EventServerListReceivedResponse(object sender, GetServersEventArgs arg)
        {
            this.BeginInvoke((Action)(delegate
            {
                MultiplePayloadFrameDecoded payload = arg.Response;

                string[] destination = payload.Destination.Split('@');
                string responseHashQuery = destination[1];
                if (HashQuery.Equals(responseHashQuery)) //si es para la consulta actual, sino se descarta
                {
                    log.DebugFormat("LLegó un payload de busqueda de servidores: {0}", payload.ToString());
                    if (payload.IsError)
                    {
                        log.Error(payload.Payload);
                        ClearResults();
                        MessageBox.Show(payload.Payload);
                    }
                    else
                    {

                        string[] infoArr = payload.Payload.Split('|');
                        foreach (var serverStr in infoArr)
                        {
                            ServerInfo serverInfo = ServerInfo.Parse(serverStr);
                            lock (serversToSearch)
                            {
                                serversToSearch.Add(serverInfo.Name, serverInfo);
                            }
                        }

                        if (payload.IsLastpart())
                        {
                            lock (serversToSearch)
                            {
                                foreach (var serverName in serversToSearch.Keys)
                                {
                                    resultsByServer.Add(serverName, null);
                                }
                                serversToProccess = resultsByServer.Count;
                                foreach (var serverName in serversToSearch.Keys)
                                {
                                    ClientHandler.GetInstance().REQSearchFiles(serversToSearch[serverName], this.pattern, HashQuery);
                                }
                            }

                        }
                        else
                        {
                            log.DebugFormat("No terminaron de llegar los servers, set timeout de busqueda");
                        }
                    }
                }
                else
                {
                    log.DebugFormat("Descarto un payload de busqueda de servidores: {0}", payload.ToString());
                }


            }));
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string pattern = txtBuscarArchivo.Text;
            if (pattern != null && !pattern.Trim().Equals(""))
            {
                try
                {
                    btnBuscar.Enabled = false;
                    ClearResults();
                    this.pattern = pattern;
                    ClientHandler.GetInstance().REQGetServerList(GenerateHashQuery(pattern));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Dispose();
            ClientHandler.GetInstance().ServerListReceivedEvent -= serverListReceivedDelegate;
        }

        private void btnDescargar_Click(object sender, EventArgs e)
        {
            if (FormUtils.HayFilaElegida(listaArchivos))
            {
                FileObject fileSelected = (FileObject)listaArchivos.SelectedItems[0].Tag;
                if (fileSelected != null)
                {
                    ServerInfo serverInfo = serversToSearch[fileSelected.Server];
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Title = "Descargar Archivo";
                    sfd.FileName = fileSelected.Name;
                    sfd.OverwritePrompt = false;

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        FileDownloader fd = new FileDownloader()
                        {
                            Destination = sfd.FileName,
                            FileSelected = fileSelected,
                            ServerInfo = serverInfo
                        };

                        //muestro la ventana del progress bar
                        DownloadProgress dp = new DownloadProgress(fd);
                        dp.Show();

                        //inicio la descarga en otro sred
                        fd.DownloadThread();
                    }
                }
            }
        }

        private void BuscarArchivo_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClientHandler.GetInstance().ServerListReceivedEvent -= serverListReceivedDelegate;
            ClientHandler.GetInstance().SearchFilesReceivedEvent -= searchFilesReceivedDelegate;
        }


        


        private void listaArchivos_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (((ColumnClickEventArgs)e).Column == lvwColumnSorter.SortColumn)
            {
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                lvwColumnSorter.SortColumn = ((ColumnClickEventArgs)e).Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }
            listaArchivos.Sort();
        }

    }
}
