using Madetomeasure.Data;
using Madetomeasure.ViewModels.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.WarehouseModels
{
    public class VendorListSave
    {
        MadeToMeasureContext db;

        public VendorListSave(MadeToMeasureContext dbContext)
        {
            db = dbContext;
        }


        public async Task addToVendorsList(AddVendorListViewModel model, int userId)
        {
            

            OrderfromVendor order = new OrderfromVendor();

            order.BrandName = model.brand;
            order.CategoryId = model.categoryName;
            order.Color = model.color;
            order.Quantity = model.quantity;
            order.Status = 0;
            order.SubCategoryId = model.subCategoryName;
            order.UnitofMeasure = model.unitOfMeasure;
            order.VendorName = model.vendorName;
            order.WarehouseManager = userId;


            db.OrderfromVendor.Add(order);
            await db.SaveChangesAsync();



        }

        public List<ItemDesc> retrieveVendorList(int userId)
        {
            List<ItemDesc> vendorList = new List<ItemDesc>();


            var items = from vendorlist in db.OrderfromVendor
                        where vendorlist.WarehouseManager == userId && vendorlist.Status == 0
                        join cat in db.Category on vendorlist.CategoryId equals cat.CategoryId
                        join subcat in db.SubCategory on vendorlist.SubCategoryId equals subcat.SubCategoryId
                        select new { Category = cat.CategoryName, SubCategory = subcat.SubCategoryName, Brand = vendorlist.BrandName, Color = vendorlist.Color, Quantity = vendorlist.Quantity, UnitOfMeasure = vendorlist.UnitofMeasure, Id = vendorlist.Id, Vendor = vendorlist.VendorName };

            foreach (var x in items)
            {
                vendorList.Add(new ItemDesc { Category = x.Category, SubCategory = x.SubCategory, Brand = x.Brand, VendorName = x.Vendor, Color = x.Color, Id = x.Id, Quantity = x.Quantity.ToString(), UnitOfMeasure = x.UnitOfMeasure });

            }


            return vendorList;

        }

        public async Task  removeVendorListItem(int Id)
        {
            var listItem = from vendorlist in db.OrderfromVendor
                           where vendorlist.Id == Id
                           select vendorlist;

            foreach (var x in listItem)
            {
                x.Status = 1;


            }

            await db.SaveChangesAsync();



        }


    }

    

}
