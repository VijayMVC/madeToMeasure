using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class InvoiceDetails
    {
        public int InvoiceId { get; set; }
        public int StitchingJobId { get; set; }
        public int Price { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual StitchingJob StitchingJob { get; set; }
    }
}
