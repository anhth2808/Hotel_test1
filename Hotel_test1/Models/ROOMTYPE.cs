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

    public partial class ROOMTYPE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ROOMTYPE()
        {
            this.RENTs = new HashSet<RENT>();
            this.ROOMs = new HashSet<ROOM>();
        }

        [DisplayName("Mã loại phòng")]
        public string RoomType_id { get; set; }
        [DisplayName("Loại phòng")]
        public string RType { get; set; }
        [DisplayName("Mô tả")]
        public string Descriptions { get; set; }
        [DisplayName("ẢNH")]
        public string Images { get; set; }
        [DisplayName("View")]
        public string Views { get; set; }
        [DisplayName("Số giường")]
        public int Bed { get; set; }
        [DisplayName("Số người")]
        public int MaxPerson { get; set; }
        [DisplayName("Diện tích")]
        public int Size { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RENT> RENTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ROOM> ROOMs { get; set; }
    }
}
