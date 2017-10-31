using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.WarehouseModels
{
    public class ItemDesc
    {
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string Quantity { get; set; }
        public string VendorName { get; set; }
        public string UnitOfMeasure { get; set; }
        public int Id { get; set; }

    }
}
