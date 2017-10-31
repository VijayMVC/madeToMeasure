using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class StitchingUnitEmployee
    {
        public StitchingUnitEmployee()
        {
            ProductionActivity = new HashSet<ProductionActivity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int WarehouseId { get; set; }
        public int DepartmentId { get; set; }

        public virtual ICollection<ProductionActivity> ProductionActivity { get; set; }
        public virtual DepartmentType Department { get; set; }
        public virtual BusinessEntity Warehouse { get; set; }
    }
}
