using System.Net.Sockets;
using System.Net;
using System.Text;
using PizzacaseServerSite.Pages;
using Microsoft.AspNetCore.Mvc;
using PizzacaseServerSite.Models;
using Newtonsoft.Json;
using PizzacaseServerSite.Repository;

namespace PizzacaseServerSite.ServerListening
{
    public class ListenerTCP
    {
        //public static Order order = new Order();
        public void StartTcpServer()
        {
            int port = 8080;
            string ipAddress = "127.0.0.1";

            TcpListener tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
            tcpListener.Start();
            Console.WriteLine("Tcp Server listening on " + ipAddress + ":" + port);

            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                Console.WriteLine("Client connected from " + ((IPEndPoint)client.Client.RemoteEndPoint).Address);

                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received message: " + message);


                client.Close();
            }

        }
    }
}

   
