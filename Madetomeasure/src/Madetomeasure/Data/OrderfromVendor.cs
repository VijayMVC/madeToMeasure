using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class OrderfromVendor
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public int WarehouseManager { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string BrandName { get; set; }
        public string VendorName { get; set; }
        public string UnitofMeasure { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }

        public virtual Category Category { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        public virtual Users WarehouseManagerNavigation { get; set; }
    }
}
