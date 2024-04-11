using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        string server = "127.0.0.1";
        int port = 8080;

        while (true)
        {
            Console.WriteLine("Typ 'tcp' of 'udp' om bestelling te sturen of typ 'exit' om af te sluiten");
            string input = Console.ReadLine();
            string message = "Jansen\r\nNieuwestad 14\r\n8901 PM Leeuwarden\r\nCalzone\r\n2\r\n0\r\nDiavolo\r\n1\r\n1\r\nMozzarella\r\n" + DateTime.Now;

            if (input.ToLower() == "tcp")
            {
                try
                {
                    TcpClient tcpClient = new TcpClient(server, port);
                    NetworkStream stream = tcpClient.GetStream();

                    // Send data

                    byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);

                    // Close connection
                    stream.Close();
                    tcpClient.Close();

                    Console.WriteLine("Message sent successfully.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            else if (input.ToLower() == "udp")
            {
                UdpClient udpClient = new UdpClient();
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    udpClient.Send(data, data.Length, server, port);

                    // Receive response from the server
                    IPEndPoint serverEndpoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] responseBytes = udpClient.Receive(ref serverEndpoint);
                    string responseMessage = Encoding.UTF8.GetString(responseBytes);

                    Console.WriteLine("Response from server: " + responseMessage);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
                finally
                {
                    udpClient.Close();
                }
            }
            else if (input.ToLower() == "exit")
            {
                break;
            }
            else
            {
                Console.WriteLine("Das geen commando. Typ 'tcp', 'udp' of 'exit'.");
            }
        }
    }
}

