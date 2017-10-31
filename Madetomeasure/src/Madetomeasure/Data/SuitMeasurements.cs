using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class SuitMeasurements
    {
        public int StitchingJobId { get; set; }
        public double? Length { get; set; }
        public double? Chest { get; set; }
        public double? Shoulder { get; set; }
        public double? Sleeve { get; set; }
        public double? Collar { get; set; }
        public double? HalfBack { get; set; }
        public double? FullBack { get; set; }
        public double? Bicep { get; set; }
        public double? Wrist { get; set; }
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
