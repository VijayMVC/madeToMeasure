using Madetomeasure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.StUnitAndCustomer
{
    public class SaveProductionActivity
    {
        public async Task logProductionActivity(MadeToMeasureContext _context, int employeeid, int stitchingjodid, int progressstatus) {
            ProductionActivity p = new ProductionActivity();
            p.StitchingUnitEmployeeId = employeeid;
            p.TimeStamp = DateTime.Now;
            p.StitchingJobId = stitchingjodid;
            p.ProgressStatus = progressstatus;

            _context.ProductionActivity.Add(p);
            if (progressstatus == 5)
            {
                p.ProgressStatus = 6;
            }
            _context.ProductionActivity.Add(p);
            await _context.SaveChangesAsync();
        }
    }
}
