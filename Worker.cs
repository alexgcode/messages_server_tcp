﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace message_server
{
    class Worker
    {
        //public event MessageEventHandler MessageReceived;
        public event EventHandler Disconnected;
        private readonly Socket socket;
        private string number;
        //private readonly Stream stream;
        public List<Worker> woks = new List<Worker>();

        public string Username { get; private set; } = null;

        public Worker(Socket socket)
        {
            this.socket = socket;
        }

        public void Send(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            socket.Send(buffer, 0, buffer.Length,0);
        }

        private void Run()
        {
            byte[] buffer = new byte[500000];
            try
            {
                while (true)
                {
                    int receivedBytes = socket.Receive(buffer, 0, buffer.Length, 0);//sock.Read(buffer, 0, buffer.Length);
                    if (receivedBytes < 1)
                        break;

                    //Array.Resize(ref buffer, receivedBytes);
                    string message = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                    Message msg = JsonConvert.DeserializeObject<Message>(message); 
                    
                    if(string.IsNullOrEmpty(this.number))
                    {
                        this.number = msg.senderNumber;
                    }

                    if(msg.type == 2)
                    {
                        Console.WriteLine(message);
                        SendFileTo(msg.targetNumber, msg);
                    }
                    else
                    {
                        Console.WriteLine(message);
                        SendMessageTo(msg.targetNumber, msg);
                    }

                    
                    //BroadcastMessage(message);
                }
            }
            catch (IOException) { }
            catch (ObjectDisposedException) { }
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        public void Start()
        {
            new Thread(Run).Start();
        }
        public void Close()
        {
            socket.Close();
        }

        void BroadcastMessage(string message)
        {
            byte[] SendDataToClient = Encoding.UTF8.GetBytes(message);
            foreach(Worker wok in woks)
            {
                if(wok != this)
                {
                    wok.socket.Send(SendDataToClient, 0, SendDataToClient.Length, 0);
                }
            }
        }

        void SendMessageTo(string targetMessage, Message msg)
        {
            string jsonString = JsonConvert.SerializeObject(msg);

            byte[] data = Encoding.UTF8.GetBytes(jsonString);
            foreach (Worker wok in woks)
            {
                if (wok != this && wok.number == targetMessage)
                {
                    wok.socket.Send(data, 0, data.Length, 0);
                }
            }
        }

        void SendFileTo(string targetMessage, Message msg)
        {
            string jsonString = JsonConvert.SerializeObject(msg);

            byte[] data = Encoding.UTF8.GetBytes(jsonString);
            foreach (Worker wok in woks)
            {
                if (wok != this && wok.number == targetMessage)
                {
                    wok.socket.Send(data, 0, data.Length, 0);
                }
            }
        }
    }
}
