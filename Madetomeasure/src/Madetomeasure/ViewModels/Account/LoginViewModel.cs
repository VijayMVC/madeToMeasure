using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter your User ID")]
        [Display(Name = "Email")]
        [StringLength(100, ErrorMessage = "The email must be atleat 11 characters long.", MinimumLength = 11)]
        public string Email { get; set; }


        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The password must be atleat 6 characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }


    }
}
