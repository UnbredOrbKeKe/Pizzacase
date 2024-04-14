using System.Net.Sockets;
using System.Net;
using System.Text;
using PizzacaseServerSite.Decryption;
using System;
using PizzacaseServerSite.Pages;

namespace PizzacaseServerSite.ServerListening
{
    public class ListenerUDP
    {
        private UdpClient udpListener;
        private bool isRunning;

        public void StartUdpServer()
        {
            if (IndexModel.IsTcpConnectionOpen == false)
            {            
                bool loggedIn = false;
                int port = 8080;
                string ipAddress = "127.0.0.1";

                udpListener = new UdpClient(new IPEndPoint(IPAddress.Parse(ipAddress), port));
                isRunning = true;
                Console.WriteLine("UDP server listening on " + ipAddress + ":" + port);
                try
                {
                    while (true)
                    {

                        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        byte[] buffer = udpListener.Receive(ref remoteEndPoint);

                        // Creëer een byte-array met de juiste grootte gebaseerd op het aantal bytes dat daadwerkelijk is ontvangen
                        byte[] receivedBytes = new byte[buffer.Length];
                        Array.Copy(buffer, receivedBytes, buffer.Length);

                        // Decrypteer het ontvangen bericht
                        string decryptedMessage = AESHelper.DecryptStringFromBytes(receivedBytes);
                        Console.WriteLine("Received message: " + decryptedMessage);

                        byte[] responseBytes;
                        if (decryptedMessage == "Klant, wachtwoord")
                        {
                            responseBytes = AESHelper.EncryptStringToBytes("Correct, je bent ingelogd");
                            loggedIn = true;
                            udpListener.Send(responseBytes, responseBytes.Length, remoteEndPoint);
                        }
                        else if (loggedIn == false)
                        {
                            responseBytes = AESHelper.EncryptStringToBytes("Incorrect probeer opnieuw");
                            udpListener.Send(responseBytes, responseBytes.Length, remoteEndPoint);
                        }

                        

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
        public void StopUdpServer()
        {
            if(isRunning)
            {
                isRunning = false;
                udpListener.Close();
                udpListener.Dispose();
            }            
        }

    }
}
