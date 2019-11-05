using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleWeb.Models;

namespace SampleWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICaptchaValidator _captchaValidator;

        public HomeController(ICaptchaValidator captchaValidator)
        {
            _captchaValidator = captchaValidator;
        }

        public IActionResult Index()
        {
            ViewData["Message"] = "";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(SampleViewModel collection, string captcha)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(captcha))
            {
                ModelState.AddModelError("captcha","Captcha validation failed");
            }
            if(ModelState.IsValid)
            {
                ViewData["Message"] = "Success";
            }
            return View(collection);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
