using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PizzacaseServerSite.Models;
using PizzacaseServerSite.Repository;
using PizzacaseServerSite.ServerListening;
using System.Net.Sockets;

namespace PizzacaseServerSite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ListenerTCP TCPlistener = ListenerTCP.Instance;
        private readonly ListenerUDP UDPlistener = ListenerUDP.Instance;

        public static bool IsTcpConnectionOpen { get; set; }
        [BindProperty] public IEnumerable<Order> orders { get; set; }
        [BindProperty] public Guid orderId { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            orders = new OrderRepository().GetAllOrders();
            Console.WriteLine(IsTcpConnectionOpen);
            if (IsTcpConnectionOpen)
            {
                Task.Run(() => { TCPlistener.StartTcpServer(); });
                Task.Run(() => { UDPlistener.StopUdpServer(); });
            }
            else
            {
                Task.Run(() => { UDPlistener.StartUdpServer(); });
                Task.Run(() => { TCPlistener.StopTcpServer(); });
            }            
        }

        public RedirectToPageResult OnPostDelete()
        {
            Console.WriteLine(orderId.ToString());
            return new RedirectToPageResult("Index");
        }

        public RedirectToPageResult OnPostConnection()
        {
            IsTcpConnectionOpen = !IsTcpConnectionOpen;

            return new RedirectToPageResult("Index");
        }
    }
}
