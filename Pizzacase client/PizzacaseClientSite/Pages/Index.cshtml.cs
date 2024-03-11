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

        [BindProperty] public List<Pizza> pizzas { get; set; }

        [BindProperty] public Order Order { get; set; }

        public List<Pizza> _pizzas = new List<Pizza>
        {
            new Pizza { PizzaName = "Margherita" },
            new Pizza { PizzaName = "Pepperoni" },
            new Pizza { PizzaName = "Hawaiian" },
            new Pizza { PizzaName = "Vegetarian" },
            new Pizza { PizzaName = "Meat Lovers" }
        };


        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }


        public RedirectToPageResult OnPostKlik()
        {
            var guid = Guid.NewGuid();
            pizzas = new List<Pizza>();
            pizzas.Add(new Pizza
            {
                PizzaName = "calzone",
                OrderId = guid,
                Amount = 3,
                ExtraToppings = 2,
                ToppingTypes = "uien, champignons"
            });
            pizzas.Add(new Pizza
            {
                PizzaName = "Salami",
                OrderId = guid,
                Amount = 2,
                ExtraToppings = 0,
                ToppingTypes = ""
            });

            var order = new Order
            {
                OrderId = guid,
                CustomerName = Order.CustomerName,
                CustomerAddress = Order.CustomerAddress,
                CustomerZipcode = Order.CustomerZipcode,
                PizzaList = pizzas.Select(p => new Pizza
                {
                    PizzaName = p.PizzaName,
                    OrderId = guid,
                    Amount =p.Amount,
                    ExtraToppings = p.ExtraToppings,
                    ToppingTypes = p.ToppingTypes,
                }).ToList(),
                Datum = DateTime.Now
            };

            server.SendMessage(order);

            return new RedirectToPageResult("Index");
        }
    }
}