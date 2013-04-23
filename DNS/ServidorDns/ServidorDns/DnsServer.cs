using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace uy.edu.ort.obligatorio.ServidorDns
{
    public class DnsServer
    {

        public TcpClient client;
        public NetworkStream netStream;
        public BinaryReader br;
        public BinaryWriter bw;
        UserInfo userInfo;

        public DnsServer(TcpClient c)
        {
          
            client = c;

            // Handle client in another thread.
            (new Thread(new ThreadStart(SetupConn))).Start();
        }



        void SetupConn()  // Setup connection and login or register.
        {
            try
            {
                Console.WriteLine("[{0}] New connection!", DateTime.Now);
                netStream = client.GetStream();
            


                br = new BinaryReader(netStream, Encoding.UTF8);
                bw = new BinaryWriter(netStream, Encoding.UTF8);

                // Say "hello".
                //bw.Write(IM_Hello);
                //bw.Flush();


                string tmp1 = br.ReadString();//REQ
                string tmp2 = br.ReadString();//02
                string tmp3 = br.ReadString();//00005
                string tmp4 = br.ReadString();//HOLA!

                Console.WriteLine(tmp1 + " " + tmp2 + " " + tmp3 + " " + tmp4);
                Console.WriteLine("respondo");


                bw.Write("RES");
                bw.Write("03");
                bw.Write("00004");
                bw.Write("CHAU");
                bw.Flush();

                Console.WriteLine("termino");
                //int hello = br.ReadInt32();
                //if (hello == IM_Hello)
                //{
                //    // Hello packet is OK. Time to wait for login or register.
                //    byte logMode = br.ReadByte();
                //    string userName = br.ReadString();
                //    string password = br.ReadString();
                //    if (userName.Length < 10) // Isn't username too long?
                //    {
                //        if (password.Length < 20)  // Isn't password too long?
                //        {
                //            if (logMode == IM_Register)  // Register mode
                //            {
                //                if (!prog.users.ContainsKey(userName))  // User already exists?
                //                {
                //                    userInfo = new UserInfo(userName, password, this);
                //                    prog.users.Add(userName, userInfo);  // Add new user
                //                    bw.Write(IM_OK);
                //                    bw.Flush();
                //                    Console.WriteLine("[{0}] ({1}) Registered new user", DateTime.Now, userName);
                //                    prog.SaveUsers();
                //                    Receiver();  // Listen to client in loop.
                //                }
                //                else
                //                    bw.Write(IM_Exists);
                //            }
                //            else if (logMode == IM_Login)  // Login mode
                //            {
                //                if (prog.users.TryGetValue(userName, out userInfo))  // User exists?
                //                {
                //                    if (password == userInfo.Password)  // Is password OK?
                //                    {
                //                        // If user is logged in yet, disconnect him.
                //                        if (userInfo.LoggedIn)
                //                            userInfo.Connection.CloseConn();

                //                        userInfo.Connection = this;
                //                        bw.Write(IM_OK);
                //                        bw.Flush();
                //                        Receiver();  // Listen to client in loop.
                //                    }
                //                    else
                //                        bw.Write(IM_WrongPass);
                //                }
                //                else
                //                    bw.Write(IM_NoExists);
                //            }
                //        }
                //        else
                //            bw.Write(IM_TooPassword);
                //    }
                //    else
                //        bw.Write(IM_TooUsername);
                //}
                //CloseConn();
            }
            finally { CloseConn(); }
        }

        void CloseConn() // Close connection.
        {
            try
            {
                userInfo.LoggedIn = false;
                br.Close();
                bw.Close();
                netStream.Close();
                client.Close();
                Console.WriteLine("[{0}] End of connection!", DateTime.Now);
            }
            catch { }
        }
        //void Receiver()  // Receive all incoming packets.
        //{
        //    Console.WriteLine("[{0}] ({1}) User logged in", DateTime.Now, userInfo.UserName);
        //    userInfo.LoggedIn = true;

        //    try
        //    {
        //        while (client.Client.Connected)  // While we are connected.
        //        {
        //            byte type = br.ReadByte();  // Get incoming packet type.

        //            if (type == IM_IsAvailable)
        //            {
        //                string who = br.ReadString();

        //                bw.Write(IM_IsAvailable);
        //                bw.Write(who);

        //                UserInfo info;
        //                if (prog.users.TryGetValue(who, out info))
        //                {
        //                    if (info.LoggedIn)
        //                        bw.Write(true);   // Available
        //                    else
        //                        bw.Write(false);  // Unavailable
        //                }
        //                else
        //                    bw.Write(false);      // Unavailable
        //                bw.Flush();
        //            }
        //            else if (type == IM_Send)
        //            {
        //                string to = br.ReadString();
        //                string msg = br.ReadString();

        //                UserInfo recipient;
        //                if (prog.users.TryGetValue(to, out recipient))
        //                {
        //                    // Is recipient logged in?
        //                    if (recipient.LoggedIn)
        //                    {
        //                        // Write received packet to recipient
        //                        recipient.Connection.bw.Write(IM_Received);
        //                        recipient.Connection.bw.Write(userInfo.UserName);  // From
        //                        recipient.Connection.bw.Write(msg);
        //                        recipient.Connection.bw.Flush();
        //                        Console.WriteLine("[{0}] ({1} -> {2}) Message sent!", DateTime.Now, userInfo.UserName, recipient.UserName);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (IOException) { }

        //    userInfo.LoggedIn = false;
        //    Console.WriteLine("[{0}] ({1}) User logged out", DateTime.Now, userInfo.UserName);
        //}
    
    
    }

}
