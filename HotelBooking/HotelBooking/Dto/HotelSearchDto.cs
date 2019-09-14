using HotelBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBooking.Dto
{
    public class HotelSearchDto
    {
        public List<HotelSearchItem> hotelSearchList { get; set; }
        public List<HotelProperties> HotelProperties { get; set; }
        public List<DistrictV10> Districts { get; set; }
        //public string checkinDate { get; set; }
       //public string checkoutDate { get; set; }
        public string adult { get; set; }
        public string child { get; set; }
        public int HotelCount { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }

    }
    public class HotelSearchItem
    {
        public int? DistrictId { get; set; }
        public int? HotelPropertyId { get; set; }
        public int HotelId { get; set; }
        public string RoomName { get; set; }
        public string HotelName { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string HotelImage { get; set; }
        public decimal? Price { get; set; }
        public int Day { get; set; }
        public string Description { get; set; }
        public List<HotelProperties> hotelProperties { get; set; }
        public int? HotelStar { get; set; }
    }



}