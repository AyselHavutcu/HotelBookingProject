//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HotelBooking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HotelComments
    {
        public int HotelCommentId { get; set; }
        public Nullable<int> HotelId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public Nullable<decimal> Rating { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
    
        public virtual Customers Customer { get; set; }
        public virtual Hotels Hotel { get; set; }
    }
}
