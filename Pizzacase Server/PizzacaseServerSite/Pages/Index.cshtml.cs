using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PizzacaseServerSite.Models;
using PizzacaseServerSite.Repository;
using PizzacaseServerSite.ServerListening;

namespace PizzacaseServerSite.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty] public IEnumerable<Order> orders {get; set;}
        [BindProperty] public Guid orderId { get; set;}

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            orders = new OrderRepository().GetAllOrders();          
        }

        public RedirectToPageResult OnPostDelete() 
        {
            Console.WriteLine(orderId.ToString());
            return new RedirectToPageResult("Index");
        }
    }
}