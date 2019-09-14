using HotelBooking.Dto;
using HotelBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelBooking.Controllers
{
    public class HotelCheckoutController : Controller
    {
        // GET: HotelCheckout
        HotelBookingDBEntities dbContext = new HotelBookingDBEntities();

        public ActionResult Index(string Id)
        {
            int HotelRoomId = Convert.ToInt32(Id);

            Session["HotelRoomId"] = HotelRoomId;

            int HotelId = 0;

            if (Session["HotelId"] != null)
            {
                HotelId = Convert.ToInt32(Session["HotelId"]);
            }

            Hotels hotelItem = dbContext.Hotels.Where(x => x.HotelId == HotelId).FirstOrDefault();

            var hotelProperties = from hotelPropertiesConnect in dbContext.HotelPropertyConnects
                                  join hotel in dbContext.Hotels on hotelPropertiesConnect.HotelId equals hotel.HotelId
                                  join hotelProperty in dbContext.HotelProperties on hotelPropertiesConnect.HotelPropertyId equals hotelProperty.HotelPropertyId
                                  where hotel.HotelId == HotelId
                                  select hotelProperty;

            var hotelRoomType = from hotelRoomValue in dbContext.HotelRooms
                                join roomType in dbContext.RoomTypes on hotelRoomValue.RoomTypeId equals roomType.RoomTypeId
                                where hotelRoomValue.HotelRoomId == HotelRoomId && hotelRoomValue.HotelId == HotelId
                                select new { roomType, hotelRoomValue };


            var hotelRoom = (from roomPropertyConnectKey in dbContext.RoomPropertyConnects
                             join roomPropertyKey in dbContext.RoomProperties on roomPropertyConnectKey.RoomPropertyId equals roomPropertyKey.RoomPropertyId
                             join hotelRoomKey in dbContext.HotelRooms on roomPropertyConnectKey.RoomId equals hotelRoomKey.HotelRoomId
                             join RoomTypeKey in dbContext.RoomTypes on hotelRoomKey.RoomTypeId equals RoomTypeKey.RoomTypeId
                             where hotelRoomKey.HotelId == HotelId
                             select new { hotelRoomKey, RoomTypeKey }).ToList().GroupBy(x => x.hotelRoomKey.HotelRoomId).AsEnumerable().Select(s => new HotelRoomDetailItem()
                             {
                                 AvailableCount = s.First().hotelRoomKey.AvailableCount,
                                 BedCount = s.First().RoomTypeKey.BedCount,
                                 PersonCount = s.First().RoomTypeKey.PersonCount,
                                 Price = s.First().hotelRoomKey.Price,
                                 RoomType = s.First().RoomTypeKey.Title,
                                 HotelRoomId = s.First().hotelRoomKey.HotelRoomId,
                                 roomproperties = dbContext.RoomPropertyConnects.Join(dbContext.RoomProperties, k => k.RoomPropertyId, p => p.RoomPropertyId, (p, k) => new { p, k }).ToList().Where(x => x.p.RoomId == s.First().hotelRoomKey.HotelRoomId).Select(c => new RoomProperties()
                                 {
                                     RoomPropertyId = c.k.RoomPropertyId,
                                     Title = c.k.Title

                                 }).ToList()

                             }).ToList();



            Guests guestItems = new Guests();

            if (Session["adult"] != null || Session["child"] != null)
            {
                guestItems.adultCount = Convert.ToInt32(Session["adult"]);
                guestItems.childCount = Convert.ToInt32(Session["child"]);

            }

            HotelCheckoutDto hotelCheckoutDto = new HotelCheckoutDto()
            {
                Description = hotelItem.Description,
                hotelId = hotelItem.HotelId,
                hotelName = hotelItem.Title,
                hotelPhotos = dbContext.HotelPhotos.Where(x => x.HotelId == hotelItem.HotelId).FirstOrDefault(),
                hotelRooms = hotelRoom,
                hotelProperties = hotelProperties.ToList(),
                rate = Convert.ToInt32(hotelItem.Rating),
                guests = guestItems,
                roomTypes = hotelRoomType.FirstOrDefault().roomType,
                Price = hotelRoomType.FirstOrDefault().hotelRoomValue.Price,
                HotelRoomId = hotelRoomType.FirstOrDefault().hotelRoomValue.HotelRoomId,
                HotelAddress = hotelItem.Address,
                Countries = dbContext.CountryV10.ToList()
            };

            if (Session["checkin"] != null || Session["checkout"] != null)
            {
                hotelCheckoutDto.checkin = Convert.ToDateTime(Session["checkin"]).Date.ToShortDateString();
                hotelCheckoutDto.checkout = Convert.ToDateTime(Session["checkout"]).Date.ToShortDateString();
                hotelCheckoutDto.nightCount = Convert.ToDateTime(Session["checkout"]).Date.Day - Convert.ToDateTime(Session["checkin"]).Date.Day;
            }


            return View(hotelCheckoutDto);
        }

        public ActionResult FillCity(string countryId)
        {
            int countyKey = Convert.ToInt32(countryId);

            List<TownV10> townList = dbContext.TownV10.Where(x => x.CountryId == countyKey).ToList();

            return Json(townList);
        }
    }
}