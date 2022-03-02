using HyperShop.DataAccess;
using HyperShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HyperShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }


        //GET Product/Create
        public IActionResult Create()
        {
            return View();
        }

        //POST Product/Create
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }





        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productList = await _context.Products.ToListAsync();
            return Json(new { data = productList });
        }

        #endregion

    }
}
