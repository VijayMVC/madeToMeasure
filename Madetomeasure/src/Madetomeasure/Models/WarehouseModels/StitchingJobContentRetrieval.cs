using Madetomeasure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.WarehouseModels
{
    public class StitchingJobContentRetrieval
    {
        MadeToMeasureContext db;

        public StitchingJobContentRetrieval(MadeToMeasureContext dbContext)
        {
            db = dbContext;
        }


        public StitchingJobContentDetails retrieveOrders(int jobId)
        {
            

            StitchingJobContentDetails stitchingJOb = new StitchingJobContentDetails();
            stitchingJOb.orderDetails = new StitchingOrderDetails();
            List<ItemDesc> itemDescs = new List<ItemDesc>();

            var itemDetails = from stitchingjobdetails in db.StitchingJobDetails
                           where stitchingjobdetails.JobId == jobId
                           join stitchingjob in db.StitchingJob on stitchingjobdetails.JobId equals stitchingjob.JobId
                           join jobtype in db.JobType on stitchingjob.JobTypeId equals jobtype.Id
                           join item in db.Item on stitchingjobdetails.ItemCode equals item.ItemCode
                           join category in db.Category on item.CategoryId equals category.CategoryId
                           join subcategory in db.SubCategory on item.SubCategoryId equals subcategory.SubCategoryId
                           select new { OrderId = stitchingjob.JobId, ShopCode = stitchingjob.ShopCode, JobType = jobtype.JobName, OrderDate = stitchingjob.OrderDate, ExpectedDate = stitchingjob.ExpectedDate,  Category = category.CategoryName, SubCategory = subcategory.SubCategoryName, Brand = item.BrandName, UnitOfMeasure = item.UnitofMeasure, Color = item.Color, Quantity = stitchingjobdetails.Quantity };

       
            
            foreach (var x in itemDetails)
            {
                stitchingJOb.orderDetails.OrderId = x.OrderId;
                stitchingJOb.orderDetails.ShopCode ="Shop" + x.ShopCode.ToString();
                stitchingJOb.orderDetails.JobType = x.JobType;
                stitchingJOb.orderDetails.OrderDate = x.OrderDate.Month.ToString() + "/" + x.OrderDate.Day.ToString() + "/" + x.OrderDate.Year.ToString();
                stitchingJOb.orderDetails.ExpectedDeliveryDate = x.ExpectedDate.Value.Month.ToString() + "/" + x.ExpectedDate.Value.Day.ToString() + "/" + x.ExpectedDate.Value.Year.ToString();
                
                itemDescs.Add(new ItemDesc { Category = x.Category, SubCategory = x.SubCategory, Brand = x.Brand, Color = x.Color, Quantity = x.Quantity.ToString()+" "+ x.UnitOfMeasure  });

            }

            stitchingJOb.itemDescription = itemDescs;



            return stitchingJOb;



        }



    }
}
