using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.WarehouseModels
{
    public class StitchingOrderDetails
    {
        public int OrderId { get; set; }
        public string ShopCode { get; set; }
        public string JobType { get; set; }
        public string OrderDate { get; set; }
        public string ExpectedDeliveryDate { get; set; }
        public int FabricFromCustomer { get; set; }

    }
}
