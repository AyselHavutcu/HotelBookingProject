using HotelBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBooking.Dto
{
    public class AccountDetailDto
    {
        public Customers customerItem { get; set; }

        public List<CountryV10> countryV10s { get; set; }

        public List<TownV10> townV10s { get; set; }
    }
}