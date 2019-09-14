using HotelBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBooking.Dto
{
    public class HotelDetailDto
    {
        public string hotelName { get; set; }
        public int hotelId { get; set; }
        public List<HotelPhotos> hotelPhotos { get; set; }
        public int rate { get; set; }
        public List<HotelRoomDetail> hotelRooms { get; set; }
        public List<HotelProperties> hotelProperties { get; set; }
        public List<CommentItem> hotelComments { get; set; }
        public string Description { get; set; }
        public string checkin { get; set; }
        public string checkout { get; set; }
        public GuestItem guests { get; set; }
        public RoomTypes roomTypes { get; set; }
        public int nightCount { get; set; }
        public decimal? Price { get; set; }
        public int HotelRoomId { get; set; }
    }
    public class CommentItem
    {
        public string Comment { get; set; }
        public string NameSurname { get; set; }
        public string Title { get; set; }
        public decimal? Rate { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int HotelId { get; set; }
    }

    public class HotelRoomDetail
    {
        public int HotelRoomId { get; set; }
        public string RoomType { get; set; }
        public decimal? Price { get; set; }
        public int? AvailableCount { get; set; }
        public List<RoomProperties> roomproperties { get; set; }
        public int? BedCount { get; set; }
        public int? PersonCount { get; set; }
    }

    public class GuestItem
    {
        public int adultCount { get; set; }
        public int childCount { get; set; }
    }
}