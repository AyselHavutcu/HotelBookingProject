using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBooking.Dto
{
    public class RegisterDto
    {
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
    }
}