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
using Stripe;
using System.Configuration; // Read the Strip API keys from appssettings.json
using Microsoft.Extensions.Configuration;
using Stripe.Checkout;

namespace FItnessTrack.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        private IConfiguration _iconfiguation;

        public ShopController(ApplicationDbContext context, IConfiguration iconfiguation)
        {
            _context = context;
            _iconfiguation = iconfiguation;
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

        //GET /Shop/RemoveFromCart
        public IActionResult RemoveFromCart(int id)
        {
            // find the item with this PK value
            var cartItem = _context.Carts.Find(id);

            // delete record from Carts table
            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
                _context.SaveChanges();
            }

            //redirect to updated Cart
            return RedirectToAction("Cart");
        }

        [Authorize]

        public IActionResult Checkout()
        {
            return View();
        }

        //POST: /Shop/Checkout
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout([Bind("FirstName, LastName, Address, City, Province, PostalCode")] Models.Order order)
        {
            //Populate the 3 automatic Order properties
            order.OrderDate = DateTime.Now;
            order.CustomerId = User.Identity.Name;
            //calc order total based on the current cart
            order.Total = (from c in _context.Carts
                           where c.CustomerId == HttpContext.Session.GetString("CustomerId")
                           select c.Quantity * c.Charge).Sum();

            //use SessionExtension Obj to store the order Obj in a session variable
            HttpContext.Session.SetObject("Order", order);

            //redirect to Payment Page
            return RedirectToAction("Payment");
        }

        [Authorize]
        public IActionResult Payment()
        {
            var order = HttpContext.Session.GetObject<Models.Order>("Order");

            ViewBag.Total = order.Total;

            // also use the ViewBag to set the PublishableKey, which we can read from the Configuration
            ViewBag.PublishableKey = _iconfiguation.GetSection("Stripe")["PublishableKey"];

            // load the Payment view
            return View();
        }

        //POST /Shop/ProcessPayment
        [Authorize]
        [HttpPost]
        public IActionResult ProcessPayment()
        {
            var order = HttpContext.Session.GetObject<Models.Order>("Order");
            // get the Stripe Secret Key from the configuration and pass it before we can create a new checkout session
            StripeConfiguration.ApiKey = _iconfiguation.GetSection("Stripe")["SecretKey"];

            // code will go here to create and submit Stripe payment charge
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                      UnitAmount = (long?)order.Total,
                      Currency = "cad",
                      ProductData = new SessionLineItemPriceDataProductDataOptions
                      {
                        Name = "Fitness Track Purchase",
                      },
                    },
                    Quantity = 1,
                  },
                },
                Mode = "payment",
                SuccessUrl = "https://" + Request.Host + "/Shop/SaveOrder",
                CancelUrl = "https://" + Request.Host + "/Shop/Cart"
            };
            var service = new SessionService();
            Session session = service.Create(options);
            return Json(new { id = session.Id });
        }
        // GET: /Shop/SaveOrder
        [Authorize]
        public IActionResult SaveOrder()
        {
            // get the order from the session variable
            var order = HttpContext.Session.GetObject<Models.Order>("Order");
            // save as new order to the db
            _context.Orders.Add(order);
            _context.SaveChanges();

            // save the line items as new order details records
            var cartItems = _context.Carts.Where(c => c.CustomerId == HttpContext.Session.GetString("CustomerId"));
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ServiceId = item.ServiceId,
                    Quantity = item.Quantity,
                    Charge = item.Charge,
                    OrderId = order.OrderId
                };

                _context.OrderDetails.Add(orderDetail);
            }
            _context.SaveChanges();

            // delete the items from the user's cart
            foreach (var item in cartItems)
            {
                _context.Carts.Remove(item);
            }
            _context.SaveChanges();

            // set the Session ItemCount variable (which shows in the navbar) back to zero
            HttpContext.Session.SetInt32("ItemCount", 0);

            // redirect to order confirmation page i.e. /Orders/Details/1
            return RedirectToAction("Details", "Orders", new { @id = order.OrderId });
        }
    }
}
