using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace ServerLoginLibrary
{

    public class ServerLoginAPM : ServerLogin
    {
        public delegate void TransmissionDataDelegate(NetworkStream stream);
        public ServerLoginAPM(IPAddress IP, int port) : base(IP, port)
        {
        }
        protected override void AcceptClient()
        {
            while (true)
            {
                TcpClient tcpClient = TcpListener.AcceptTcpClient();
                Stream = tcpClient.GetStream();
                TransmissionDataDelegate transmissionDelegate = new TransmissionDataDelegate(BeginDataTransmission);
                //callback style
                transmissionDelegate.BeginInvoke(Stream, TransmissionCallback, tcpClient);
                // async result style
                //IAsyncResult result = transmissionDelegate.BeginInvoke(Stream, null, null);
                ////operacje......
                //while (!result.IsCompleted) ;
                ////sprzątanie
            }
        }
        private void TransmissionCallback(IAsyncResult ar)
        {
            // sprzątanie
        }
        /// <summary>
        /// Sprawdzanie czy login jest jednym z możliwych i odsyłanie odpowiedniego komunikatu
        /// </summary>
        /// <param name="stream"></param>
        protected override void BeginDataTransmission(NetworkStream stream)
        {
            byte[] buffer = new byte[Buffer_size];
            string[] logins = new string[] { "Admin", "User1", "User2", "User3", "User4", "User5", "User6" };
            stream.Write(Encoding.Unicode.GetBytes("Wprowadz login: "+ Environment.NewLine), 0, Encoding.Unicode.GetBytes("Wprowadz login: " + Environment.NewLine).Length);
            while (true)
            {
                try
                {
                    int message_size = stream.Read(buffer, 0, Buffer_size);
                    int i = buffer.Length - 1;

                    while (buffer[i] == 0)
                    {
                        --i;
                    }
                    byte[] nazwa_uzytkownika_bez_0 = new byte[i + 1];
                    Array.Copy(buffer, nazwa_uzytkownika_bez_0, i + 1);
                    bool login_good = false;
                    for (int j = 0; j < logins.Length; j++)
                    {
                        if (Encoding.UTF8.GetString(nazwa_uzytkownika_bez_0) == logins[j])
                        {
                            login_good = true;
                        }
                    }
                    if (login_good==true)
                    {
                        stream.Write(Encoding.Unicode.GetBytes("Login poprawny, Witaj " + Encoding.ASCII.GetString(nazwa_uzytkownika_bez_0)+" "+Environment.NewLine), 0, Encoding.Unicode.GetBytes("Login poprawny, Witaj " + Encoding.ASCII.GetString(nazwa_uzytkownika_bez_0) +" "+ Environment.NewLine).Length);
                        message_size = stream.Read(buffer, 0, Buffer_size);
                    }
                    else
                    {
                        stream.Write(Encoding.Unicode.GetBytes("Niepoprawny login, sproboj ponownie "+ Environment.NewLine), 0, Encoding.Unicode.GetBytes("Niepoprawny login, sproboj ponownie " + Environment.NewLine).Length);
                         message_size = stream.Read(buffer, 0, Buffer_size);
                    }

                }
                catch (IOException e)
                {
                    break;
                }
            }
        }
        public override void Start()
        {
            StartListening();
            //transmission starts within the accept function
            AcceptClient();
        }
    }
}
