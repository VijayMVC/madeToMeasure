using Madetomeasure.Data;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.WarehouseModels
{
    public class StitchingJobOrdersRetrieval
    {
        MadeToMeasureContext db;

        public StitchingJobOrdersRetrieval(MadeToMeasureContext dbContext)
        {
            db = dbContext;
        }


        public List<StitchingOrderDetails> retrieveOrders(int WorksAt)
        {


            List<StitchingOrderDetails> stitchingjobs = new List<StitchingOrderDetails>();

            var stitJobs = from shop in db.Shop
                         where shop.AssociatedWarehouseCode == WorksAt
                         join stitchingjob in db.StitchingJob on shop.ShopCode equals stitchingjob.ShopCode
                         where stitchingjob.WarehouseStatus == 0
                         join jobtype in db.JobType on stitchingjob.JobTypeId equals jobtype.Id
                         select new { OrderId = stitchingjob.JobId, ShopCode = stitchingjob.ShopCode, JobType = jobtype.JobName, OrderDate = stitchingjob.OrderDate, ExpectedDate = stitchingjob.ExpectedDate, FabricFromFabric = stitchingjob.FabricfromCustomer };
            


            foreach (var x in stitJobs)
            {
                stitchingjobs.Add(new StitchingOrderDetails { OrderId = x.OrderId, ShopCode = "Shop" +  x.ShopCode.ToString(), JobType = x.JobType, OrderDate = x.OrderDate.Month.ToString() +"/" + x.OrderDate.Day.ToString() + "/" + x.OrderDate.Year.ToString()   , ExpectedDeliveryDate = x.ExpectedDate.Value.Month.ToString() + "/" + x.ExpectedDate.Value.Day.ToString() + "/" + x.ExpectedDate.Value.Year.ToString(), FabricFromCustomer = x.FabricFromFabric });

            }



            return stitchingjobs;



        }

     


    }
}
