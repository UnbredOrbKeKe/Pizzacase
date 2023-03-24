﻿namespace PizzacaseServerSite.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string address { get; set; }
        public string zipcode_city { get; set; }
        public IEnumerable<Pizza> pizzas { get; set; }
        public DateTime Date { get; set; }
    }
}
