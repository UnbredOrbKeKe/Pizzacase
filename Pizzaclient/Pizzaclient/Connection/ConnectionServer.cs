using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Pizzacase_client.Connection
{
    public class ConnectionServer
    {
        private static readonly ConnectionServer instance = new ConnectionServer();

        private ConnectionServer() { }

        public static ConnectionServer GetInstance () 
        {
            return instance;
        }
        public string SendMessage(string message) {
            using (var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                IPAddress serverIP = IPAddress.Parse("127.0.0.1");
                IPEndPoint serverEndPoint = new IPEndPoint(serverIP, 8080);
                client.Connect(serverEndPoint);

                var messageBytes = Encoding.ASCII.GetBytes(message);
                Console.WriteLine("Send message: {0}", messageBytes);
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
}
