﻿using MessageSender;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        bool loggedIn = false;
        string server = "127.0.0.1";
        int portTCP = 8080;
        int portUDP = 8080;
        byte[] IV = AESHelper.GenerateIV();

        while (true)
        {
            if(loggedIn == false) 
            {
                Console.WriteLine("Stuur inloggegevens als: 'AccountType, Wachtwoord'");
                string inlog = Console.ReadLine();
                byte[] inlogEnc = AESHelper.EncryptStringToBytes(inlog);

                UdpClient udpClient = new UdpClient();
                try
                {
                    udpClient.Send(inlogEnc, inlogEnc.Length, server, portUDP);
                    Console.WriteLine("Inloggegevens gestuurd");

                    // Receive response from the server
                    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] responseBytes = udpClient.Receive(ref remoteEndPoint);
                    string response = AESHelper.DecryptStringFromBytes(responseBytes);

                    Console.WriteLine("Server response: " + response);

                    if (response == "Correct, je bent ingelogd")
                    {
                        loggedIn = true;
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine("Error: Unable to connect to the server via UDP. " + e.Message);
                }
                finally
                {
                    udpClient.Close();
                }

            }

            if (loggedIn)
            {

                Console.WriteLine("Type 'tcp' or 'udp' to send a message, or 'exit' to quit");
                string input = Console.ReadLine();
                string messageTCP = "Jansen\r\nNieuwestad 14\r\n8901 PM Leeuwarden\r\nCalzone\r\n2\r\n0\r\nDiavolo\r\n1\r\n1\r\nMozzarella\r\n" + DateTime.Now;
                string messageUDP = "Fransje\r\nBoornBats 14\r\n8901 PM Leeuwarden\r\nCalzone\r\n2\r\n0\r\nDiavolo\r\n1\r\n1\r\nMozzarella\r\n" + DateTime.Now;
                byte[] encryptedMessageTCP = AESHelper.EncryptStringToBytes(messageTCP);
                byte[] encryptedMessageUDP = AESHelper.EncryptStringToBytes(messageUDP);

                if (input.ToLower() == "tcp")
                {
                    try
                    {
                        TcpClient tcpClient = new TcpClient(server, portTCP);
                        NetworkStream stream = tcpClient.GetStream();

                        // Send data                                       
                        stream.Write(encryptedMessageTCP, 0, encryptedMessageTCP.Length);
                        Console.WriteLine("Message sent via TCP");

                        // Close connection
                        stream.Close();
                        tcpClient.Close();
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine("Error: Unable to connect to the server via TCP. " + e.Message);
                    }
                }
                else if (input.ToLower() == "udp")
                {
                    UdpClient udpClient = new UdpClient();
                    try
                    {
                        udpClient.Send(encryptedMessageUDP, encryptedMessageUDP.Length, server, portUDP);
                        Console.WriteLine("Message sent via UDP");
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine("Error: Unable to connect to the server via UDP. " + e.Message);
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
                else if (input.ToLower() == "logout")
                {
                    loggedIn = false;
                }
                else
                {
                    Console.WriteLine("Invalid command. Type 'tcp', 'udp', 'logout' or 'exit'.");
                }
            }
        }
    }
}
