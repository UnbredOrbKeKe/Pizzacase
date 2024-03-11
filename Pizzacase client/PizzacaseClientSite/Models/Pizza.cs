namespace PizzacaseServerSite.Models
{
    public class Pizza
    {
        public Guid OrderId { get; set; }
        public string PizzaName { get; set; }
        public int Amount { get; set; }
        public int ExtraToppings { get; set; }
        public string ToppingTypes { get; set; }
    }
}
