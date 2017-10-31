using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class ShalwarMeasurements
    {
        public int StitchingJobId { get; set; }
        public double? Length { get; set; }
        public double? Waist { get; set; }
        public double? Bottom { get; set; }
        public string OtherDetails { get; set; }

        public virtual StitchingJob StitchingJob { get; set; }
    }
}
