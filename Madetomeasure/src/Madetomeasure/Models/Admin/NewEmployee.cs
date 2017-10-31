using Madetomeasure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.Admin
{


    public class NewEmployee
    {
        [Required(ErrorMessage = "Please enter your UserId")]
        [Display(Name = "UserId")]
        [EmailAddress]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Please enter your Full name")]
        [Display(Name = "Name")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Characters are not allowed.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your Confirm Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        public string CPassword { get; set; }

        [Required(ErrorMessage = "Please select your Designation")]
        [Display(Name = "Designation")]
        public string designation { get; set; }

        [Required(ErrorMessage = "Please enter your Joining data")]
        [DataType(DataType.DateTime)]
        [Display(Name = "JoiningDate")]
        public DateTime JoiningDate { get; set; }

        [Required(ErrorMessage = "Please select your Designation")]
        [Display(Name = "Designation")]
        public string  WorksAt { get; set; }
        public async Task addEmployee(MadeToMeasureContext context)
        {

            BusinessEntity b = new BusinessEntity();
           // b.EntityAddress = address;
           // b.EntityType = 3;

          //  context.BusinessEntity.Add(b);
            await context.SaveChangesAsync();
        }
    }
}
