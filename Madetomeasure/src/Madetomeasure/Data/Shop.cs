using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class Shop
    {
        public int ShopCode { get; set; }
        public int AssociatedWarehouseCode { get; set; }

        public virtual BusinessEntity AssociatedWarehouseCodeNavigation { get; set; }
        public virtual BusinessEntity ShopCodeNavigation { get; set; }
    }
}
