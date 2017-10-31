using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class BusinessEntity
    {
        public BusinessEntity()
        {
            Invoice = new HashSet<Invoice>();
            Item = new HashSet<Item>();
            ShopAssociatedWarehouseCodeNavigation = new HashSet<Shop>();
            ShopShopCodeNavigation = new HashSet<Shop>();
            ShopOrder = new HashSet<ShopOrder>();
            StitchingJob = new HashSet<StitchingJob>();
            StitchingUnitEmployee = new HashSet<StitchingUnitEmployee>();
            Users = new HashSet<Users>();
            WarehouseAssociatedStitchingUnitCodeNavigation = new HashSet<Warehouse>();
            WarehouseWarehouseCodeNavigation = new HashSet<Warehouse>();
        }

        public int EntityCode { get; set; }
        public int EntityType { get; set; }
        public string EntityAddress { get; set; }

        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<Item> Item { get; set; }
        public virtual ICollection<Shop> ShopAssociatedWarehouseCodeNavigation { get; set; }
        public virtual ICollection<Shop> ShopShopCodeNavigation { get; set; }
        public virtual ICollection<ShopOrder> ShopOrder { get; set; }
        public virtual ICollection<StitchingJob> StitchingJob { get; set; }
        public virtual ICollection<StitchingUnitEmployee> StitchingUnitEmployee { get; set; }
        public virtual ICollection<Users> Users { get; set; }
        public virtual ICollection<Warehouse> WarehouseAssociatedStitchingUnitCodeNavigation { get; set; }
        public virtual ICollection<Warehouse> WarehouseWarehouseCodeNavigation { get; set; }
        public virtual BusinessEntityType EntityTypeNavigation { get; set; }
    }
}
