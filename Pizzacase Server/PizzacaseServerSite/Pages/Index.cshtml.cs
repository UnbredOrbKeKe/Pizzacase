using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PizzacaseServerSite.ServerListening;

namespace PizzacaseServerSite.Pages
{
    public class IndexModel : PageModel
    {
        public static string Test = "\n";
        [BindProperty] public IEnumerable<string> Order { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Order = Test.Split('\n');
            
        }
    }
}