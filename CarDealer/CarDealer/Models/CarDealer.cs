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

        [Display(Name = "Address ID")]
        [Required]
        public virtual int AddressId { get; set; }

        [Display(Name = "Parking places")]
        [Required]
        [Range(0, int.MaxValue)]
        public virtual int ParkingPlaces { get; set; }

        //Validate?
        [StringLength(50)]
        public virtual string Telephone { get; set; }

        //Validate
        [StringLength(200)]
        public virtual string Email { get; set; }

        //Munkarend

        //public bool IsValid()
        //{
        //    if (this.GetErrors().Count == 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public List<Error> GetErrors()
        //{
        //    List<Error> Errors = new List<Error>();

        //    if (String.IsNullOrEmpty(this.Title))
        //    {
        //        Errors.Add(
        //          new Error
        //          {
        //              Description = "Title cannot be blank",
        //              Property = "Title"
        //          });
        //    }
        //    return Errors;
        //} 
    }
}