using FItnessTrack.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FItnessTrack.Controllers
{
    public class ShopController: Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //Get a list of categories to display to customers on the main shopping page
            var Services = _context.Services.OrderBy(c => c.ServiceName).ToList() ;
            return View(Services);
        }
    }
}
