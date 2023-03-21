
using System.Net;
using System.Net.Sockets;
using System.Text;

Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 8080);
listener.Bind(localEndPoint);
listener.Listen(10);

while (true)
{
    Console.WriteLine("waiting for a connection....");
    Socket handler = listener.Accept();

    Task.Run(() =>
    {
        try
        {
            byte[] buffer = new byte[1024];
            int bytesReceived = handler.Receive(buffer);
            string message = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
            Console.WriteLine("Received message: {0}", message);

            string response = "Hello CLient!";
            byte[] responseBytes = Encoding.ASCII.GetBytes(response);
            handler.Send(responseBytes);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: {0}", ex.Message);
        }
        finally
        {
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
    });
}