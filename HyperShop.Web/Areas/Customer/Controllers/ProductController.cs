using HyperShop.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HyperShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var productList = _context.Products.ToList();
            return View(productList);
        }
    }
}
