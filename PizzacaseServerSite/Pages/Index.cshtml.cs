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
        private readonly ListenerUDP UDPlistener = new ListenerUDP();


        public static bool IsTcpConnectionOpen { get; set; }
        [BindProperty] public IEnumerable<Order> orders {get; set;}
        [BindProperty] public Guid orderId { get; set;}

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            orders = new OrderRepository().GetAllOrders();      
            IsTcpConnectionOpen = true;
            Task.Run(() => TCPlistener.StartTcpServer());
        }

        public RedirectToPageResult OnPostDelete() 
        {
            Console.WriteLine(orderId.ToString());
            return new RedirectToPageResult("Index");
        }

        public void OnPostConnection()
        {           
            IsTcpConnectionOpen = !IsTcpConnectionOpen;
            
            if (IsTcpConnectionOpen)
            {                
                Task.Run(() => TCPlistener.StartTcpServer());
                UDPlistener.StopUdpServer();
            }
            else
            {
                
                Task.Run(() => UDPlistener.StartUdpServer());
                TCPlistener.StopTcpServer();
            }
            orders = new OrderRepository().GetAllOrders();
        }
    }
}