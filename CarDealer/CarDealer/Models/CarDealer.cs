using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CarDealerProject.Models
{
    public class CarDealer
    {
        [HiddenInput(DisplayValue = false)]
        public virtual int Id { get; set; }

        [Required]
        [StringLength(200)]
        public virtual string Name { get; set; }

        [Display(Name = "Parking places")]
        [Required]
        [Range(0, int.MaxValue)]
        public virtual int ParkingPlaces { get; set; }

        //Validate? hun or foreign...
        [StringLength(50)]
        public virtual string Telephone { get; set; }

        [StringLength(200)]
        [EmailAddress]
        public virtual string Email { get; set; }

        [StringLength(200)]
        public virtual string Country { get; set; }

        [StringLength(200)]
        public virtual string City { get; set; }

        [StringLength(10)]
        [Display(Name = "Zip code")]
        public virtual string ZipCode { get; set; }

        [StringLength(200)]
        public virtual string Street { get; set; }

        [StringLength(50)]
        [Display(Name = "Street number")]
        public virtual string StreetNumber { get; set; }
    }
}