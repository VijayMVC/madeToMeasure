using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Madetomeasure.Models;
using Microsoft.AspNetCore.Http;
using Madetomeasure.Models.WarehouseModels;
using Madetomeasure.Data;
using Madetomeasure.ViewModels.Warehouse;
using Microsoft.AspNetCore.Authorization;



//Author: Hamza Saleem
//Last Edited: 2/12/2016
//This Controller is used to manage the warehouse manager account and interation with system
//A warehouse manager is redirected here after login



namespace Madetomeasure.Controllers
{
    public class WarehouseController : Controller
    {
        MadeToMeasureContext db;
       

        public WarehouseController(MadeToMeasureContext context)
        {
            db = context;
          
        }

        //Action Method to retrieve the items availability and stock at a warehouse

        [HttpGet]
        public IActionResult WarehouseStatus()
        {
            List<Item> item_list = new List<Item>();

            string worksat = HttpContext.Session.GetString("WorksAt");
            int warehouse_code = Int32.Parse(worksat);
           
            var temp_warehouse_items = from c in db.Item
                                       join o in db.Category on c.CategoryId equals o.CategoryId
                                       join p in db.SubCategory on c.SubCategoryId equals p.SubCategoryId
                                       where c.WarehouseCode == warehouse_code
                                       select new { c, o, p };
            int count = 0;
            foreach (var x in temp_warehouse_items)
            {
                Item temp = new Item();
                temp.BrandName = x.c.BrandName;
                temp.CategoryId = x.c.CategoryId;
                temp.ItemCode = x.c.ItemCode;
                temp.Color = x.c.Color;
                temp.PercentageProfit = x.c.PercentageProfit;
                temp.Quantity = x.c.Quantity;
                temp.SubCategoryId = x.c.SubCategoryId;
                temp.VendorName = x.c.VendorName;
                temp.WarehouseCode = x.c.WarehouseCode;
                temp.UnitofMeasure = x.c.UnitofMeasure;
                temp.UnitCostPrice = x.c.UnitCostPrice;
                temp.Category = new Category();
                temp.SubCategory = new SubCategory();
                temp.Category.CategoryName = x.o.CategoryName;
                temp.SubCategory.SubCategoryName = x.p.SubCategoryName;
                item_list.Add(temp);
                count++;
            }

            if (count == 0)
            {
                ViewBag.itemsreturned = "yes";
                return View(item_list);
            }

            else
            {
                ViewBag.itemsreturned = "yes";
                return View(item_list);
            }


        }



        //Action Method to retrieve Profile Data

        public IActionResult Index()
        {
            
            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index","Account");
            }


            return View();
        }


        [HttpGet]
        public IActionResult ManageCategory()
        {

            CategoryManagement c = new CategoryManagement(db);
            IEnumerable<Category> categories = c.retrieveCategories();

            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(string categoryName)
        {
            CategoryManagement cat = new CategoryManagement(db);

            if (!cat.checkCategoryPresence(categoryName))
            {
                await cat.addCategory(categoryName);

                TempData["CatConfirmation"] = "success";

            }

            else
                TempData["CatConfirmation"] = "failure";



            return RedirectToAction("ManageCategory");
        }

        [HttpPost]
        public async Task<IActionResult> AddSubCategory(int categoryId, string subcategoryName)
        {
            CategoryManagement cat = new CategoryManagement(db);

            if (!cat.checkSubCategoryPresence(subcategoryName))
            {
                await cat.addSubCategory(categoryId, subcategoryName);

                TempData["SubConfirmation"] = "success";

            }

            else
                TempData["SubConfirmation"] = "failure";


            return RedirectToAction("ManageCategory");
        }

        [HttpGet]
        public IActionResult AddItemStock()
        {
            CategoryManagement cat = new CategoryManagement(db);
            IEnumerable<Category> categories = cat.retrieveCategories();
            //IEnumerable<SubCategory> subcategories = cat.retrieveSubCategories();

            ViewData["Categories"] = categories;
           // ViewData["SubCategories"] = subcategories;


            return View();
        }


        [HttpGet]
        public JsonResult GetSubCategories(int CategoryId)
        {
            CategoryManagement c = new CategoryManagement(db);
            
            List<SubCategoryDetail> sub = c.retrieveSubCategories(CategoryId);
            
            return Json(sub);

        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItemStocks(AddItemStockViewModel itemModel)
        {
            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }


            if (ModelState.IsValid)
            {
                ItemManagement i = new ItemManagement(db);
                await i.addItem(itemModel, Convert.ToInt32(HttpContext.Session.GetString("WorksAt")));

                TempData["ItemMsg"] = "success";

                return RedirectToAction("AddItemStock");
            }
            else
            {

                return RedirectToAction("AddItemStock",itemModel);
            }



        }

        [HttpGet]
        public IActionResult VendorList()
        {

            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }
            VendorListSave vendrlist = new VendorListSave(db);
            IEnumerable<ItemDesc> listitems =  vendrlist.retrieveVendorList(Convert.ToInt32(HttpContext.Session.GetString("Id")));

            ViewData["VendorList"] = listitems;

            CategoryManagement cat = new CategoryManagement(db);
            IEnumerable<Category> categories = cat.retrieveCategories();
            //IEnumerable<SubCategory> subcategories = cat.retrieveSubCategories();

            ViewData["Categories"] = categories;
            // ViewData["SubCategories"] = subcategories;



            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToVendorList(AddVendorListViewModel itemModel)
        {
            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }

            if (ModelState.IsValid)
            {
                VendorListSave vendorList = new VendorListSave(db);
              
                await vendorList.addToVendorsList(itemModel, Convert.ToInt32(HttpContext.Session.GetString("Id")));
                
                return RedirectToAction("VendorList");
            }
            else
            {
                return RedirectToAction("VendorList",itemModel);
            }



        }

        public async Task<IActionResult> RemoveVendorListItem(int Id)
        {
            VendorListSave vendrlist = new VendorListSave(db);
            await vendrlist.removeVendorListItem(Id);

            return RedirectToAction("VendorList");

        }
             
        [HttpGet]
        public IActionResult ShopOrder()
        {
            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }

            ShopOrdersRetrieval orders = new ShopOrdersRetrieval(db);
           List<ShopOrderDetails> shopOrders =  orders.retrieveOrders(Int32.Parse(HttpContext.Session.GetString("WorksAt")));
            
            return View(shopOrders);
        }

        //Action Method to process a shop order

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShopOrder(IEnumerable<int> ordersProcessed)
        {
            if (ordersProcessed.Count<int>() != 0)
            {
                ShopOrdersRetrieval sh = new ShopOrdersRetrieval(db);
                await sh.processOrder(ordersProcessed);

                TempData["ShopConfirmation"] = "success";


            }




            return RedirectToAction("ShopOrder");

        }

        [HttpGet]
        public IActionResult StitchingJobOrders()
        {
            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }

            StitchingJobOrdersRetrieval retrieveJobs = new StitchingJobOrdersRetrieval(db);
            List<StitchingOrderDetails> stitchingOrders = retrieveJobs.retrieveOrders(Int32.Parse(HttpContext.Session.GetString("WorksAt")));


            return View(stitchingOrders);
        }

        [HttpGet]
        public IActionResult StitchingJobWithFabric(int Id)
        {

            StitchingJobContentRetrieval orderDetails = new StitchingJobContentRetrieval(db);
            StitchingJobContentDetails details = orderDetails.retrieveOrders(Id);

            

            return View(details);
        }

        [HttpGet]
        public IActionResult StitchingJobWithoutFabric(int Id)
        {


            StitchingJobContentRetrieval orderDetails = new StitchingJobContentRetrieval(db);
            StitchingJobContentDetails details = orderDetails.retrieveOrders(Id);



            return View(details);
        }

        [HttpGet]
        public async Task<IActionResult> ProcessStitchingJob(int Id)
        {
            ProcessStitchingJob jobProcess = new ProcessStitchingJob(db);
            await jobProcess.doOrderProcessing(Id);

            return RedirectToAction("StitchingJobOrders");

        }

        //Action Method to retrieve a confirmation page displaying result of an action

        [HttpGet]
        public IActionResult ConfirmationPage()
        {
            ViewBag.Message = "Smooth kam hogya ustad ";
            ViewBag.MessageType = "success";
            return View();
        }





        //[HttpGet]
        //public IActionResult BarChart()
        //{

        //    return View();
        //}



    }
}
