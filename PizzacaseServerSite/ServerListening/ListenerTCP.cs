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
        private readonly object lockObject = new object();

        static int port = 8080;
        static string ipAddress = "127.0.0.1";

        private TcpListener tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
        private volatile bool isRunning;

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
            if (!isRunning)
            {
                isRunning = true;
                tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
                tcpListener.Start();
                Console.WriteLine("TCP Server listening on " + ipAddress + ":" + port);

                try
                {
                    while (isRunning)
                    {
                        try
                        {
                            if (tcpListener.Pending())
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
                            else
                            {
                                // No pending connection; sleep briefly to reduce CPU usage.
                                System.Threading.Thread.Sleep(100);
                            }
                        }
                        catch (SocketException ex)
                        {
                            // Check if the exception is due to the stopping of the listener
                            if (ex.SocketErrorCode == SocketError.Interrupted)
                            {
                                Console.WriteLine("TCP Server stopping...");
                                break;
                            }
                            else
                            {
                                throw; // Re-throw the exception if it's not the expected one
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
                finally
                {
                    tcpListener.Stop();
                    Console.WriteLine("TCP Server stopped.");
                }
            }
        }

        public void StopTcpServer()
        {
            Console.WriteLine("Attempting to stop TCP Server...");
            lock (lockObject)
            {
                if (isRunning)
                {
                    isRunning = false;
                    tcpListener.Stop();
                    Console.WriteLine("TCP Server stopped.");
                }
                else
                {
                    Console.WriteLine("Stop request ignored: TCP Server was already stopped.");
                }
            }
        }
    }
}
