﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hotel_test1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class BILL
    {
        public string Bill_id { get; set; }
        [DisplayName("Tổng tiền")]
        public Nullable<int> Total { get; set; }
        [DisplayName("Mã bill")]
        public string BillPay_id { get; set; }
        [DisplayName("Mã giá phòng")]
        public string Rent_id { get; set; }
        [DisplayName("Mã booking")]
        public string Booking_id { get; set; }

        public virtual BILLPAY BILLPAY { get; set; }
        public virtual BOOKING BOOKING { get; set; }
        public virtual RENT RENT { get; set; }
    }
}
