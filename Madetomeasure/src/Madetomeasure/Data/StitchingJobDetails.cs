using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class StitchingJobDetails
    {
        public int JobId { get; set; }
        public int ItemCode { get; set; }
        public string Quantity { get; set; }

        public virtual Item ItemCodeNavigation { get; set; }
    }
}
