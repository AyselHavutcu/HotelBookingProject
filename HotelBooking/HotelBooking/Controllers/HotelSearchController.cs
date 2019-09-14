using HotelBooking.Dto;
using HotelBooking.Models;
using HotelBooking.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelBooking.Controllers
{
    public class HotelSearchController : Controller
    {
        HotelBookingDBEntities dbContext = new HotelBookingDBEntities();
        public ActionResult Index(string hotelNameOrCity, string checkinDate, string  checkoutDate, string room, string adult, string child)
        {
          // DateTime checkinDateKey = checkinDate == null ? DateTime.Now : DateTime.Parse(checkinDate,System.Globalization.CultureInfo.InvariantCulture);
            //DateTime checkoutDateKey = checkoutDate == null ? DateTime.Now : DateTime.Parse(checkinDate, System.Globalization.CultureInfo.InvariantCulture);

            int townId = 0;
            bool Iscity = false;

            if (hotelNameOrCity != null)
            {
                foreach (var cityItem in dbContext.TownV10)
                {
                    if (cityItem.TownName.ToLower().Trim() == hotelNameOrCity.ToLower().Trim())
                    {
                        townId = dbContext.TownV10.Where(x => x.TownName.ToLower().Trim() == hotelNameOrCity.ToLower().Trim()).FirstOrDefault().TownId;

                        Iscity = true;
                    }
                }
            }

            List<HotelSearchItem> hotelSearchList = null;

            if (Iscity)
            {

                hotelSearchList = (from hotel in dbContext.Hotels
                                   join city in dbContext.TownV10 on hotel.CityId equals city.TownId
                                   join hotelPhotos in dbContext.HotelPhotos on hotel.HotelId equals hotelPhotos.HotelId
                                   join hotelRooms in dbContext.HotelRooms on hotel.HotelId equals hotelRooms.HotelId
                                   join roomTypes in dbContext.RoomTypes on hotelRooms.RoomTypeId equals roomTypes.RoomTypeId
                                   where city.TownId == townId && hotelPhotos.OrderNumber == 1 && roomTypes.Title == "Standart Oda"
                                   select new { hotel, hotelPhotos, city, roomTypes, hotelRooms }).ToList()
                                   .GroupBy(x => x.hotel.HotelId).Select(h => new HotelSearchItem()
                                             {
                                                 CityName = h.FirstOrDefault().city.TownName,
                                                 CountryName = "Türkiye",
                                               //  Day = (checkoutDateKey.Day - checkinDateKey.Day) - 1,
                                                 Description = h.FirstOrDefault().hotel.Description,
                                                 HotelId = h.FirstOrDefault().hotel.HotelId,
                                                 HotelImage = h.FirstOrDefault().hotelPhotos.Photo,
                                                 HotelName = h.FirstOrDefault().hotel.Title,
                                                 Price = h.FirstOrDefault().hotelRooms.Price,
                                                 RoomName = h.FirstOrDefault().roomTypes.Title,
                                                 HotelStar = h.FirstOrDefault().hotel.Star,
                                                 hotelProperties = dbContext.HotelPropertyConnects.Join(dbContext.HotelProperties, k => k.HotelPropertyId, p =>         p.HotelPropertyId, (p, k) => new { p, k }).ToList().Where(x => x.p.HotelId == h.FirstOrDefault().hotel.HotelId).Select(c => new HotelProperties()
                                                 {
                                                     HotelPropertyId = c.k.HotelPropertyId,
                                                     Title = c.k.Title,
                                                     Icon = c.k.Icon

                                                 }).ToList()

                                             }).ToList();

            }

            if (!Iscity)
            {
                hotelSearchList = (from hotel in dbContext.Hotels
                                   join city in dbContext.TownV10 on hotel.CityId equals city.TownId
                                   join hotelPhotos in dbContext.HotelPhotos on hotel.HotelId equals hotelPhotos.HotelId
                                   join hotelRooms in dbContext.HotelRooms on hotel.HotelId equals hotelRooms.HotelId
                                   join roomTypes in dbContext.RoomTypes on hotelRooms.RoomTypeId equals roomTypes.RoomTypeId
                                   where hotel.Title.ToLower().Contains(hotelNameOrCity.ToLower()) && hotelPhotos.OrderNumber == 1 && roomTypes.Title == "Standart Oda"
                                   select new { hotel, hotelPhotos, city, roomTypes, hotelRooms }).ToList()
                                   .GroupBy(x => x.hotel.HotelId).Select(h => new HotelSearchItem()
                                   {
                                       CityName = h.FirstOrDefault().city.TownName,
                                       CountryName = "Türkiye",
                                     //  Day = (checkoutDateKey.Day - checkinDateKey.Day) - 1,
                                       Description = h.FirstOrDefault().hotel.Description,
                                       HotelId = h.FirstOrDefault().hotel.HotelId,
                                       HotelImage = h.FirstOrDefault().hotelPhotos.Photo,
                                       HotelName = h.FirstOrDefault().hotel.Title,
                                       Price = h.FirstOrDefault().hotelRooms.Price,
                                       RoomName = h.FirstOrDefault().roomTypes.Title,
                                       HotelStar = h.FirstOrDefault().hotel.Star,
                                       hotelProperties = dbContext.HotelPropertyConnects.Join(dbContext.HotelProperties, k => k.HotelPropertyId, p => p.HotelPropertyId, (p, k) => new { p, k }).ToList().Where(x => x.p.HotelId == h.FirstOrDefault().hotel.HotelId).Select(c => new HotelProperties()
                                       {
                                           HotelPropertyId = c.k.HotelPropertyId,
                                           Title = c.k.Title,
                                           Icon = c.k.Icon

                                       }).ToList()

                                   }).ToList();



            }
            HotelSearchDto hotelSearchDto = new HotelSearchDto()
            {
                HotelProperties = dbContext.HotelProperties.ToList(),
                Districts = dbContext.DistrictV10.Where(x => x.TownId == townId).Take(10).ToList(),
              //  checkinDate = checkinDateKey,
                //checkoutDate = checkoutDateKey,
                adult = adult,
                child = child,
                HotelCount = hotelSearchList.Count(),
                CityName = hotelSearchList.FirstOrDefault().CityName,
                CountryName = hotelSearchList.FirstOrDefault().CountryName,
                hotelSearchList = hotelSearchList

            };
            return View(hotelSearchDto);
        }

        [HttpPost]
        public ActionResult GetHotelDistrict(string districts, string checkin, string checkout)
        {
            DateTime checkinDate = checkin == null ? DateTime.Now : Convert.ToDateTime(checkin);
            DateTime checkoutDate = checkout == null ? DateTime.Now : Convert.ToDateTime(checkout);

            List<string> districtIdList = districts == null ? null : districts.Split(',').ToList();

            List<string> hotelPropertyId = null;

            List<string> hotelStar = null;

            int minAmount = 0;

            int maxAmount = 0;

            HotelSearchHelper helper = new HotelSearchHelper();

            List<HotelSearchItem> hotelList = helper.hotelItem(districtIdList, hotelPropertyId, hotelStar, minAmount, maxAmount, checkinDate, checkoutDate);

            return Json(hotelList);
        }

        [HttpPost]
        public ActionResult GetHotelProperty(string hotelProperties, string checkin, string checkout)
        {
            DateTime checkinDate = checkin == null ? DateTime.Now : Convert.ToDateTime(checkin);
            DateTime checkoutDate = checkout == null ? DateTime.Now : Convert.ToDateTime(checkout);

            List<string> hotelPropertiesKey = hotelProperties == null ? null : hotelProperties.Split(',').ToList();

            List<string> hotelStar = null;

            List<string> districtId = null;

            int minAmount = 0;

            int maxAmount = 0;

            HotelSearchHelper helper = new HotelSearchHelper();

            List<HotelSearchItem> hotelList = helper.hotelItem(districtId, hotelPropertiesKey, hotelStar, minAmount, maxAmount, checkinDate, checkoutDate);

            return Json(hotelList);

        }

        [HttpPost]
        public ActionResult GetHotelMinMaxAmount(string minAmount, string maxAmount, string checkin, string checkout)
        {
            DateTime checkinDate = checkin == null ? DateTime.Now : Convert.ToDateTime(checkin);
            DateTime checkoutDate = checkout == null ? DateTime.Now : Convert.ToDateTime(checkout);

            int minAmountKey = Convert.ToInt32(minAmount);

            int maxAmountKey = Convert.ToInt32(maxAmount);

            List<string> hotelStars = null;

            List<string> hotelProperties = null;

            List<string> districts = null;

            HotelSearchHelper helper = new HotelSearchHelper();

            List<HotelSearchItem> hotelList = helper.hotelItem(districts, hotelProperties, hotelStars, minAmountKey, maxAmountKey, checkinDate, checkoutDate);

            return Json(hotelList);
        }

        [HttpPost]
        public ActionResult GetHotelStar(string hotelStars, string checkin, string checkout)
        {
            DateTime checkinDate = checkin == null ? DateTime.Now : Convert.ToDateTime(checkin);
            DateTime checkoutDate = checkout == null ? DateTime.Now : Convert.ToDateTime(checkout);

            List<string> hotelStarsKey = hotelStars == null ? null : hotelStars.Split(',').ToList();

            List<string> hotelProperties = null;

            List<string> districts = null;

            int minAmount = 0;

            int maxAmount = 0;

            HotelSearchHelper helper = new HotelSearchHelper();

            List<HotelSearchItem> hotelList = helper.hotelItem(districts, hotelProperties, hotelStarsKey, minAmount, maxAmount, checkinDate, checkoutDate);

            return Json(hotelList);
        }
    }
}