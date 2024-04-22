using System;

namespace Courier_Tracking_and_Delivery_System.Models
{
    public class PackageStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public string Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string Remarks { get; set; }
        public int PackageId { get; set; }
        public Package Package { get; set; }
       
    }
}

