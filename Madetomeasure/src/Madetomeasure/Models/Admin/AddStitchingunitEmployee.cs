using Madetomeasure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.Admin
{
    public class AddStitchingunitEmployee
    {
        [Required(ErrorMessage = "Please enter your Full name")]
        [Display(Name = "Name")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Characters are not allowed.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your Department")]
        [Display(Name = "Department")]
         public string Department { get; set; }

        [Required(ErrorMessage = "Please enter your Stitchingunit")]
        [Display(Name = "Stitchingunit")]
       public int StitchingunitId { get; set; }
        public async Task AddStunitEmployee(MadeToMeasureContext context, string name, int id,int duty )
        {

            StitchingUnitEmployee st = new StitchingUnitEmployee();
            st.Name = name;
            st.WarehouseId = id;
            st.DepartmentId = duty;

            // b.EntityAddress = address;
            // ;/ b.EntityType = 3;

            context.StitchingUnitEmployee.Add(st);
            await context.SaveChangesAsync();
        }
    }
}
