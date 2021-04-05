using FItnessTrack.Data;
using FItnessTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FItnessTrack.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //Get a list of categories to display to customers on the main shopping page
            var Services = _context.Services.OrderBy(c => c.ServiceName).ToList();
            return View(Services);
        }
        public IActionResult Browse(int id)
        {
            // query Products for the selected Category
            var products = _context.Trainings.Where(p => p.ServiceId == id).OrderBy(p => p.Desciption).ToList();

            // get Name of selected Category.  Find() only filters on key fields
            ViewBag.category = _context.Services.Find(id).ServiceName.ToString();
            return View(products);
        }
        public IActionResult AddToCart(int ServiceId, int Quantity)
        {
            var price = _context.Services.Find(ServiceId).Charge;

            var currentDateTime = DateTime.Now;

            var CustomerId = GetCustomerId();

            var cart = new Cart
            {
                ServiceId = ServiceId,
                Quantity = Quantity,
                Charge = price,
                DateCreated = currentDateTime,
                CustomerId = CustomerId   
            };

            _context.Carts.Add(cart);
            _context.SaveChanges();

            return RedirectToAction("Cart");
        }

        private string GetCustomerId()
        {
            // check the session for an existing CustomerId
            if (HttpContext.Session.GetString("CustomerId") == null)
            {
                // if we don't already have an existing CustomerId in the session, check if customer is logged in
                var CustomerId = "";

                // if customer is logged in, use their email as the CustomerId
                if (User.Identity.IsAuthenticated)
                {
                    CustomerId = User.Identity.Name; //Name = email address
                }
                // if the customer is anonymous, use Guid to create a new identifier
                else
                {
                    CustomerId = Guid.NewGuid().ToString();
                }
                // now store the CustomerId in a session variable
                HttpContext.Session.SetString("CustomerId", CustomerId);
            }
            // return the Session variable
            return HttpContext.Session.GetString("CustomerId");
        }


        public IActionResult Cart()
        {
            // fetch current cart for display
            var CustomerId = "";
            // in case user comes to cart page before adding anything, identify them first
            if (HttpContext.Session.GetString("CustomerId") == null)
            {
                CustomerId = GetCustomerId();
            }
            else
            {
                CustomerId = HttpContext.Session.GetString("CustomerId");
            }

            // query the db for this customer
            //Add the "Include(c => c.Product)" to have our query include the Parent Products into our cart
            var cartItems = _context.Carts.Include(c => c.Service).Where(c => c.CustomerId == CustomerId).ToList();

            // pass the data to the view for display
            return View(cartItems);
        }

        [Authorize]

        public IActionResult Checkout()
        {
            return View();
        }
    }
}
