using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class DepartmentType
    {
        public DepartmentType()
        {
            StitchingUnitDepartmentHead = new HashSet<StitchingUnitDepartmentHead>();
            StitchingUnitEmployee = new HashSet<StitchingUnitEmployee>();
        }

        public int Id { get; set; }
        public string DepartmentName { get; set; }

        public virtual ICollection<StitchingUnitDepartmentHead> StitchingUnitDepartmentHead { get; set; }
        public virtual ICollection<StitchingUnitEmployee> StitchingUnitEmployee { get; set; }
    }
}
