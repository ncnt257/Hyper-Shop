using HyperShop.DataAccess;
using HyperShop.Models;
using HyperShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
        public IActionResult Index(int page, int taking, string search, string orderBy, bool isDesc, List<int> categories, List<int> brands,
            List<int> colors, List<string> shoesHeights, List<string> closureTypes, List<string> genders)
        {
            ViewBag.Page = page;
            ViewBag.Taking = taking;
            ViewBag.Search = search;
            ViewBag.OrderBy = orderBy;
            ViewBag.IsDesc = isDesc;
            ViewBag.Categories = categories;
            ViewBag.Brands = brands;
            ViewBag.Colors = colors;
            ViewBag.ShoesHeights = shoesHeights;
            ViewBag.ClosureTypes = closureTypes;
            ViewBag.Genders = genders;

            ProductsPageVM productsPageVM = new ProductsPageVM()
            {
                Brands = _context.Products
                .Include(p => p.Brand)
                .ToList()
                .GroupBy(p => p.Brand)
                .OrderByDescending(grp => grp.Count())
                .Take(5)
                .Select(p => new Brand()
                {
                    Id = p.Key.Id,
                    Name = p.Key.Name
                }).ToList(),
                Categories = _context.Products
                .Include(p => p.Category)
                .ToList()
                .GroupBy(p => p.Category)
                .OrderByDescending(grp => grp.Count())
                .Take(5)
                .Select(p => new Category()
                {
                    Id = p.Key.Id,
                    Name = p.Key.Name
                }).ToList(),

                Colors = _context.Stock
                .Include(p => p.Product)
                .Include(p => p.Color)
                .ToList()
                .GroupBy(s => new { s.Product, s.Color })
                .GroupBy(s => s.Key.Color)
                .OrderByDescending(grp => grp.Count())
                .Take(5)
                .Select(p => new Color { Id = p.Key.Id, ColorValue = p.Key.ColorValue})
                .ToList(),

                ShoesHeightTypes = _context.Products
                .ToList()
                .GroupBy(p => p.ShoesHeight)
                .OrderByDescending(grp => grp.Count())
                .Take(5)
                .Select(p => p.Key).ToList(),

                ClosureTypes = _context.Products.ToList()
                .GroupBy(p => p.ClosureType)
                .OrderByDescending(grp => grp.Count())
                .Take(5)
                .Select(p => p.Key).ToList(),

                Genders = _context.Products.ToList()
                .GroupBy(p => p.Gender)
                .OrderByDescending(grp => grp.Count())
                .Take(5)
                .Select(p => p.Key).ToList(),
            };
            return View(productsPageVM);
        }
        public IActionResult Detail(int id)
        {
            ProductDetailVM productDetailVM = new();
            productDetailVM.Product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (productDetailVM.Product != null)
            {
                productDetailVM.Colors = _context.Stock
                    .GroupBy(s=>new {s.ColorId, s.ProductId})
                    .Where(g => g.Key.ProductId == productDetailVM.Product.Id)
                    .Select(g => new ProductColor() { 
                        ColorId = g.Key.ColorId,
                        PrimaryImage = _context.PrimaryImages.FirstOrDefault(p=>p.ProductId==productDetailVM.Product.Id && p.ColorId==g.Key.ColorId).Url })
                    .ToList();
                productDetailVM.Sizes = _context.Sizes.ToList();
                return View(productDetailVM);
            }
            return NotFound();
        }
    }
}
