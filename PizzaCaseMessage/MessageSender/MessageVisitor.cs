using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MessageSender
{
    interface IMessageVisitor
    {
        void VisitTCPMessage(byte[] message);
        void VisitUDPMessage(byte[] message);
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
    }

    // Concrete visitor for UDP messages
    class UDPMessageProcessor : IMessageVisitor
    {
        string server = "127.0.0.1";
        int portUDP = 8080;
        public void VisitTCPMessage(byte[] message)
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
    }
}
