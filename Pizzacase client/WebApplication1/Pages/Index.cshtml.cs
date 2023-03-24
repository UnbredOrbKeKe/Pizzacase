using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Sockets;
using Pizzacase_client.Connection;
using Pizzacase_Client.Models;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {


        ConnectionServer server = ConnectionServer.GetInstance();
        private Guid orderID;

        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        [BindProperty] public Order order { get; set; }

        [BindProperty] public string Name { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            orderID = Guid.NewGuid();
            order = new Order { Pizzas = new List<Pizza>() };
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public List<Pizza> Pizzas
        {
            get
            {
                var value = _session.GetString("Pizzas");
                return value == null ? new List<Pizza>() : JsonConvert.DeserializeObject<List<Pizza>>(value);
            }
            set
            {
                _session.SetString("Pizzas", JsonConvert.SerializeObject(value));
            }
        }

        public void OnGet()
        {
        }

        public void OnPostKlik()
        {
            string selectedValue = Request.Form["selectedValue"];
            
            
            var newpizza = new Pizza
            {
                Name = selectedValue,
                Id = orderID,
                Topping = 2,
                ToppingTypes = "asdfasd, asdf"
            };
            Pizzas.Add(newpizza);






            //string test = server.SendMessage(klik);
            //Name = test;
        }

        public void OnPostOrder()
        {
            var guid = Guid.NewGuid();
            var order = new Order
            {
                Id = guid,
                Name = "asdfa",
                Address = "asdfasdfjasdofij",
                Zipcode_City = "asdfhopiwue",
                Pizzas = Pizzas.Select(p => new Pizza
                {
                    Name = p.Name,
                    Id = guid,
                    Topping = p.Topping,
                    ToppingTypes = p.ToppingTypes,
                }).ToList(),
                Date = DateTime.Now
            };

        }
    }
}