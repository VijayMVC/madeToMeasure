using Madetomeasure.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.WarehouseModels
{
    public class ShopOrdersRetrieval
    {
            MadeToMeasureContext db;

            public ShopOrdersRetrieval(MadeToMeasureContext dbContext)
            {
                db = dbContext;
            }
        

            public List<ShopOrderDetails> retrieveOrders(int WorksAt)
            {

            
            List<ShopOrderDetails> shopOrders = new List<ShopOrderDetails>();

            var orders = from shop in db.Shop
                         where shop.AssociatedWarehouseCode == WorksAt
                         join shoporder in db.ShopOrder on shop.ShopCode equals shoporder.ShopCode
                         where shoporder.WarehouseStatus == 0
                         join item in db.Item on shoporder.ItemCode equals item.ItemCode
                         where item.WarehouseCode == WorksAt
                         join category in db.Category on item.CategoryId equals category.CategoryId
                         join subcategory in db.SubCategory on item.SubCategoryId equals subcategory.SubCategoryId
                         select new {OrderId = shoporder.Id, ShopCode = shop.ShopCode, Quantity = shoporder.Quantity, Category = category.CategoryName, SubCategory = subcategory.SubCategoryName, Brand = item.BrandName, UnitOfMeasure = item.UnitofMeasure, Color = item.Color  };


            foreach (var x in orders)
                {
                shopOrders.Add(new ShopOrderDetails { Id = x.OrderId, ShopCode = x.ShopCode, Quantity = x.Quantity, CategoryName = x.Category, SubCategoryName = x.SubCategory, Brand = x.Brand, UnitOfMeasure = x.UnitOfMeasure, Color = x.Color });

                }



            return shopOrders;



        }

        public async Task processOrder(IEnumerable<int> ordersProcessed)
        {

            var orders = from p in db.ShopOrder
                           where ordersProcessed.Contains<int>(p.Id) 
                           select p;

            List<int> itemCodes = new List<int>();
            List<int> quant = new List<int>();


            foreach (var o in orders)
            {
                o.WarehouseStatus = 1;
                itemCodes.Add(o.ItemCode.Value);
                quant.Add(o.Quantity);

            }


            var ite = from p in db.Item
                         where itemCodes.Contains<int>(p.ItemCode)
                         select p;


            int i = 0;

            foreach (var o in ite)
            {
                o.Quantity = o.Quantity - quant.ElementAt<int>(i);
                i++;


            }


            

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {

            }


        }



        }

    }

