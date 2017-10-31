using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class SubCategory
    {
        public SubCategory()
        {
            Item = new HashSet<Item>();
            OrderfromVendor = new HashSet<OrderfromVendor>();
        }

        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public string SubCategoryName { get; set; }

        public virtual ICollection<Item> Item { get; set; }
        public virtual ICollection<OrderfromVendor> OrderfromVendor { get; set; }
        public virtual Category Category { get; set; }
    }
}
