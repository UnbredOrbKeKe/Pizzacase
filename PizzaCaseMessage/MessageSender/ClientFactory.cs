using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class ClientFactory
{
    public static UdpClient CreateUdpClient(string server, int port)
    {
        return new UdpClient(server, port);
    }

    public static TcpClient CreateTcpClient(string server, int port)
    {
        return new TcpClient(server, port);
    }
}
