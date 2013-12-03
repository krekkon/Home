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
        public virtual string CarNumber { get; set; }

        [Required]
        public virtual int Owners { get; set; }

        [Required]
        public virtual int CarDealerId { get; set; }

        [Required]
        [StringLength(200)]
        public virtual string CarDealerName { get; set; }
    }
}