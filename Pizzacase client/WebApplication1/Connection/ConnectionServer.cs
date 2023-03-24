using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Pizzacase_client.Connection
{
    public class ConnectionTCPServer
    {
        private static readonly ConnectionTCPServer instance = new ConnectionTCPServer();

        private ConnectionTCPServer() { }

        public static ConnectionTCPServer GetInstance () 
        {
            return instance;
        }
        public string SendMessage(object order) {
            using (var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                IPAddress serverIP = IPAddress.Parse("127.0.0.1");
                IPEndPoint serverEndPoint = new IPEndPoint(serverIP, 80);
                client.Connect(serverEndPoint);

                //serialize object to JSON-formatted string
                string message = JsonConvert.SerializeObject(order);

                var messageBytes = Encoding.ASCII.GetBytes(message);
                client.Send(messageBytes);

                var buffer = new byte[1024];
                var bytesRead = client.Receive(buffer);
                var response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received response: {0}", response);

                client.Shutdown(SocketShutdown.Both);
                client.Close();

                return response;
            }
        }
    }

    public class ConnectionUDPServer
    {
        private static readonly ConnectionUDPServer instance = new ConnectionUDPServer();

        private Socket client;

        private ConnectionUDPServer()
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        public static ConnectionUDPServer GetInstance()
        {
            return instance;
        }

        public string SendMessage(object order)
        {
            IPAddress serverIP = IPAddress.Parse("127.0.0.1");
            IPEndPoint serverEndPoint = new IPEndPoint(serverIP, 80);

            //serialize object to JSON-formatted string
            string message = JsonConvert.SerializeObject(order);

            var messageBytes = Encoding.ASCII.GetBytes(message);
            client.SendTo(messageBytes, serverEndPoint);

            var buffer = new byte[1024];
            EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            var bytesRead = client.ReceiveFrom(buffer, ref remoteEP);
            var response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Received response: {0}", response);

            return response;
        }

        ~ConnectionUDPServer()
        {
            client.Close();
        }
    }



}
