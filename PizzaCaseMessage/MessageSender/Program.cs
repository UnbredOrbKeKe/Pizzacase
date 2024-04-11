using MessageSender;
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
        byte[] IV = AESHelper.GenerateIV();
        string ivString = BitConverter.ToString(IV).Replace("-", "");
        Console.WriteLine(ivString);


        while (true)
        {
            Console.WriteLine("Typ 'tcp' of 'udp' om bestelling te sturen of typ 'exit' om af te sluiten");
            string input = Console.ReadLine();
            string message = "Jansen\r\nNieuwestad 14\r\n8901 PM Leeuwarden\r\nCalzone\r\n2\r\n0\r\nDiavolo\r\n1\r\n1\r\nMozzarella\r\n" + DateTime.Now;
            byte[] encryptedMessage = AESHelper.EncryptStringToBytes(message);

            //bericht sturen via tcp
            if (input.ToLower() == "tcp")
            {
                try
                {
                    TcpClient tcpClient = new TcpClient(server, port);
                    NetworkStream stream = tcpClient.GetStream();

                    // Send data                                       
                    stream.Write(encryptedMessage, 0, encryptedMessage.Length);
                    Console.WriteLine("Bericht verstuurd via tcp");

                    // Close connection
                    stream.Close();
                    tcpClient.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            //bericht sturen via udp
            else if (input.ToLower() == "udp")
            {
                UdpClient udpClient = new UdpClient();
                try
                {
                   
                    udpClient.Send(encryptedMessage, encryptedMessage.Length, server, port);
                    Console.WriteLine("Bericht verstuurd via udp");

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

