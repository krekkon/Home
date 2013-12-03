using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CarDealerProject.Models
{
    public class Address
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [StringLength(200)]
        public string Country { get; set; }

        [StringLength(10)]
        public string ZipCode { get; set; }

        [StringLength(200)]
        public string City { get; set; }

        [StringLength(200)]
        public string Street { get; set; }

        [StringLength(50)]
        public string StreetNumber { get; set; }
    }
}