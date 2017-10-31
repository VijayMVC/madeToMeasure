using Madetomeasure.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.Admin
{
    public class LinkStitchingunitware
    {
        [Required(ErrorMessage = "Please selectyour Stitchingunit  ")]
        [Display(Name = "stitchingco")]
        public int stitchingco { get; set; }

        [Required(ErrorMessage = "Please select your Warehouse ")]
        [Display(Name = "warehouseco")]
        public int warehouseco { get; set; }

        public async Task add(MadeToMeasureContext context, int stitchingcode, int warehousecode)
        {



            Warehouse warehouse = new Warehouse();
          warehouse.AssociatedStitchingUnitCode= stitchingcode;
            warehouse.WarehouseCode = warehousecode;


            context.Warehouse.Add(warehouse);
            await context.SaveChangesAsync();
        }
    }
}
