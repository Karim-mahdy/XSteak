using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using xSteak.Data;
using xSteak.Models;
using xSteak.Models.ViewModels;
using xSteak.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace xSteak.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public OrderDetailsCart detailsCart { get; set; }
        
        public CartController(ApplicationDbContext db , IEmailSender emailSender )

        {
          
            _db = db;
        }

        [Authorize(Roles = SD.CustomerEndUser + "," + SD.ManagerUser + "," + SD.KitchenUser + "," + SD.FrontDeskUser)]
        public async Task< IActionResult> Index()
        {

            detailsCart = new OrderDetailsCart()
            {
                OrderHeader = new Models.OrderHeader()
            };
            detailsCart.OrderHeader.OrderTotal = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var cart = _db.ShoppingCart.Where(x => x.ApplicationUserId == claim.Value);

            if (cart!=null)
            {
                detailsCart.ListCart = cart.ToList();
            }
            foreach (var list in detailsCart.ListCart)
            {
                list.MenuItem = await _db.MenuItems.FirstOrDefaultAsync(m=>m.Id==list.MenuItemId);
                if (list.MenuItem == null)
                {
                    continue;
                }
                detailsCart.OrderHeader.OrderTotal = detailsCart.OrderHeader.OrderTotal + (list.MenuItem.Price * list.Count);
                
            }
            detailsCart.OrderHeader.OrderTotalOriginal = detailsCart.OrderHeader.OrderTotal;
           
           
            return View(detailsCart);
        }

        //Summary

       
    }
}
