﻿using Microsoft.AspNetCore.Mvc;

namespace AkaryakitOtomasyonu.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
