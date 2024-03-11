namespace PizzacaseServerSite.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerZipcode { get; set; }
        public IEnumerable<Pizza> PizzaList { get; set; }
        public DateTime Datum { get; set; }
    }
}
