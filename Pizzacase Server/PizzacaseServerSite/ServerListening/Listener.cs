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
        public static Order order = new Order();
        public void StartTcpServer() {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress serverIP = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(serverIP, 8080);
            listener.Bind(localEndPoint);
            listener.Listen(10);

            while (true)
            {
                Console.WriteLine("waiting for a connection....");
                Socket handler = listener.Accept();

                Task.Run(() =>
                {
                    try
                    {
                        byte[] buffer = new byte[1024];
                        var bytesReceived = handler.Receive(buffer);
                        string message = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                        Console.WriteLine("Received message: {0}", message);
                        order = JsonConvert.DeserializeObject<Order>(message);
                        Console.WriteLine("Order ID: {0}", order.OrderId);
                        Console.WriteLine("Order Name: {0}", order.Datum);
                        if(order.CustomerName != null)
                        {
                            IActionResult SaveOrder = new OrderRepository().AddOrder(order);
                        }

                        string response = "Thank you for your order DumbAss!";
                        byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                        handler.Send(responseBytes);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: {0}", ex.Message);
                    }
                    finally
                    {
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                });

            }
        }
    }
    public class ListenerUDP
    {

        public void StartUdpServer()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress serverIp = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(serverIp, 80);
            listener.Bind(localEndPoint);
            listener.Listen(10);

            byte[] buffer = new byte[1024];

            while (true)
            {
                Console.WriteLine("waiting for a connection....");
                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                int bytesReceived = listener.ReceiveFrom(buffer, ref remoteEndPoint);
                string message = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                Console.WriteLine("Received message: {0}", message);
               

                string response = "Thank you for your order DumbAss!";
                byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                listener.SendTo(responseBytes, remoteEndPoint);
            }
        }
    }
}
