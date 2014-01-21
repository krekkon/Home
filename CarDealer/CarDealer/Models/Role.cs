using System.Web.Mvc;

namespace CarDealerProject.Models
{
    [HiddenInput(DisplayValue = false)]
    public class Role
    {
        public virtual int UserId { get; set; }
        public virtual string RoleName { get; set; }
    }
}