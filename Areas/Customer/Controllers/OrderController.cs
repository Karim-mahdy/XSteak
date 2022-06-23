using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using xSteak.Data;
using xSteak.Models;
using xSteak.Models.ViewModels;
using xSteak.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace xSteak.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender EmailSender;
        public OrderController(ApplicationDbContext db,IEmailSender emailSender)
        {
            _db = db;
            EmailSender = emailSender;
        }
        public async Task<IActionResult> Confirm(int id)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderDetalisViewModel orderDetalisViewModel = new OrderDetalisViewModel()
            {
                OrderDetails = await _db.OrderDetails.Where(x => x.OrderId == id).ToListAsync(),
                OrderHeader = await _db.OrderHeader.Include(x => x.ApplicationUser).FirstOrDefaultAsync(x => x.UserId == claim.Value && x.Id == id),
            };
            
            return View(orderDetalisViewModel);
        }

        // Order Details action link
        [Authorize]
        public async Task<IActionResult> OrderHistory()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _db.ApplicationUser.Find(claim.Value);
            List<OrderDetalisViewModel> orderList = new List<OrderDetalisViewModel>();
            List<OrderHeader> orderHeader = new List<OrderHeader>();
            if (user.JopType == "Manager")
            {
                orderHeader = await _db.OrderHeader.Include(o => o.ApplicationUser).ToListAsync();
            }
            else
            {
                orderHeader = await _db.OrderHeader.Include(o => o.ApplicationUser).Where(x => x.UserId == claim.Value).ToListAsync();
            }

            foreach (var item in orderHeader)
            {
                OrderDetalisViewModel orderDetalisViewModel = new OrderDetalisViewModel()
                {
                    OrderHeader = item,
                    OrderDetails = await _db.OrderDetails.Where(o => o.OrderId == item.Id).ToListAsync()
                };
                orderList.Add(orderDetalisViewModel);
            }


            return View(orderList);
        }



        public async Task<IActionResult> GetOrderDetails(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            OrderDetalisViewModel orderDetalisView = new OrderDetalisViewModel()
            {
                //.Include(o => o.ApplicationUser)
                OrderHeader = await _db.OrderHeader.Include(x => x.ApplicationUser).FirstOrDefaultAsync(o => o.Id == id && o.UserId == claim.Value),
                OrderDetails = await _db.OrderDetails.Where(o => o.OrderId == id).ToListAsync()
            };
            return PartialView("_IndividualOrderDetalis", orderDetalisView);
        }

        public IActionResult GetOrderStatus(int id)
        {
            return PartialView("_OrderStatus", _db.OrderHeader.Where(x => x.Id == id).FirstOrDefault().Status);
        }
        //


    }
}
