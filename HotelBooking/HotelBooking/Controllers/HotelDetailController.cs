using HotelBooking.Dto;
using HotelBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelBooking.Controllers
{
    public class HotelDetailController : Controller
    {
        // GET: HotelDetail
        HotelBookingDBEntities dbContext = new HotelBookingDBEntities();
        public ActionResult Index(string Id)
        {
            int HotelId = Convert.ToInt32(Id);

            Session["HotelId"] = HotelId;

            Hotels hotelItem = dbContext.Hotels.Where(x => x.HotelId == HotelId).FirstOrDefault();

            var hotelProperties = from hotelPropertiesConnect in dbContext.HotelPropertyConnects
                                  join hotel in dbContext.Hotels on hotelPropertiesConnect.HotelId equals hotel.HotelId
                                  join hotelProperty in dbContext.HotelProperties on hotelPropertiesConnect.HotelPropertyId equals hotelProperty.HotelPropertyId
                                  where hotel.HotelId == hotelItem.HotelId
                                  select hotelProperty;

            var hotelComments = from hotelComment in dbContext.HotelComments
                                join customer in dbContext.Customers on hotelComment.CustomerId equals customer.CustomerId
                                join hotel in dbContext.Hotels on hotelComment.HotelId equals hotel.HotelId
                                where hotelComment.HotelId == hotelItem.HotelId
                                select new CommentItem()
                                {
                                    HotelId = hotel.HotelId,
                                    NameSurname = customer.NameSurname,
                                    Comment = hotelComment.Comment,
                                    InsertedDate = hotelComment.InsertedDate,
                                    Rate = hotelComment.Rating,
                                    Title = hotelComment.Title
                                };

            var hotelRoomType = from hotelRoomValue in dbContext.HotelRooms
                                join roomType in dbContext.RoomTypes on hotelRoomValue.RoomTypeId equals roomType.RoomTypeId
                                where roomType.Title == "Standart Oda" && hotelRoomValue.HotelId == hotelItem.HotelId
                                select new { roomType, hotelRoomValue };


            var hotelRoom = (from roomPropertyConnectKey in dbContext.RoomPropertyConnects
                             join roomPropertyKey in dbContext.RoomProperties on roomPropertyConnectKey.RoomPropertyId equals roomPropertyKey.RoomPropertyId
                             join hotelRoomKey in dbContext.HotelRooms on roomPropertyConnectKey.RoomId equals hotelRoomKey.HotelRoomId
                             join RoomTypeKey in dbContext.RoomTypes on hotelRoomKey.RoomTypeId equals RoomTypeKey.RoomTypeId
                             where hotelRoomKey.HotelId == hotelItem.HotelId
                             select new { hotelRoomKey, RoomTypeKey }).ToList().GroupBy(x => x.hotelRoomKey.HotelRoomId).Select(s => new HotelRoomDetail() {

                                 AvailableCount = s.First().hotelRoomKey.AvailableCount,
                                 BedCount = s.First().RoomTypeKey.BedCount,
                                 PersonCount = s.First().RoomTypeKey.PersonCount,
                                 Price = s.First().hotelRoomKey.Price,
                                 RoomType = s.First().RoomTypeKey.Title,
                                 HotelRoomId = s.First().hotelRoomKey.HotelRoomId,
                                 roomproperties = dbContext.RoomPropertyConnects.Join(dbContext.RoomProperties, k => k.RoomPropertyId, p => p.RoomPropertyId, (p, k) => new { p, k }).ToList().Where(x => x.p.RoomId == s.First().hotelRoomKey.HotelRoomId).Select(c => new RoomProperties()
                                 {
                                     RoomPropertyId=c.k.RoomPropertyId,
                                     Title = c.k.Title

                                 }).ToList()

                             }).ToList();

            GuestItem guestItems = new GuestItem();

            if (Session["adult"] != null || Session["child"] != null)
            {
                guestItems.adultCount = Convert.ToInt32(Session["adult"]);
                guestItems.childCount = Convert.ToInt32(Session["child"]);

            }

            HotelDetailDto hotelDetailDto = new HotelDetailDto()
            {
                Description = hotelItem.Description,
                hotelComments = hotelComments.ToList(),
                hotelId = hotelItem.HotelId,
                hotelName = hotelItem.Title,
                hotelPhotos = dbContext.HotelPhotos.Where(x => x.HotelId == hotelItem.HotelId).ToList(),
                hotelRooms = hotelRoom,
                hotelProperties = hotelProperties.ToList(),
                rate = Convert.ToInt32(hotelItem.Rating),
                guests = guestItems,
                roomTypes = hotelRoomType.FirstOrDefault().roomType,
                Price = hotelRoomType.FirstOrDefault().hotelRoomValue.Price,
                HotelRoomId = hotelRoomType.FirstOrDefault().hotelRoomValue.HotelRoomId
            };

            if (Session["checkin"] != null || Session["checkout"] != null)
            {
                hotelDetailDto.checkin = Convert.ToDateTime(Session["checkin"]).Date.ToShortDateString();
                hotelDetailDto.checkout = Convert.ToDateTime(Session["checkout"]).Date.ToShortDateString();
                hotelDetailDto.nightCount = Convert.ToDateTime(Session["checkout"]).Date.Day - Convert.ToDateTime(Session["checkin"]).Date.Day;
            }

            return View(hotelDetailDto);
        }
    }
}