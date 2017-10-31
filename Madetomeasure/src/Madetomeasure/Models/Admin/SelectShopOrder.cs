using Madetomeasure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.Admin
{
    public class SelectShopOrder
    {
        public int shopcode { get; set; }
        public MadeToMeasureContext _context;
        public SelectShopOrder(MadeToMeasureContext context)
        {
            _context = context;
        }
        public List<StitchingJob> getreport(int shopco)
        {
            var report = from j in _context.StitchingJob
                         where j.OrderDate != null && j.ShopCode == shopco
                         select j;


            List<StitchingJob> bussinessEntitiesList = new List<StitchingJob>();
            foreach (StitchingJob b in report)
            {
                bussinessEntitiesList.Add(b);
            }
            return bussinessEntitiesList;


        }

    }
}
