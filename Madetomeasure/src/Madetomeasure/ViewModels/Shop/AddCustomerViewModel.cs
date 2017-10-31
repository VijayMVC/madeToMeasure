using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.ViewModels.Shop
{
    public class AddCustomerViewModel
    {

        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone Number must be numeric")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
