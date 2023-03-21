namespace Pizzacase_Client.Models
{
    public class Order
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public Pizza[] Pizzas { get; set; }
        public string Date { get; set; }
        public string time { get; set; }
    }
}
