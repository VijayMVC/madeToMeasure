using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class JobType
    {
        public JobType()
        {
            StitchingJob = new HashSet<StitchingJob>();
        }

        public int Id { get; set; }
        public string JobName { get; set; }

        public virtual ICollection<StitchingJob> StitchingJob { get; set; }
    }
}
