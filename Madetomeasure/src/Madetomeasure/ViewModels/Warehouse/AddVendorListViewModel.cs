using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.ViewModels.Warehouse
{
    public class AddVendorListViewModel
    {
        [Required(ErrorMessage = "Please enter the category")]
        [Display(Name = "categoryName")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid category")]
        public int categoryName { get; set; }


        [Required(ErrorMessage = "Please enter the sub-category")]
        [Display(Name = "subCategoryName")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid category")]
        public int subCategoryName { get; set; }


        [Required(ErrorMessage = "Please enter the brand")]
        [Display(Name = "brand")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Characters are not allowed.")]
        public String brand { get; set; }


        [Required(ErrorMessage = "Please enter the vendor's name")]
        [Display(Name = "vendorName")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Characters are not allowed.")]
        public String vendorName { get; set; }


        [Required(ErrorMessage = "Please enter the color")]
        [Display(Name = "color")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Characters are not allowed.")]
        public String color { get; set; }

        [Required(ErrorMessage = "Please select the unit of measure")]
        [Display(Name = "unitOfMeasure")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Characters are not allowed.")]
        public String unitOfMeasure { get; set; }


        [Required(ErrorMessage = "Please enter the quantity")]
        [Display(Name = "quantity")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid quantity")]
        public int quantity { get; set; }




    }
}
