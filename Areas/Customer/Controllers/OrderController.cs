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


    }
}
