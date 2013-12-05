using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CarDealerProject.Models.Attributes;

namespace CarDealerProject.Models
{
    public class Car
    {
        [HiddenInput(DisplayValue = false)]
        public virtual int Id { get; set; }

        [Required]
        [StringLength(50)]
        public virtual string Brand { get; set; }

        [Required]
        [StringLength(50)]
        public virtual string Model { get; set; }

        [Required]
        [CarYearAttribute("1886")]
        public virtual int Year { get; set; }

        [BirthDate]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Manufacture date")]
        public virtual DateTime ManufactureDate { get; set; }

        [Required]
        [StringLength(200)]
        public virtual string State { get; set; }

        [StringLength(1000)]
        public virtual string Description { get; set; }

        [Required]
        [StringLength(50)]
        public virtual string Color { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Reg. number")]
        public virtual string CarNumber { get; set; }

        [Required]
        public virtual int Owners { get; set; }

        [Required]
        [Display(Name = "Dealership Id")]
        [Range(0, int.MaxValue)]
        public virtual int CarDealerId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Dealership name")]
        public virtual string CarDealerName { get; set; }
    }
}