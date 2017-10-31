using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class Category
    {
        public Category()
        {
            Item = new HashSet<Item>();
            OrderfromVendor = new HashSet<OrderfromVendor>();
            SubCategory = new HashSet<SubCategory>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

         



        public virtual ICollection<Item> Item { get; set; }
        public virtual ICollection<OrderfromVendor> OrderfromVendor { get; set; }
        public virtual ICollection<SubCategory> SubCategory { get; set; }
    }
}
