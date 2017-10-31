using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Madetomeasure.Data;

namespace Madetomeasure.Models.Shop
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class HistoryItem
    {
        private readonly RequestDelegate _next;
        public StitchingJob sj { get; set; }
        public Invoice invoice { get; set; }
        public HistoryItem(StitchingJob stitchingjob, Invoice inv)
        {
            sj = stitchingjob;
            invoice = inv;
        }
    }
}
