using Madetomeasure.Data;
using Madetomeasure.ViewModels.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.WarehouseModels
{
    public class ItemManagement
    {
        MadeToMeasureContext db;

        public ItemManagement(MadeToMeasureContext dbContext)
        {
            db = dbContext;
        }

        public async Task addItem(AddItemStockViewModel model, int WorksAt)
        {

            var ite = from i in db.Item
                      where i.BrandName == model.brand && i.CategoryId == model.categoryName && i.Color == model.color && i.PercentageProfit == model.salePrice && i.SubCategoryId == model.subCategoryName && i.UnitofMeasure == model.unitOfMeasure && i.UnitCostPrice == model.costPrice && i.VendorName == model.vendorName && i.WarehouseCode == WorksAt
                      select i;

            bool flag = false;

            foreach (var x in ite)
            {
                x.Quantity += model.quantity;
                flag = true;
            }

            if (flag == true)
            {
                await db.SaveChangesAsync();


            }
            else
            {

                Item item = new Item();

                item.BrandName = model.brand;
                item.CategoryId = model.categoryName;
                item.Color = model.color;
                item.PercentageProfit = model.salePrice;
                item.Quantity = model.quantity;
                item.SubCategoryId = model.subCategoryName;
                item.UnitofMeasure = model.unitOfMeasure;
                item.UnitCostPrice = model.costPrice;
                item.VendorName = model.vendorName;
                item.WarehouseCode = WorksAt;



                db.Item.Add(item);
                await db.SaveChangesAsync();

            }


          



        }



    }
}
