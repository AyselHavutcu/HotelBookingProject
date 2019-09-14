using HotelBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBooking.Dto
{
    public class HotelCheckoutDto
    {
        public string hotelName { get; set; }
        public int hotelId { get; set; }
        public HotelPhotos hotelPhotos { get; set; }
        public string HotelAddress { get; set; }
        public int rate { get; set; }
        public List<HotelRoomDetailItem> hotelRooms { get; set; }
        public List<HotelProperties> hotelProperties { get; set; }
        public string Description { get; set; }
        public string checkin { get; set; }
        public string checkout { get; set; }
        public Guests guests { get; set; }
        public RoomTypes roomTypes { get; set; }
        public int nightCount { get; set; }
        public decimal? Price { get; set; }
        public int HotelRoomId { get; set; }
        public List<CountryV10> Countries { get; set; }
        public List<TownV10> Cities { get; set; }
    }
    
    public class HotelRoomDetailItem
    {
        public int HotelRoomId { get; set; }
        public string RoomType { get; set; }
        public decimal? Price { get; set; }
        public int? AvailableCount { get; set; }
        public List<RoomProperties> roomproperties { get; set; }
        public int? BedCount { get; set; }
        public int? PersonCount { get; set; }
    }

    public class Guests
    {
        public int adultCount { get; set; }
        public int childCount { get; set; }
    }
}