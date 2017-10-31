using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class ShopOrder
    {
        public int Id { get; set; }
        public int ShopCode { get; set; }
        public int? ItemCode { get; set; }
        public int Quantity { get; set; }
        public int? WarehouseStatus { get; set; }

        public virtual Item ItemCodeNavigation { get; set; }
        public virtual BusinessEntity ShopCodeNavigation { get; set; }
    }
}
