using BL.Contract.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UI.Models;

namespace UI.Controllers
{
    [Authorize(Roles = "User")]
    //[Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IShippingTypeService _IGenericRepository;
        public HomeController(ILogger<HomeController> logger, IShippingTypeService iGenericRepository)
        {
            _logger = logger;
            _IGenericRepository = iGenericRepository;
        }

        public async Task<IActionResult> Index()
        {
            //var types = await _IGenericRepository.GetAllAsync();
            return View();
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
