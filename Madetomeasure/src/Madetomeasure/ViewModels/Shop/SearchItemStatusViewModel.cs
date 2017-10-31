using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.ViewModels.Shop
{
    public class SearchItemStatusViewModel
    {
        [Required(ErrorMessage = "Please enter an item code")]
        [Display(Name = "Item Code")]
        public int ItemCode { get; set; }
    }
}
