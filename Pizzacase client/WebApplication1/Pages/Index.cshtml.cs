using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Sockets;
using Pizzacase_client.Connection;
using PizzacaseServerSite.Models;
using System;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        //comment out to switch between tcp and udp connection
        ConnectionTCPServer server = ConnectionTCPServer.GetInstance();
        //ConnectionUDPServer server = ConnectionUDPServer.GetInstance();


        private readonly ILogger<IndexModel> _logger;

        [BindProperty] public string Name { get; set; }
        [BindProperty] public List<Pizza> pizzas { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            

            
        }

        public void OnPostKlik()
        {
            var guid = Guid.NewGuid();
            pizzas = new List<Pizza>();
            pizzas.Add(new Pizza
            {
                Name = "calzone",
                Id = guid,
                Topping = 2,
                ToppingTypes = "asdfasd, asdf"
            });

            var order = new Order
            {
                Id = guid,
                Name = "asdfa",
                Address = "asdfasdfjasdofij",
                Zipcode_City = "asdfhopiwue",
                Pizzas = pizzas.Select(p => new Pizza
                {
                    Name = p.Name,
                    Id = guid,
                    Topping = p.Topping,
                    ToppingTypes = p.ToppingTypes,
                }).ToList(),
                Date = DateTime.Now
            };

            string test = server.SendMessage(order);
            Name = test;
        }
    }
}