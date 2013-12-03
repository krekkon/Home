using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CarDealerProject.Models.Attributes;

namespace CarDealerProject.Models
{
    public class Person
    {
        [HiddenInput(DisplayValue = false)]
        public virtual int Id { get; set; }

        [Required]
        [StringLength(50)]
        //TODO REGEX FOR HUN? no
        public virtual string IDCardNumber { get; set; }

        [Required]
        [StringLength(200)]
        public virtual string Name { get; set; }

        [Required]
        [BirthDate(ErrorMessage = "The date must be greater than 01/01/1886 and lower than the actual Date.")]
        public virtual DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(200)]
        public virtual string MotherName { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public virtual int AddressID { get; set; }

        [StringLength(50)]
        public virtual string Telephone { get; set; }

        [StringLength(200)]
        public virtual string Email { get; set; }
    }
}