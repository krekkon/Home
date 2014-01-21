using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CarDealerProject.Models
{
    public class User 
    {
        [HiddenInput(DisplayValue = false)]
        public virtual int Id { get; set; }

        [Required]
        [Display(Name = "User name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "User name length is incorrenct (3,50)")]
        public virtual string UserName { get; set; }

        [Display(Name = "Display name")]
        [StringLength(50)]
        public virtual string DisplayName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password length is incorrenct (6,50)")]
        public virtual string Password { get; set; }

        public virtual bool PersistCookie { get; set; }
    }
}