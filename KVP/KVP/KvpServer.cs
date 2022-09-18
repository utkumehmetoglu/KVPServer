using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using KVP.KVP;
using NLog;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Kvp;
using System.Collections.Concurrent;

namespace KVP.KVP
{
    class KvpServer
    {
        TcpListener server;
        private static string IP = GlobalConstants.ListenIp;
        private static int ListenPort = GlobalConstants.KvpPort;
        private ConcurrentQueue<KvpMessage> MessagesQueue = new ConcurrentQueue<KvpMessage>();
        public CancellationToken CancellationToken = new CancellationToken();
        Mutex MessagesMutex = new Mutex(false,"MessagesMutex");

        public KvpServer()
        {
            IPAddress localAddr = IPAddress.Parse(IP);
            server = new TcpListener(localAddr,ListenPort);
            server.Start();
            //Task Listen = new Task(() => StartListener(IP, ListenPort));
            Task.Factory.StartNew(() => StartListener(IP,ListenPort));
        }
        public void StartListener(String ip, int port)
        {
            try
            {

                while (true)
                {

                    if (CancellationToken.IsCancellationRequested)
                        return; // Kill the thread

                    TcpClient client = server.AcceptTcpClient();
                    if (client.Connected)
                    {


                        Task.Factory.StartNew(async () => HandleClient(client));  


                    }

                    Thread.Sleep(100);

                }
            }
            catch (SocketException e)
            {
                throw new KvpSocketException(e.Message);
                server.Stop();
            }
        }

        public void HandleClient(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            var stream = client.GetStream();
            KvpExtractor priormessage = new KvpExtractor("");
            int i;

            byte[] buffer = new byte[1024];
            while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                String data = Encoding.ASCII.GetString(buffer, 0, i);
                KvpExtractor kvpExtractor = new KvpExtractor(data);

                Console.WriteLine("Received:" + data + " Thread ID: " + Thread.CurrentThread.ManagedThreadId);

                if (priormessage.HasHead() && kvpExtractor.HasTail())
                    foreach (KvpMessage km in kvpExtractor.ExtractMessages(priormessage.getHead() + kvpExtractor.getTail()))
                    {
                        MessagesMutex.WaitOne();
                        MessagesQueue.Enqueue(km); 
                        MessagesMutex.ReleaseMutex();

                    }

                List<KvpMessage> messages = kvpExtractor.ExtractMessages(data);


                foreach (KvpMessage km in messages)
                {

                    MessagesMutex.WaitOne();
                    MessagesQueue.Enqueue(km);
                    MessagesMutex.ReleaseMutex();

                }



                priormessage = kvpExtractor;

            }
        }

        public KvpMessage DequeueOne()
        {
            KvpMessage RetVal;
            while(MessagesQueue.TryDequeue(out RetVal)) { } // Try Dequeueing message
            if (RetVal == null)
                return new KvpMessage(""); // Return an empty message to avoid any null values
            return RetVal;
        }


    }
}
