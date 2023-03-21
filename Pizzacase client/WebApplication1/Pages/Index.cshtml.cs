using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Sockets;
using Pizzacase_client.Connection;
using Pizzacase_Client.Models;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        public IEnumerable<Pizza> Pizzas { get; set; }

        ConnectionServer server = ConnectionServer.GetInstance();


        private readonly ILogger<IndexModel> _logger;

        [BindProperty] public string Name { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Pizzas = new List<Pizza>
            {
                new Pizza("Calzone"),
                new Pizza("Margerita"),
                new Pizza("Diavolo"),
                new Pizza("Salami")

            };
        }

        public void OnPostKlik()
        {
            string klik = "\n-------------\nJansen\nNieuwestad 14\n8901 PM\nLeeuwarden\nCalzone\n2\n0\nDiavolo\n1\n1\nMozzarella\n05/12/2022\n18:32\n-------------";
            string test = server.SendMessage(klik);
            Name = test;
        }
    }
}