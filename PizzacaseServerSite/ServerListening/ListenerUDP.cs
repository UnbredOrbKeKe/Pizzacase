using System.Net.Sockets;
using System.Net;
using System.Text;

namespace PizzacaseServerSite.ServerListening
{
    public class ListenerUDP
    {

        public void StartUdpServer()
        {
            int port = 8080;
            string ipAddress = "127.0.0.1";

            UdpClient udpListener = new UdpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
            Console.WriteLine("UDP server listening on " + ipAddress + ":" + port);

            try
            {
                while (true)
                {
                    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] receivedBytes = udpListener.Receive(ref remoteEndPoint);
                    string receivedMessage = Encoding.UTF8.GetString(receivedBytes);

                    Console.WriteLine("Received message: " + receivedMessage);

                    // Send a response back to the client
                    string responseMessage = "Message received successfully!";
                    byte[] responseData = Encoding.UTF8.GetBytes(responseMessage);
                    udpListener.Send(responseData, responseData.Length, remoteEndPoint);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                udpListener.Close();
            }
        }
    
    }
}
