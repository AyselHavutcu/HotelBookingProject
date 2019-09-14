using HotelBooking.Dto;
using HotelBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelBooking.Controllers
{
    public class SignInController : Controller
    {
        // GET: Register
        HotelBookingDBEntities dbContext = new HotelBookingDBEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            Customers customerItem = dbContext.Customers.Where(x => x.Email == Email && x.Password == Password).FirstOrDefault();

            if (customerItem != null)
            {
                Session["Customer"] = customerItem;

                return Json("success");
            }
            else
            {
                return Json("error");
            }
        }
    }
}