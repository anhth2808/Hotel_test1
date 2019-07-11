using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hotel_test1.Models
{
    public class BillDetailViewModel
    {
        public string BillPay_id { get; set; }

        [DisplayName("Ngày thanh toán")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Date { get; set; }

        public string Booking_id { get; set; }

        [DisplayName("Tên")]
        public string CustomerFirstName { get; set; }
        [DisplayName("Họ")]
        public string CustomerLastName { get; set; }

        [DisplayName("Số phòng")]
        public string Room_id { get; set; }
        [DisplayName("Loại phòng")]
        public string RType { get; set; }
        [DisplayName("Giá phòng")]
        public int Price { get; set; }

        [DisplayName("Ngày nhận phòng")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Check_in_date { get; set; }

        [DisplayName("Ngày trả phòng")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Check_out_date { get; set; }

        [DisplayName("Phương thức thanh toán")]
        public string PType { get; set; }
        [DisplayName("Tổng cộng")]
        public Nullable<int> Total { get; set; }

    }
}