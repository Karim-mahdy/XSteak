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
using System.Text;
using System.Threading.Tasks;

namespace xSteak.Areas.Admin.Controllers
{
    [Area("Admin")]
   
    public class ManageOrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender EmailSender;
        public ManageOrderController(ApplicationDbContext db, IEmailSender emailSender)
        {
            _db = db;
            EmailSender = emailSender;
        }

        [Authorize(Roles = SD.ManagerUser + "," + SD.KitchenUser)]
        public async Task<IActionResult> ManageOrder()
        {
            List<OrderDetalisViewModel> OrderDetalis = new List<OrderDetalisViewModel>();

            List<OrderHeader> orderHeaders = await _db.OrderHeader.Where(x => x.Status.ToLower() == SD.StatusSubmitted || x.Status.ToLower() == SD.StatusInProcess)
                                             .OrderByDescending(x => x.PickupTime)
                                             .ToListAsync();

            foreach (var item in orderHeaders)
            {
                OrderDetalisViewModel orderDetalisVM = new OrderDetalisViewModel()
                {
                    OrderHeader = item,
                    OrderDetails = await _db.OrderDetails.Where(x => x.OrderId == item.Id).ToListAsync()
                };
                OrderDetalis.Add(orderDetalisVM);
            }
            return View(OrderDetalis.OrderByDescending(x => x.OrderHeader.PickupTime));
        }




        [Authorize(Roles = SD.ManagerUser + "," + SD.KitchenUser)]
        public async Task<IActionResult> OrderPreapare(int id)
        {
            OrderHeader orderHeader = await _db.OrderHeader.FindAsync(id);
            orderHeader.Status = SD.StatusInProcess;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(ManageOrder));
        }



        [Authorize(Roles = SD.ManagerUser + "," + SD.KitchenUser)]
        public async Task<IActionResult> OrderReady(int id)
        {
            OrderHeader orderHeader = await _db.OrderHeader.FindAsync(id);
            orderHeader.Status = SD.StatusReady;
            await _db.SaveChangesAsync();
            //  email logic to notify user that order is read for pickup
            await EmailSender.SendEmailAsync(_db.Users.Where(x => x.Id == orderHeader.UserId).FirstOrDefault().Email, "xSteak -  OrderReady " + orderHeader.Id.ToString(), "order has been Ready successfuly");

            return RedirectToAction(nameof(ManageOrder));
        }




        [Authorize(Roles = SD.ManagerUser + "," + SD.KitchenUser)]
        public async Task<IActionResult> OrderCancel(int id)
        {
            OrderHeader orderHeader = await _db.OrderHeader.FindAsync(id);
            orderHeader.Status = SD.StatusCancelled;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(ManageOrder));
        }


        public async Task<IActionResult> OrderComplete(int id)
        {
            _db.OrderHeader.Find(id).Status = SD.StatusCompleted;
            await _db.SaveChangesAsync();
            return RedirectToAction("OrderPickup");
        }



        // Order Details action link
        [Authorize]
        public async Task<IActionResult> OrderPickup(string searchName = null, string searchPhone = null, string searchEmail = null)
        {

            StringBuilder pram = new StringBuilder();
            pram.Append("/Customer/Order/OrderPickup?");
            pram.Append("&searchName");
            if (searchName != null)
            {
                pram.Append(searchName);
            }
            pram.Append("&srearchPhone");
            if (searchPhone != null)
            {
                pram.Append(searchPhone);
            }
            pram.Append("&srearchEmail");
            if (searchEmail != null)
            {
                pram.Append(searchEmail);
            }
            List<OrderDetalisViewModel> orderList = new List<OrderDetalisViewModel>();
            List<OrderHeader> orderHeader = new List<OrderHeader>();
            if (searchPhone != null || searchName != null || searchEmail != null)
            {

                var user = new ApplicationUser();

                if (searchName != null)
                {
                    orderHeader = await _db.OrderHeader.Where(o => o.PickupName.ToLower().Contains(searchName.ToLower()) && o.Status == SD.StatusReady)
                        .OrderByDescending(o => o.OrderDate).ToListAsync();
                }
                else
                {
                    if (searchPhone != null)
                    {
                        orderHeader = await _db.OrderHeader.Where(o => o.PickupNumber.Contains(searchName) && o.Status == SD.StatusReady)
                            .OrderByDescending(o => o.OrderDate).ToListAsync();
                    }
                    else
                    {
                        user = await _db.ApplicationUser.Where(u => u.Email.ToLower().Contains(searchEmail.ToLower())).FirstOrDefaultAsync();
                        if (searchEmail != null)
                        {
                            orderHeader = await _db.OrderHeader.Include(o => o.ApplicationUser)
                                .Where(o => o.UserId == user.Id && o.Status == SD.StatusReady).OrderByDescending(o => o.OrderDate).ToListAsync();
                        }
                    }
                }
            }
            else
            {
                orderHeader = await _db.OrderHeader.Include(o => o.ApplicationUser).Where(x => x.Status == SD.StatusReady).ToListAsync();

            }
            foreach (OrderHeader item in orderHeader)
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

       

        public async Task<IActionResult> Invoice(int? id)
        {
            OrderDetalisViewModel orderDetalisVM = new OrderDetalisViewModel()
            {
                OrderHeader = await _db.OrderHeader.Include(x => x.ApplicationUser).Where(x => x.Id == id).FirstOrDefaultAsync(),
                OrderDetails = await _db.OrderDetails.Where(x => x.OrderId == id).ToListAsync()
            };

            return View(orderDetalisVM);
        }
    }
}

