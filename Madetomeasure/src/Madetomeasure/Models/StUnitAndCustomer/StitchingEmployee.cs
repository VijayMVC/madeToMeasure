using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.StUnitAndCustomer
{
    public class StitchingEmployee
    {
        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The password must be atleat 6 characters long.", MinimumLength = 6)]
        public string Name { get; set; }
    }
}
