namespace Pizzacase_Client.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Zipcode_City { get; set; }
        public IEnumerable<Pizza> Pizzas { get; set; }
        public DateTime Date { get; set; }
    }
}
