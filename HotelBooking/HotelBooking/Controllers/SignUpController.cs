using HotelBooking.Dto;
using HotelBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelBooking.Controllers
{
    public class SignUpController : Controller
    {
        // GET: SignUp
        HotelBookingDBEntities dbContext = new HotelBookingDBEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterDto _registerDto)
        {
            Customers customerItem = dbContext.Customers.Where(x => x.Email == _registerDto.Email).FirstOrDefault();
            //if this email exists in database before or not
            if (customerItem == null)
            {
                Customers customers = new Customers()
                {
                    NameSurname = _registerDto.NameSurname,
                    Password = _registerDto.Password,
                    Email = _registerDto.Email
                };
                dbContext.Customers.Add(customers);
                dbContext.SaveChanges();

                Session["Customer"] = customerItem;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                _registerDto.ErrorMessage = "Bu email daha önceden eklenmiş.Farklı bir mail deneyiniz.";

                return View(_registerDto);
            }

        }
    }
}