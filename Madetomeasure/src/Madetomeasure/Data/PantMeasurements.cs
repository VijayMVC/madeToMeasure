using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class PantMeasurements
    {
        public int StitchingJobId { get; set; }
        public double? Waist { get; set; }
        public double? Hip { get; set; }
        public double? Thigh { get; set; }
        public double? OutSeam { get; set; }
        public double? Inseam { get; set; }
        public double? Crotch { get; set; }
        public double? Knee { get; set; }
        public string OtherDetails { get; set; }

        public virtual StitchingJob StitchingJob { get; set; }
    }
}
