using Madetomeasure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Madetomeasure.Models.Admin
{
    public class NewStitching
    {
        public int code = 3;
        [Required(ErrorMessage = "Please Enter the address")]
        [Display(Name = "address")]
        public string address { get; set; }
        public async Task addStitching(MadeToMeasureContext context)
        {

            BusinessEntity b = new BusinessEntity();
            b.EntityAddress = address;
            b.EntityType = 3;

            context.BusinessEntity.Add(b);
            await context.SaveChangesAsync();
        }
    }
}
