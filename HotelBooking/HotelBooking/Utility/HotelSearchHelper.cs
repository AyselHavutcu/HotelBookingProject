using HotelBooking.Dto;
using HotelBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBooking.Utility
{
    public class HotelSearchHelper
    {
        HotelBookingDBEntities dbContext = new HotelBookingDBEntities();

        public List<HotelSearchItem> hotelItem(List<string> districts, List<string> hotelProperties, List<string> hotelStars, int minAmount, int maxAmount, DateTime checkinDate, DateTime checkoutDate)
        {
            
            List<HotelSearchItem> hotelList = (from hotel in dbContext.Hotels
                                               join city in dbContext.TownV10 on hotel.CityId equals city.TownId
                                               join district in dbContext.DistrictV10 on city.TownId equals district.TownId
                                               join hotelPhotos in dbContext.HotelPhotos on hotel.HotelId equals hotelPhotos.HotelId
                                               join hotelRooms in dbContext.HotelRooms on hotel.HotelId equals hotelRooms.HotelId
                                               join roomTypes in dbContext.RoomTypes on hotelRooms.RoomTypeId equals roomTypes.RoomTypeId
                                               where hotelPhotos.OrderNumber == 1 && roomTypes.Title == "Standart Oda"
                                               select new { hotel, hotelPhotos, city, district, roomTypes, hotelRooms }
                                             ).ToList().GroupBy(x => x.hotel.HotelId).Select(h => new HotelSearchItem()
                                             {
                                                 DistrictId = h.FirstOrDefault().hotel.DistrictId,
                                                 CityName = h.FirstOrDefault().city.TownName,
                                                 CountryName = "Türkiye",
                                                 Day = (checkoutDate.Day - checkinDate.Day) - 1,
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

         
            if (districts != null)
            {
                hotelList = hotelList.Where(t => districts.Contains(t.DistrictId.ToString())).ToList();
            }

            if (hotelProperties != null)
            {
                hotelList = hotelList.Where(x => x.hotelProperties.Any(item => hotelProperties.Contains(item.HotelPropertyId.ToString()))).ToList();
            }

            if (hotelStars != null)
            {
                hotelList = hotelList.Where(t => hotelStars.Contains(t.HotelStar.ToString())).ToList();
            }

            if (minAmount >= 0 && maxAmount > 0)
            {
                hotelList = hotelList.Where(x => x.Price >= minAmount && x.Price <= maxAmount).ToList();
            }

            if (districts != null && hotelProperties != null)
            {
                hotelList = hotelList.Where(t => districts.Contains(t.DistrictId.ToString()) && t.hotelProperties.Any(item => hotelProperties.Contains(item.HotelPropertyId.ToString()))).ToList();
            }

            if (districts != null && hotelStars != null)
            {
                hotelList = hotelList.Where(t => districts.Contains(t.DistrictId.ToString()) && hotelStars.Contains(t.HotelStar.ToString())).ToList();
            }

            if (districts != null && minAmount >= 0 && maxAmount > 0)
            {
                hotelList = hotelList.Where(t => districts.Contains(t.DistrictId.ToString()) && t.Price >= minAmount && t.Price <= maxAmount).ToList();
            }

            if (hotelProperties != null && hotelStars != null)
            {
                hotelList = hotelList.Where(t => t.hotelProperties.Any(item => hotelProperties.Contains(item.HotelPropertyId.ToString())) && hotelStars.Contains(t.HotelStar.ToString())).ToList();
            }

            if (hotelProperties != null && minAmount >= 0 && maxAmount > 0)
            {
                hotelList = hotelList.Where(t => t.hotelProperties.Any(item => hotelProperties.Contains(item.HotelPropertyId.ToString())) && t.Price >= minAmount && t.Price <= maxAmount).ToList();
            }

            if (hotelStars != null && minAmount >= 0 && maxAmount > 0)
            {
                hotelList = hotelList.Where(t => hotelStars.Contains(t.HotelStar.ToString()) && t.Price >= minAmount && t.Price <= maxAmount).ToList();
            }

            List<HotelSearchItem> hotelSearchList = hotelList;

            return hotelSearchList;

        }
    }
}