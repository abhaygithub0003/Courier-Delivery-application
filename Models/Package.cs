using System;
using System.ComponentModel.DataAnnotations;

namespace Courier_Tracking_and_Delivery_System.Models
{
    public class Package
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pickup address is required")]
        public string PickupAddress { get; set; }

        [Required(ErrorMessage = "Delivery address is required")]
        public string DeliveryAddress { get; set; }

        [Required(ErrorMessage = "Package type is required")]
        public string PackageType { get; set; }

        [Required(ErrorMessage = "Preferred delivery time is required")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime PreferredDeliveryTime { get; set; }

        [Required(ErrorMessage = "Delivery by is required")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryBy { get; set; }

       
    }
}

