﻿using Microsoft.AspNetCore.Mvc;

namespace HyperShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]

    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}