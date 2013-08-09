using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage="Please enter a name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter the first address line")]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        [Required(ErrorMessage="Plesae enter a suburb name")]
        public string Suburb { get; set; }
        [Required(ErrorMessage="Please enter a state name")]
        public string State { get; set; }
        [Required(ErrorMessage="Please enter a postcode")]
        public string Postcode { get; set; }
        public bool GiftWrap { get; set; }
    }
}
