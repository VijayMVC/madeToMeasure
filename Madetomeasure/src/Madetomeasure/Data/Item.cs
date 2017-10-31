using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class Item
    {
        public Item()
        {
            ShopOrder = new HashSet<ShopOrder>();
            StitchingJobDetails = new HashSet<StitchingJobDetails>();
        }

        public int ItemCode { get; set; }
        public int? WarehouseCode { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string BrandName { get; set; }
        public string VendorName { get; set; }
        public string UnitofMeasure { get; set; }
        public int Quantity { get; set; }
        public double UnitCostPrice { get; set; }
        public double PercentageProfit { get; set; }
        public string Color { get; set; }

        public virtual ICollection<ShopOrder> ShopOrder { get; set; }
        public virtual ICollection<StitchingJobDetails> StitchingJobDetails { get; set; }
        public virtual Category Category { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        public virtual BusinessEntity WarehouseCodeNavigation { get; set; }
    }
}
