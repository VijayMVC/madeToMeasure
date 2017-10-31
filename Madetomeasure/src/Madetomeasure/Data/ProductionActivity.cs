using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class ProductionActivity
    {
        public int Id { get; set; }
        public int StitchingJobId { get; set; }
        public int StitchingUnitEmployeeId { get; set; }
        public DateTime TimeStamp { get; set; }
        public int ProgressStatus { get; set; }

        public virtual StitchingJob StitchingJob { get; set; }
        public virtual StitchingUnitEmployee StitchingUnitEmployee { get; set; }
    }
}
