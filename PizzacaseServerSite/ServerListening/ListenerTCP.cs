using System.Net.Sockets;
using System.Net;
using System.Text;
using PizzacaseServerSite.Decryption;

namespace PizzacaseServerSite.ServerListening
{
    public class ListenerTCP
    {
        private static ListenerTCP _instance;
        private static readonly object _lock = new object();

        static int port = 8080;
        static string ipAddress = "127.0.0.1";

        private TcpListener tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
        private bool isRunning;

        // Private constructor to prevent external instantiation.
        private ListenerTCP() { }

        // Public static method to get the instance.
        public static ListenerTCP Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ListenerTCP();
                        }
                    }
                }
                return _instance;
            }
        }

        public void StartTcpServer()
        {
            tcpListener.Start();
            isRunning = true;
            Console.WriteLine("Tcp Server listening on " + ipAddress + ":" + port);

            while (isRunning)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                Console.WriteLine("Client connected from " + ((IPEndPoint)client.Client.RemoteEndPoint).Address);

                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                byte[] encryptedMessage = new byte[bytesRead];
                Array.Copy(buffer, encryptedMessage, bytesRead);

                string decryptedMessage = AESHelper.DecryptStringFromBytes(encryptedMessage);
                Console.WriteLine("Received message: " + decryptedMessage);

                client.Close();
            }
        }

        public void StopTcpServer()
        {
            if (isRunning)
            {
                isRunning = false;
                tcpListener.Stop();
                Console.WriteLine("TCP Server stopped.");
            }
        }
    }
}
