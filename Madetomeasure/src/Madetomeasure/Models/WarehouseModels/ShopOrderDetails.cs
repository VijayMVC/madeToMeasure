using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.WarehouseModels
{
    public class ShopOrderDetails
    {
        public int Id { get; set; }
        public int ShopCode { get; set; }
        public int Quantity { get; set; }
        
        public String CategoryName { get; set; }
        public String SubCategoryName { get; set; }
        public String Brand { get; set; }
        public String Color { get; set; }
        public String UnitOfMeasure { get; set; }



    }
}
