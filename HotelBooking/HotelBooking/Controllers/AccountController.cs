using HotelBooking.Dto;
using HotelBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelBooking.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        HotelBookingDBEntities dbContext = new HotelBookingDBEntities();

        public ActionResult Index()
        {
            Customers customerItem = (Customers)Session["Customer"] ?? null;

            AccountDetailDto accountDetailDto = new AccountDetailDto()
            {
                countryV10s = dbContext.CountryV10.ToList(),
                townV10s = dbContext.TownV10.ToList(),
                customerItem = customerItem
            };


            return View(accountDetailDto);
        }

        public ActionResult AccountEdit(Customers customers)
        {
            Customers customerItem = (Customers)Session["Customer"] ?? null;

            Customers customerContext = null;

            if (customerItem != null)
            {
                 customerContext = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerItem.CustomerId);
                
                customerContext.NameSurname = customers.NameSurname;
                customerContext.Email = customers.Email;
                customerContext.Phone = customers.Phone;
                customerContext.Address = customers.Address;
                customerContext.BirthDate = customers.BirthDate;
                customerContext.CountryId = customers.CountryId;
                customerContext.CityId = customers.CityId;

                dbContext.SaveChanges();
            }

            AccountDetailDto accountDetailDto = new AccountDetailDto()
            {
                countryV10s = dbContext.CountryV10.ToList(),
                townV10s = dbContext.TownV10.ToList(),
                customerItem = customerContext
            };


            return View("Index", accountDetailDto);
        }

        [HttpPost]
        public JsonResult FillCity(string countryId)
        {
            int countyKey = Convert.ToInt32(countryId);

            List<TownV10> townList = dbContext.TownV10.Where(x => x.CountryId == countyKey).ToList();

            return Json(townList);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        
        public ActionResult PasswordEdit(string NewPassword)
        {
            Customers customerItem = (Customers)Session["Customer"] ?? null;

            Customers customerContext = null;

            if (customerItem != null)
            {
                customerContext = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerItem.CustomerId);

                customerContext.Password = NewPassword;

                dbContext.SaveChanges();
            }

            AccountDetailDto accountDetailDto = new AccountDetailDto()
            {
                countryV10s = dbContext.CountryV10.ToList(),
                townV10s = dbContext.TownV10.ToList(),
                customerItem = customerContext
            };
            
            return View("Index", accountDetailDto);
        }
    }
}