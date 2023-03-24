namespace PizzacaseServerSite.Models
{
    public class Pizza
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Topping { get; set; }
        public string ToppingTypes { get; set; }
    }
}
