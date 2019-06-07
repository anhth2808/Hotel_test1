using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel_test1.Models
{
    public class RoomtypeRentViewModel
    {
        public string RoomType_id { get; set; }
        public string RType { get; set; }
        public string Descriptions { get; set; }
        public string Images { get; set; }
        public string Views { get; set; }
        public int Bed { get; set; }
        public int MaxPerson { get; set; }
        public int Size { get; set; }
        public int Price { get; set; }
    }
}