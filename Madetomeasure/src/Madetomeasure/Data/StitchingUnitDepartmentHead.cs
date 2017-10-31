using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class StitchingUnitDepartmentHead
    {
        public int Id { get; set; }
        public int AssociatedDepartmentId { get; set; }

        public virtual DepartmentType AssociatedDepartment { get; set; }
        public virtual Users IdNavigation { get; set; }
    }
}
