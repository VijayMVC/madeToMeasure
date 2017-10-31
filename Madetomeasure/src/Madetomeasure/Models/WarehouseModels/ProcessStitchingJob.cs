using Madetomeasure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.WarehouseModels
{
    public class ProcessStitchingJob
    {
        MadeToMeasureContext db;

        public ProcessStitchingJob(MadeToMeasureContext dbContext)
        {
            db = dbContext;
        }


        public async Task  doOrderProcessing(int jobId)
        {




            var jobDetail = from stitchingjob in db.StitchingJob
                            where stitchingjob.JobId == jobId
                            select stitchingjob;



            foreach (var x in jobDetail)
            {
                x.WarehouseStatus = 1;
            }

            await db.SaveChangesAsync();



        }



    }
}
