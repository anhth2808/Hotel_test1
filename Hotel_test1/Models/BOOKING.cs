//------------------------------------------------------------------------------
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
    
    public partial class BOOKING
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BOOKING()
        {
            this.BILLs = new HashSet<BILL>();
        }
    
        public string Booking_id { get; set; }
        public System.DateTime Check_in_date { get; set; }
        public System.DateTime Check_out_date { get; set; }
        public string Customer_id { get; set; }
        public string Room_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BILL> BILLs { get; set; }
        public virtual CUSTOMER CUSTOMER { get; set; }
        public virtual ROOM ROOM { get; set; }
    }
}
