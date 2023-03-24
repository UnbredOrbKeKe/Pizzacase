namespace Pizzacase_Client.Models
{
    public class Pizza
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Topping { get; set; }
        public string ToppingTypes { get; set; }

        //public Pizza(int id, string name, int topping, string toppingtypes)
        //{
        //    Name = name;
        //    Id = id;
        //    this.topping = topping;
        //    ToppingTypes = toppingtypes;

        //}

        
 
    }
}
