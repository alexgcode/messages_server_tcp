using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace message_server
{
    //A Socket is a point-to-point connection: one server, one client.
    //To support one server, multiple clients, you need multiple Sockets: one per client.
    class Program
    {
        private static List<Worker> workers = new List<Worker>();
        private static string receiveMessage = string.Empty;

        /*
        static void Receive()
        {
            while (true)
            {
                Thread.Sleep(50);
                byte[] buffer = new byte[300];
                int receive = acceptSocket.Receive(buffer, 0, buffer.Length, 0);
                Array.Resize(ref buffer, receive);
                if(receive > 0)
                {
                    Console.WriteLine(Encoding.Default.GetString(buffer));
                    receiveMessage = Encoding.UTF8.GetString(buffer);
                    byte[] SendDataToClient = Encoding.UTF8.GetBytes(" <server>: " + receiveMessage);
                    acceptSocket.Send(SendDataToClient, 0, SendDataToClient.Length, 0);
                    Console.WriteLine("Mensaje enviado: " + " <server>: " + receiveMessage);
                    receiveMessage = string.Empty;
                }
            }
        }
        */

        static void UpdateListas()
        {
            foreach(Worker wok in workers)
            {
                wok.woks = workers;
            }
        }

        static void Main(string[] args)
        {
            string hostName = Dns.GetHostName();
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            //server configuration
            int port = 1234;
            //string ipServer = "127.0.0.1";
            //Thread sender = new Thread(SenderThread);
            //Thread thread = new Thread(Receive);

            IPAddress ip = IPAddress.Parse(myIP);

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(ip, port));
            listener.Listen(100);    //nro de conexiones pendientes
            Console.WriteLine("Server started at IP {0}. Listening on port {1}",ip.ToString(), port);
            //----------------------
            string message = string.Empty;

            while (true)
            {
                Socket newCliente = listener.Accept();
                Console.WriteLine($"Conexion aceptada, aviability: {newCliente.Available}");

                Worker worker = new Worker(newCliente);
                workers.Add(worker);
                UpdateListas();
                worker.Start();
            }
            
            //acceptSocket = listener.Accept();
            //Console.WriteLine($"Conexion aceptada, aviability: {acceptSocket.Available}");

            //thread.Start();

        }
    }
}
