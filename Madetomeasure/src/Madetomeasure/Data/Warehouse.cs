using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class Warehouse
    {
        public int WarehouseCode { get; set; }
        public int AssociatedStitchingUnitCode { get; set; }

        public virtual BusinessEntity AssociatedStitchingUnitCodeNavigation { get; set; }
        public virtual BusinessEntity WarehouseCodeNavigation { get; set; }
    }
}
