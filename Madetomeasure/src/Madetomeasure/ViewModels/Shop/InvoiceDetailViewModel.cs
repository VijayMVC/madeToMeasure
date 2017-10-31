using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.ViewModels.Shop
{
    public class InvoiceDetailViewModel
    {
        public int InvoiceId { get; set; }
        public int StitchingJobId { get; set; }
        public int Price { get; set; }
        public string job_type { get; set; }

    }
}
