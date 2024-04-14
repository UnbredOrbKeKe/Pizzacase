using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MessageSender
{
    interface IMessageVisitor
    {
        void VisitTCPMessage(byte[] message);
        void VisitUDPMessage(byte[] message);
        void VisitUDPMessageLogin(byte[] message);
    }

    // Concrete visitor for TCP messages
    class TCPMessageProcessor : IMessageVisitor
    {
        string server = "127.0.0.1";
        int portTCP = 8080;
        
        public void VisitTCPMessage(byte[] encryptedMessageTCP)
        {
            // Implement processing logic for TCP message
            Console.WriteLine("Processing TCP message");

            TcpClient tcpClient = ClientFactory.CreateTcpClient(server, portTCP);
            NetworkStream stream = tcpClient.GetStream();

            // Send data                                       
            stream.Write(encryptedMessageTCP, 0, encryptedMessageTCP.Length);

            // Close connection
            stream.Close();
            tcpClient.Close();
        }

        public void VisitUDPMessage(byte[] message)
        {
            // Do nothing when encountering UDP message
        }
        public void VisitUDPMessageLogin(byte[] message)
        {
            // Do nothing when encountering UDP message
        }
    }

    // Concrete visitor for UDP messages
    class UDPMessageProcessor : IMessageVisitor
    {
        string server = "127.0.0.1";
        int portUDP = 8080;
        public void VisitTCPMessage(byte[] encryptedMessageUDP)
        {
            // Do nothing when encountering TCP message
        }

        public void VisitUDPMessage(byte[] encryptedMessageUDP)
        {
            // Implement processing logic for UDP message
            Console.WriteLine("Processing UDP message");

            UdpClient udpClient = ClientFactory.CreateUdpClient(server, portUDP);
            try
            {
                udpClient.Send(encryptedMessageUDP, encryptedMessageUDP.Length);
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

        public void VisitUDPMessageLogin(byte[] inlogEnc)
        {

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
                    Program.loggedIn = true;
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
    }
}
