using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Madetomeasure.Data;
using Madetomeasure.Models.Shop;
using Microsoft.AspNetCore.Http;
using Madetomeasure.ViewModels.Shop;


//Author: Hammad Zia
//Last Edited: 2/12/2016
//This Controller manages shopkeeper functionalities which includes adding jobs, customers, shoporders, invoices, receipts.
//A shopkeeper is redirected here after login

namespace Madetomeasure.Controllers
{
    public class ShopController : Controller
    {

        MadeToMeasureContext db;
        //static List<AddStitchingJobViewModel> stitchingjob_list= new List<AddStitchingJobViewModel>();
        List<AddStitchingJobViewModel> stitchingjob_list;

        
        public ShopController(MadeToMeasureContext obj)
        {
            //stitchingjob_list = new List<AddStitchingJobViewModel>();
            db = obj;

            //var myComplexObject = new AddStitchingJobViewModel();
            //HttpContext.Session.SetObjectAsJson("Test", stitchingjob_list);

            //var myComplexObject = HttpContext.Session.GetObjectFromJson<MyClass>("Test");

        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        //Redirecrts to search item status page
        [HttpGet]
        public IActionResult SearchItemStatus()
        {
            String page_title = "Item Status";
            ViewBag.Title = page_title;
            return View();
        }

        //Takes as input an item code and returns its status from the warehouse.
        [HttpPost]
        public IActionResult SearchItemStatus(Item obj)
        {
            if (ModelState.IsValid)
            {
                Item temp = new Item();
                int id = obj.ItemCode;
                int warehouse_code = 0;
                string worksat = HttpContext.Session.GetString("WorksAt");
                int shop_code = Int32.Parse(worksat);
                var temp_warehouse = from c in db.Shop
                                     where c.ShopCode == shop_code
                                     select c;

                foreach (var x in temp_warehouse)
                {
                    warehouse_code = x.AssociatedWarehouseCode;
                }


                var temp_item = from c in db.Item
                                join o in db.Category on c.CategoryId equals o.CategoryId
                                join p in db.SubCategory on c.SubCategoryId equals p.SubCategoryId
                                where c.ItemCode == id && c.WarehouseCode == warehouse_code
                                select new { c, o, p };




                int count = 0;
                foreach (var x in temp_item)
                {
                    temp.BrandName = x.c.BrandName;
                    temp.CategoryId = x.c.CategoryId;
                    temp.ItemCode = x.c.ItemCode;
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
                    count++;
                }
                if (count > 0)
                {
                    ViewBag.itemreturned = "yes";
                    return View(temp);
                }


                ViewBag.itemreturned = "no";
                ViewBag.notfound = "item not found";
                return View(obj);
            }

            else
            {
                return View(obj);
            }



        }

        //Ajax request to get customer phone number
        [HttpGet]
        public JsonResult GetCustomerId(string id)
        {
            String phonenumber = id;
            // List<Users> user_list = new List<Users>();

            var users = from p in db.Users
                        where p.UserId.Contains(phonenumber) && p.UserType == 1
                        select p;

            //   Users u = new Users();

            List<UUser> user_list = new List<UUser>();


            foreach (var p in users)
            {
                UUser u = new UUser();
                u.userId = p.UserId;
                user_list.Add(u);
            }


            return Json(user_list);
        }

        //Redirects to the add shop order page
        [HttpGet]
        public IActionResult AddShopOrder()
        {
            return View();
        }

        //Takes as input a shoporder object and sends a request to the warehouse.
        [HttpPost]
        public async Task<ActionResult> AddShopOrder(ShopOrder obj)
        {
            int shopcode =Convert.ToInt32( HttpContext.Session.GetString("WorksAt"));
            int warehouse_status = 0;
            string insufficient_quantity;


            if (!ModelState.IsValid)
            {
                // ModelState.AddModelError(string.Empty, "blabla");
                return View(obj);
            }

            else
            {

                int count = 0;
                bool check = false;
                int quantity2 = obj.Quantity;
                if (quantity2 <= 0)
                {
                    ViewBag.quantityerror = "invalid quantity";
                    return View(obj);
                }

                var temp_item1 = from u in db.Item
                                 where u.ItemCode == obj.ItemCode
                                 select u;
                foreach (var x in temp_item1)
                {
                    count++;
                }

                if (count == 0)
                {
                    ViewBag.itemnotfound = "item not found";
                    return View(obj);
                }



                foreach (var y in temp_item1)
                {
                    if (y.Quantity > quantity2)
                    {
                        check = true;
                    }
                    else
                    {
                        insufficient_quantity = y.Quantity.ToString();
                        check = false;
                    }
                }


                if (check)
                {
                    foreach (var y in temp_item1)
                    {
                        y.Quantity = y.Quantity - quantity2;

                    }
                    obj.ShopCode = shopcode;
                    obj.WarehouseStatus = warehouse_status;
                    await db.ShopOrder.AddAsync(obj);
                    await db.SaveChangesAsync();
                    ViewBag.successful = "item added successfully";
                    return View(obj);

                }

                ViewBag.itemquantity = "quantity not sufficient";
                return View(obj);
            }
        }
        
        //Redirects to the view invoice page which displays invoice details alongwith shop information
        [HttpGet]
        public IActionResult ViewInvoice()
        {
            InvoiceViewModel invoice_view = new InvoiceViewModel();
            int count = 0;
            string invoice_number = HttpContext.Session.GetString("invoice_number");
            int invoice_num = Int32.Parse(invoice_number);
            var temp_invocie = from c in db.Invoice
                               join o in db.InvoiceDetails on c.InvoiceId equals o.InvoiceId
                               join p in db.StitchingJob on o.StitchingJobId equals p.JobId
                               join d in db.JobType on p.JobTypeId equals d.Id
                               join e in db.BusinessEntity on p.ShopCode equals e.EntityCode
                               join f in db.Users on c.SalesPersonId equals f.Id
                               where c.InvoiceId == invoice_num
                               select new { c, o, p, d, e, f };

            foreach (var x in temp_invocie)
            {
                InvoiceDetailViewModel detail_temp = new InvoiceDetailViewModel();
                if (count == 0)
                {
                    invoice_view.AdvanceReceived = x.c.AdvanceReceived;
                    invoice_view.Date = x.c.Date;
                    invoice_view.InvoiceId = x.c.InvoiceId;
                    invoice_view.salesperson_name = x.f.Name;
                    invoice_view.shop_address = x.e.EntityAddress;
                    invoice_view.TotalAmount = x.c.TotalAmount;
                    invoice_view.balance = (x.c.TotalAmount - x.c.AdvanceReceived);
                }
                count++;

                detail_temp.job_type = x.d.JobName;
                detail_temp.Price = x.o.Price;
                invoice_view.InvoiceDetails.Add(detail_temp);
            }


            return View(invoice_view);
        }

        //Redirects to the view receipt page which displays receipt details.
        [HttpGet]
        public IActionResult ViewReceipt()
        {
            InvoiceViewModel invoice_view = new InvoiceViewModel();
            int count = 0;
            string receipt_number = HttpContext.Session.GetString("receipt_number");
            int receipt_num = Int32.Parse(receipt_number);
            var temp_receipt = from c in db.Invoice
                               join r in db.Receipt on c.InvoiceId equals r.InvoiceId
                               join o in db.InvoiceDetails on c.InvoiceId equals o.InvoiceId
                               join p in db.StitchingJob on o.StitchingJobId equals p.JobId
                               join d in db.JobType on p.JobTypeId equals d.Id
                               join e in db.BusinessEntity on p.ShopCode equals e.EntityCode
                               join f in db.Users on c.SalesPersonId equals f.Id
                               where r.ReceiptId == receipt_num
                               select new { c, o, p, d, e, f, r };

            foreach (var x in temp_receipt)
            {
                InvoiceDetailViewModel detail_temp = new InvoiceDetailViewModel();
                if (count == 0)
                {
                    invoice_view.AdvanceReceived = x.c.AdvanceReceived;
                    invoice_view.Date = x.c.Date;
                    invoice_view.InvoiceId = x.c.InvoiceId;
                    invoice_view.salesperson_name = x.f.Name;
                    invoice_view.shop_address = x.e.EntityAddress;
                    invoice_view.TotalAmount = x.c.TotalAmount;
                    invoice_view.balance = (x.c.TotalAmount - x.c.AdvanceReceived);
                    invoice_view.ReceiptId = x.r.ReceiptId;
                }
                count++;

                detail_temp.job_type = x.d.JobName;
                detail_temp.Price = x.o.Price;
                invoice_view.InvoiceDetails.Add(detail_temp);
            }


            return View(invoice_view);
        }


        //Returns a list of items belonging to the associated warehouses
        [HttpGet]
        public IActionResult WarehouseStatus()
        {
            List<Item> item_list = new List<Item>();

            string worksat = HttpContext.Session.GetString("WorksAt");
            int shop_code = Int32.Parse(worksat);
            int warehouse_code = 0;
            var temp_warehouse = from u in db.Shop
                                 where u.ShopCode == shop_code
                                 select u;
            foreach (var temp in temp_warehouse)
            {
                warehouse_code = temp.AssociatedWarehouseCode;
            }

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

        //Redirects to the add customer page
        [HttpGet]
        public IActionResult AddCustomer()
        {

            return View();
        }

        //Takes as input a customer object and adds it in the users table
        [HttpPost]
        public async Task<ActionResult> AddCustomer(AddCustomerViewModel obj)
        {
            if (!ModelState.IsValid)
            {

                return View(obj);
            }

            var temp_customer = from u in db.Users
                                where u.UserId == obj.PhoneNumber
                                select u;
            int count = 0;
            foreach (var temp in temp_customer)
            {
                count++;
            }

            if (count > 0)
            {
                ViewBag.duplicationerror = "Customer already exists";
                return View(obj);
            }

            else
            {
                Users new_user = new Users();
                new_user.UserId = obj.PhoneNumber;
                new_user.Password = obj.Password;
                new_user.Name = obj.Name;
                new_user.JoiningDate = System.DateTime.Now;
                new_user.UserType = 1;
                await db.Users.AddAsync(new_user);
                await db.SaveChangesAsync();
                ViewBag.successful = "Customer Added Successfully";
                return View(obj);
            }


        }

        //Redirects to the invoice generation page which takes as input advance amount received
        [HttpGet]
        public IActionResult InvoiceGeneration()
        {

            return View();
        }

        //Redirects to the receipt generation page which takes as input an invoice number.
        [HttpGet]
        public IActionResult ReceiptGeneration()
        {

            return View();
        }

        //Gets invoice details according to the given invoice number and adds it to the receipts table.
        [HttpPost]
        public async Task<ActionResult> ReceiptGeneration(Receipt r)
        {
            if (!ModelState.IsValid)
            {
                return View(r);

            }

            var temp_receipt = from u in db.Invoice
                               where u.InvoiceId == r.InvoiceId
                               select u;

            var check_receipt_duplication = from u in db.Receipt
                                            where u.InvoiceId == r.InvoiceId
                                            select u;

            int duplicatecount = 0;
            foreach (var temp in check_receipt_duplication)
            {
                string session_receipt = temp.ReceiptId.ToString();
                HttpContext.Session.SetString("receipt_number", session_receipt);
                duplicatecount++;
            }
            if (duplicatecount > 0)
            {
                ViewBag.alreadypresent = "receipt already present";
                return View(r);
            }

            int count = 0;
            Receipt new_receipt = new Receipt();
            int receipt_temp = 0;
            while (true)
            {
                int dup = 0;
                Random rnd = new Random();
                int rnd_number = rnd.Next(10000, 90000);

                var temp = from i in db.Receipt
                           where i.ReceiptId == rnd_number
                           select i;
                foreach (var b in temp)
                {
                    dup++;
                }

                if (dup == 0)
                {
                    receipt_temp = rnd_number;
                    break;
                }
            }


            foreach (var temp in temp_receipt)
            {
                new_receipt.InvoiceId = temp.InvoiceId;
                new_receipt.Amount = temp.TotalAmount;
                new_receipt.Date = System.DateTime.Now;
                count++;
            }

            if (count == 0)
            {
                ViewBag.receiptmessage = "Invalid InvoiceId";
                return View(r);
            }
            else
            {
                new_receipt.ReceiptId = receipt_temp;
                string session_receipt_temp = receipt_temp.ToString();
                HttpContext.Session.SetString("receipt_number", session_receipt_temp);
                db.Receipt.Add(new_receipt);
                await db.SaveChangesAsync();
                return RedirectToAction("ViewReceipt");
            }

        }


        //Takes as input an invoice object and adds invoice details and invoice to their respective tables.
        [HttpPost]
        public async Task<ActionResult> InvoiceGeneration(Invoice n)
        {
            double total_sum = 0;
            // int amount_received = 5000;
            Invoice invoice_new = new Invoice();
            while (true)
            {
                int dup = 0;
                Random rnd = new Random();
                int rnd_number = rnd.Next(10000, 90000);

                var temp = from i in db.Invoice
                           where i.InvoiceId == rnd_number
                           select i;
                foreach (var b in temp)
                {
                    dup++;
                }

                if (dup == 0)
                {
                    invoice_new.InvoiceId = rnd_number;
                    break;
                }
            }

            string session_invoice = invoice_new.InvoiceId.ToString();
            HttpContext.Session.SetString("invoice_number", session_invoice);
            //invoice_number = invoice_new.InvoiceId;
            var myComplexObject = HttpContext.Session.GetObjectFromJson<List<AddStitchingJobViewModel>>("Test");
            List<AddStitchingJobViewModel> temp_list = (List<AddStitchingJobViewModel>)myComplexObject;
            if (myComplexObject == null)
                return View("InvoiceGeneration"); ;
            if (myComplexObject.Count == 0)
            {
                ViewBag.jobnotadded = "Add a job to proceed with invoice generation";
                return View(n);

            }

            foreach (var job in temp_list)
            {


                int stitching_job_id = job.JobId.Value;
                invoice_new.Date = job.ExpectedDate;
                double sum = 0;
                double unitcost = 0;

                foreach (var detail in job.StitchingJobDetails)
                {
                    int quantity = Int32.Parse(detail.Quantity);
                    int itemcode = detail.ItemCode;
                    double margin = 0;

                    var price = from p in db.Item
                                where p.ItemCode == itemcode
                                select p;

                    foreach (var q in price)
                    {
                        unitcost = q.UnitCostPrice;
                        margin = unitcost * q.PercentageProfit;
                        margin = margin / 100;
                        margin = margin + unitcost;
                        sum = sum + (margin * quantity);

                    }


                }
                total_sum = total_sum + sum;
                InvoiceDetails invoice_detail = new InvoiceDetails();
                invoice_detail.InvoiceId = invoice_new.InvoiceId;
                if (job.JobTypeId == 1)
                {
                    int temp_sum = Convert.ToInt32(sum);
                    invoice_detail.Price = temp_sum + 5000;
                    total_sum = total_sum + 5000;
                }
                if (job.JobTypeId == 2)
                {
                    int temp_sum = Convert.ToInt32(sum);
                    invoice_detail.Price = temp_sum + 1500;
                    total_sum = total_sum + 1500;
                }
                if (job.JobTypeId == 3)
                {
                    int temp_sum = Convert.ToInt32(sum);
                    invoice_detail.Price = temp_sum + 1000;
                    total_sum = total_sum + 1000;
                }
                if (job.JobTypeId == 4)
                {
                    int temp_sum = Convert.ToInt32(sum);
                    invoice_detail.Price = temp_sum + 3000;
                    total_sum = total_sum + 3000;
                }
                if (job.JobTypeId == 5)
                {
                    int temp_sum = Convert.ToInt32(sum);
                    invoice_detail.Price = temp_sum + 1000;
                    total_sum = total_sum + 1000;
                }
                if (job.JobTypeId == 6)
                {
                    int temp_sum = Convert.ToInt32(sum);
                    invoice_detail.Price = temp_sum + 2000;
                    total_sum = total_sum + 2000;
                }
                //int itemcode = detail.ItemCode;

                invoice_detail.StitchingJobId = stitching_job_id;
                invoice_new.InvoiceDetails.Add(invoice_detail);




            }
            invoice_new.TotalAmount = Convert.ToInt32(total_sum);
            invoice_new.AdvanceReceived = n.AdvanceReceived;

            string worksat = HttpContext.Session.GetString("WorksAt");
            string sales_person_id = HttpContext.Session.GetString("Id");
            invoice_new.ShopCode = Int32.Parse(worksat);
            invoice_new.SalesPersonId = Int32.Parse(sales_person_id);

            await db.Invoice.AddAsync(invoice_new);

            foreach (var d in invoice_new.InvoiceDetails)
            {
                await db.InvoiceDetails.AddAsync(d);
            }

            //await db.InvoiceDetails.AddAsync(invoice_detail);
            await db.SaveChangesAsync();
            return RedirectToAction("ViewInvoice");

        }


        //Sets the currently added job to be printed.
        [HttpPost]
        public IActionResult PrintJobCard(AddStitchingJobViewModel obj)
        {
            var myComplexObject = HttpContext.Session.GetObjectFromJson<AddStitchingJobViewModel>("Tempjob");
            AddStitchingJobViewModel temp_job = (AddStitchingJobViewModel)myComplexObject;
            return View(temp_job);
        }

        //redirects to the add stitching job page
        [HttpPost]
        public IActionResult AddAnotherJob()
        {
            return View("AddStitchingJob");
        }

        //redirects to the add stitching job page
        [HttpGet]
        public IActionResult AddStitchingJob()
        {
            stitchingjob_list = new List<AddStitchingJobViewModel>();


            //var myComplexObject = new AddStitchingJobViewModel();
            HttpContext.Session.SetObjectAsJson("Test", stitchingjob_list);
            return View();
        }

        //Takes as input an add stitching job view model and adds it to the stitching job, stitching job details and mesaurements table
        [HttpPost]
        public async Task<ActionResult> AddStitchingJob(AddStitchingJobViewModel a, int JobTypeId)
        {
            int itemcode1 = 0;
            int itemcode2 = 0;
            int itemcode3 = 0;
            int quantity1 = 0;
            int quantity2 = 0;
            int quantity3 = 0;
            string details_temp;
            string worksat = HttpContext.Session.GetString("WorksAt");
            int shop_code = Int32.Parse(worksat);
            int warehouse_code = 0;
            var temp_warehouse = from u in db.Shop
                                 where u.ShopCode == shop_code
                                 select u;
            foreach (var temp in temp_warehouse)
            {
                warehouse_code = temp.AssociatedWarehouseCode;
            }
            //int shop_code = 1;
            //int warehouse_code = 4;


            //this.ModelState.Remove("SuitMeasurements");
            if (!ModelState.IsValid)
            {
                return View(a);
            }

            int customer_count = 0;
            int customer_id = 0;
            var temp_customer = from u in db.Users
                                where u.UserId == a.CustomerId
                                select u;

            foreach (var x in temp_customer)
            {
                customer_id = x.Id;
                customer_count++;
            }

            if (customer_count == 0)
            {
                ViewBag.customernotfound = "customer not found";
                return View(a);
            }

            if (JobTypeId == 3)
            {
                int count = 0;
                bool check = false;
                int measurmentcount = 0;

                double m1 = 0, m2 = 0, m3 = 0, m4 = 0, m5 = 0, m6 = 0, m7 = 0, m8 = 0, m9 = 0, m10 = 0;
                if (a.ShirtItemCode1.HasValue)
                {
                    itemcode1 = a.ShirtItemCode1.Value;

                }

                if (a.ShirtItemCode2.HasValue)
                {
                    itemcode2 = a.ShirtItemCode2.Value;

                }
                if (a.ShirtQuantity1.HasValue)
                {
                    quantity1 = a.ShirtQuantity1.Value;
                    if (quantity1 <= 0)
                    {
                        ViewBag.shirtquantity1 = "invalid quantity";
                        return View(a);
                    }

                }
                if (a.ShirtQuantity2.HasValue)
                {
                    quantity2 = a.ShirtQuantity2.Value;
                    if (quantity2 <= 0)
                    {
                        ViewBag.shirtquantity2 = "invalid quantity";
                        return View(a);
                    }

                }

                if (a.ShirtMeasurements.Bicep.HasValue)
                {
                    m1 = a.ShirtMeasurements.Bicep.Value;
                    measurmentcount++;
                }
                if (a.ShirtMeasurements.Chest.HasValue)
                {
                    m2 = a.ShirtMeasurements.Chest.Value;
                    measurmentcount++;
                }
                if (a.ShirtMeasurements.Collar.HasValue)
                {
                    m3 = a.ShirtMeasurements.Collar.Value;
                    measurmentcount++;
                }
                if (a.ShirtMeasurements.FullBack.HasValue)
                {
                    m4 = a.ShirtMeasurements.FullBack.Value;
                    measurmentcount++;
                }
                if (a.ShirtMeasurements.HalfBack.HasValue)
                {
                    m5 = a.ShirtMeasurements.HalfBack.Value;
                    measurmentcount++;
                }
                if (a.ShirtMeasurements.Length.HasValue)
                {
                    m6 = a.ShirtMeasurements.Length.Value;
                    measurmentcount++;
                }
                if (a.ShirtMeasurements.Shoulder.HasValue)
                {
                    m7 = a.ShirtMeasurements.Shoulder.Value;
                    measurmentcount++;
                }
                if (a.ShirtMeasurements.Sleeve.HasValue)
                {
                    m8 = a.ShirtMeasurements.Sleeve.Value;
                    measurmentcount++;
                }
                if (a.ShirtMeasurements.Waist.HasValue)
                {
                    m9 = a.ShirtMeasurements.Waist.Value;
                    measurmentcount++;
                }
                if (a.ShirtMeasurements.Wrist.HasValue)
                {
                    m10 = a.ShirtMeasurements.Wrist.Value;
                    measurmentcount++;
                }
                //if(measurmentcount<10)
                //{
                //    ViewBag.measurementsempty = "measurements cannot be empty";
                //    return View(a);
                //}
                details_temp = a.ShirtMeasurements.OtherDetails;

                if (m1 <= 0 || m2 <= 0 || m3 <= 0 || m4 <= 0 || m5 <= 0 || m6 <= 0 || m7 <= 0 || m8 <= 0 || m9 <= 0 || m10 <= 0)
                {
                    ViewBag.shirtmeasurements = "Measurements cannot be empty or zero";
                    return View(a);
                }



                var temp_item1 = from u in db.Item
                                 where u.ItemCode == itemcode1 && u.WarehouseCode == warehouse_code
                                 select u;

                var temp_item2 = from u in db.Item
                                 where u.ItemCode == itemcode2 && u.WarehouseCode == warehouse_code
                                 select u;
                if (a.FabricfromCustomer == 0)
                {
                    foreach (var x in temp_item1)
                    {
                        count++;
                    }

                    if (count == 0)
                    {
                        ViewBag.shirtitem1 = "item not found";
                        return View(a);
                    }

                    foreach (var x in temp_item2)
                    {
                        count++;
                    }

                    if (count == 0)
                    {
                        ViewBag.shirtitem2 = "item not found";
                        return View(a);
                    }
                }

                if (count == 2 && a.FabricfromCustomer == 0)
                {
                    foreach (var x in temp_item1)
                    {
                        if (x.Quantity > quantity1)
                        {
                            check = true;
                        }
                        else
                        {
                            check = false;
                        }
                    }

                    foreach (var y in temp_item2)
                    {
                        if (y.Quantity > quantity2)
                        {
                            check = true;
                        }
                        else
                        {
                            check = false;
                        }
                    }


                    if (check)
                    {
                        if (a.FabricfromCustomer == 0)
                        {
                            foreach (var y in temp_item1)
                            {
                                y.Quantity = y.Quantity - quantity1;

                            }
                            //await db.SaveChangesAsync();
                            foreach (var x in temp_item2)
                            {
                                x.Quantity = x.Quantity - quantity2;

                            }
                        }
                        StitchingJob job = new StitchingJob();
                        while (true)
                        {
                            int dup = 0;
                            Random rnd = new Random();
                            int rnd_number = rnd.Next(10000, 90000);

                            var temp = from i in db.StitchingJob
                                       where i.JobId == rnd_number
                                       select i;
                            foreach (var b in temp)
                            {
                                dup++;
                            }

                            if (dup == 0)
                            {
                                job.JobId = rnd_number;
                                break;
                            }
                        }

                        a.JobId = job.JobId;
                        job.JobTypeId = a.JobTypeId;
                        job.ShopCode = shop_code;
                        job.WarehouseStatus = 0;
                        job.CurrentStatus = 0;
                        job.FabricfromCustomer = a.FabricfromCustomer;
                        job.ExpectedDate = a.ExpectedDate;
                        job.CustomerId = customer_id;
                        job.OrderDate = System.DateTime.Now;



                        ShirtMeasurements shirt = new ShirtMeasurements();
                        shirt.Bicep = m1;
                        shirt.Chest = m2;
                        shirt.Collar = m3;
                        shirt.FullBack = m4;
                        shirt.HalfBack = m5;
                        shirt.Length = m6;
                        shirt.Shoulder = m7;
                        shirt.Sleeve = m8;
                        shirt.Waist = m9;
                        shirt.Wrist = m10;
                        shirt.OtherDetails = details_temp;
                        shirt.StitchingJobId = job.JobId;

                        StitchingJobDetails jobdetails1 = new StitchingJobDetails();
                        StitchingJobDetails jobdetails2 = new StitchingJobDetails();
                        if (a.FabricfromCustomer == 0)
                        {

                            jobdetails1.JobId = job.JobId;
                            jobdetails1.ItemCode = itemcode1;
                            jobdetails1.Quantity = quantity1.ToString();


                            jobdetails2.JobId = job.JobId;
                            jobdetails2.ItemCode = itemcode2;
                            jobdetails2.Quantity = quantity2.ToString();
                            a.StitchingJobDetails.Add(jobdetails1);
                            a.StitchingJobDetails.Add(jobdetails2);
                        }

                        var myComplexObject = HttpContext.Session.GetObjectFromJson<List<AddStitchingJobViewModel>>("Test");
                        List<AddStitchingJobViewModel> temp_list = (List<AddStitchingJobViewModel>)myComplexObject;
                        temp_list.Add(a);
                        HttpContext.Session.SetObjectAsJson("Test", temp_list);
                        HttpContext.Session.SetObjectAsJson("Tempjob", a);



                        await db.StitchingJob.AddAsync(job);
                        if (a.FabricfromCustomer == 0)
                        {
                            await db.StitchingJobDetails.AddAsync(jobdetails1);
                            await db.StitchingJobDetails.AddAsync(jobdetails2);
                        }
                        await db.ShirtMeasurements.AddAsync(shirt);
                        await db.SaveChangesAsync();
                        ViewBag.successful = "yes";
                        return View();
                    }
                }

                else if (a.FabricfromCustomer == 1)
                {
                    StitchingJob job = new StitchingJob();
                    while (true)
                    {
                        int dup = 0;
                        Random rnd = new Random();
                        int rnd_number = rnd.Next(10000, 90000);

                        var temp = from i in db.StitchingJob
                                   where i.JobId == rnd_number
                                   select i;
                        foreach (var b in temp)
                        {
                            dup++;
                        }

                        if (dup == 0)
                        {
                            job.JobId = rnd_number;
                            break;
                        }
                    }

                    a.JobId = job.JobId;
                    job.JobTypeId = a.JobTypeId;
                    job.ShopCode = shop_code;
                    job.WarehouseStatus = 0;
                    job.CurrentStatus = 0;
                    job.FabricfromCustomer = a.FabricfromCustomer;
                    job.ExpectedDate = a.ExpectedDate;
                    job.CustomerId = customer_id;
                    job.OrderDate = System.DateTime.Now;


                    ShirtMeasurements shirt = new ShirtMeasurements();
                    shirt.Bicep = m1;
                    shirt.Chest = m2;
                    shirt.Collar = m3;
                    shirt.FullBack = m4;
                    shirt.HalfBack = m5;
                    shirt.Length = m6;
                    shirt.Shoulder = m7;
                    shirt.Sleeve = m8;
                    shirt.Waist = m9;
                    shirt.Wrist = m10;
                    shirt.OtherDetails = details_temp;
                    shirt.StitchingJobId = job.JobId;

                    var myComplexObject = HttpContext.Session.GetObjectFromJson<List<AddStitchingJobViewModel>>("Test");
                    List<AddStitchingJobViewModel> temp_list = (List<AddStitchingJobViewModel>)myComplexObject;
                    temp_list.Add(a);
                    HttpContext.Session.SetObjectAsJson("Test", temp_list);
                    HttpContext.Session.SetObjectAsJson("Tempjob", a);



                    await db.StitchingJob.AddAsync(job);

                    await db.ShirtMeasurements.AddAsync(shirt);
                    await db.SaveChangesAsync();
                    ViewBag.successful = "yes";
                    return View();
                }





            }

            else if (JobTypeId == 1)
            {
                int count = 0;
                bool check = false;
                double m1 = 0, m2 = 0, m3 = 0, m4 = 0, m5 = 0, m6 = 0, m7 = 0, m8 = 0, m9 = 0, m10 = 0, m11 = 0, m12 = 0, m13 = 0, m14 = 0, m15 = 0, m16 = 0, m17 = 0;
                if (a.SuitItemCode1.HasValue)
                {
                    itemcode1 = a.SuitItemCode1.Value;
                }
                if (a.SuitItemCode2.HasValue)
                {
                    itemcode2 = a.SuitItemCode2.Value;
                }
                if (a.SuitItemCode3.HasValue)
                {
                    itemcode3 = a.SuitItemCode3.Value;
                }
                if (a.SuitQuantity1.HasValue)
                {

                    quantity1 = a.SuitQuantity1.Value;
                    if (quantity1 <= 0)
                    {
                        ViewBag.suitquantity1 = "invalid quantity";
                        return View(a);
                    }
                }
                if (a.SuitQuantity2.HasValue)
                {
                    quantity2 = a.SuitQuantity2.Value;
                    if (quantity2 <= 0)
                    {
                        ViewBag.suitquantity2 = "invalid quantity";
                        return View(a);
                    }
                }
                if (a.SuitQuantity3.HasValue)
                {
                    quantity3 = a.SuitQuantity3.Value;
                    if (quantity3 <= 0)
                    {
                        ViewBag.suitquantity3 = "invalid quantity";
                        return View(a);
                    }
                }

                if (a.SuitMeasurements.Bicep.HasValue)
                {
                    m1 = a.SuitMeasurements.Bicep.Value;
                }
                if (a.SuitMeasurements.Chest.HasValue)
                {
                    m2 = a.SuitMeasurements.Chest.Value;
                }
                if (a.SuitMeasurements.Collar.HasValue)
                {
                    m3 = a.SuitMeasurements.Collar.Value;
                }
                if (a.SuitMeasurements.FullBack.HasValue)
                {
                    m4 = a.SuitMeasurements.FullBack.Value;
                }
                if (a.SuitMeasurements.HalfBack.HasValue)
                {
                    m5 = a.SuitMeasurements.HalfBack.Value;
                }
                if (a.SuitMeasurements.Length.HasValue)
                {
                    m6 = a.SuitMeasurements.Length.Value;
                }
                if (a.SuitMeasurements.Shoulder.HasValue)
                {
                    m7 = a.SuitMeasurements.Shoulder.Value;
                }
                if (a.SuitMeasurements.Sleeve.HasValue)
                {
                    m8 = a.SuitMeasurements.Sleeve.Value;
                }
                if (a.SuitMeasurements.Waist.HasValue)
                {
                    m9 = a.SuitMeasurements.Waist.Value;
                }
                if (a.SuitMeasurements.Wrist.HasValue)
                {
                    m10 = a.SuitMeasurements.Wrist.Value;
                }
                if (a.SuitMeasurements.Crotch.HasValue)
                {
                    m11 = a.SuitMeasurements.Crotch.Value;
                }
                if (a.SuitMeasurements.Hip.HasValue)
                {
                    m12 = a.SuitMeasurements.Hip.Value;
                }
                if (a.SuitMeasurements.Inseam.HasValue)
                {
                    m13 = a.SuitMeasurements.Inseam.Value;
                }
                if (a.SuitMeasurements.Knee.HasValue)
                {
                    m14 = a.SuitMeasurements.Knee.Value;
                }
                if (a.SuitMeasurements.Length.HasValue)
                {
                    m15 = a.SuitMeasurements.Length.Value;
                }
                if (a.SuitMeasurements.OutSeam.HasValue)
                {
                    m16 = a.SuitMeasurements.OutSeam.Value;
                }
                if (a.SuitMeasurements.Thigh.HasValue)
                {
                    m17 = a.SuitMeasurements.Thigh.Value;
                }
                details_temp = a.SuitMeasurements.OtherDetails;
                //SuitMeasurements temp_suit = new SuitMeasurements();
                //temp_suit = a.SuitMeasurements;


                if (m1 <= 0 || m2 <= 0 || m3 <= 0 || m4 <= 0 || m5 <= 0 || m6 <= 0 || m7 <= 0 || m8 <= 0 || m9 <= 0 || m10 <= 0 || m11 <= 0 || m12 <= 0 || m13 <= 0 || m14 <= 0 || m15 <= 0 || m16 <= 0 || m17 <= 0)
                {
                    ViewBag.suitmeasurements = "Measurements cannot be empty or zero";
                    return View(a);
                }



                var temp_item1 = from u in db.Item
                                 where u.ItemCode == itemcode1 && u.WarehouseCode == warehouse_code
                                 select u;

                var temp_item2 = from u in db.Item
                                 where u.ItemCode == itemcode2 && u.WarehouseCode == warehouse_code
                                 select u;

                var temp_item3 = from u in db.Item
                                 where u.ItemCode == itemcode3 && u.WarehouseCode == warehouse_code
                                 select u;
                if (a.FabricfromCustomer == 0)
                {
                    foreach (var x in temp_item1)
                    {
                        count++;
                    }

                    if (count == 0)
                    {
                        ViewBag.suititem1 = "item not found";
                        return View(a);
                    }

                    foreach (var x in temp_item2)
                    {
                        count++;
                    }

                    if (count == 0)
                    {
                        ViewBag.suititem2 = "item not found";
                        return View(a);
                    }

                    foreach (var x in temp_item3)
                    {
                        count++;
                    }

                    if (count == 0)
                    {
                        ViewBag.suititem3 = "item not found";
                        return View(a);
                    }
                }

                if (count == 3 && a.FabricfromCustomer == 0)
                {
                    foreach (var x in temp_item1)
                    {
                        if (x.Quantity > quantity1)
                        {
                            check = true;
                        }
                        else
                        {
                            check = false;
                        }
                    }

                    foreach (var y in temp_item2)
                    {
                        if (y.Quantity > quantity2)
                        {
                            check = true;
                        }
                        else
                        {
                            check = false;
                        }
                    }

                    foreach (var z in temp_item3)
                    {
                        if (z.Quantity > quantity2)
                        {
                            check = true;
                        }
                        else
                        {
                            check = false;
                        }
                    }


                    if (check)
                    {
                        if (a.FabricfromCustomer == 0)
                        {
                            foreach (var y in temp_item1)
                            {
                                y.Quantity = y.Quantity - quantity1;

                            }
                            //await db.SaveChangesAsync();
                            foreach (var x in temp_item2)
                            {
                                x.Quantity = x.Quantity - quantity2;

                            }

                            foreach (var z in temp_item3)
                            {
                                z.Quantity = z.Quantity - quantity2;

                            }
                        }
                        StitchingJob job = new StitchingJob();
                        while (true)
                        {
                            int dup = 0;
                            Random rnd = new Random();
                            int rnd_number = rnd.Next(10000, 90000);

                            var temp = from i in db.StitchingJob
                                       where i.JobId == rnd_number
                                       select i;
                            foreach (var b in temp)
                            {
                                dup++;
                            }

                            if (dup == 0)
                            {
                                job.JobId = rnd_number;
                                break;
                            }
                        }

                        a.JobId = job.JobId;
                        job.JobTypeId = a.JobTypeId;
                        job.ShopCode = shop_code;
                        job.WarehouseStatus = 0;
                        job.CurrentStatus = 0;
                        job.FabricfromCustomer = a.FabricfromCustomer;
                        job.ExpectedDate = a.ExpectedDate;
                        job.CustomerId = customer_id;
                        job.OrderDate = System.DateTime.Now;


                        SuitMeasurements suit = new SuitMeasurements();
                        suit.Bicep = m1;
                        suit.Chest = m2;
                        suit.Collar = m3;
                        suit.FullBack = m4;
                        suit.HalfBack = m5;
                        suit.Length = m6;
                        suit.Shoulder = m7;
                        suit.Sleeve = m8;
                        suit.Waist = m9;
                        suit.Wrist = m10;
                        suit.Crotch = m11;
                        suit.Hip = m12;
                        suit.Inseam = m13;
                        suit.Knee = m14;
                        suit.Length = m15;
                        suit.OutSeam = m16;
                        suit.Thigh = m17;
                        suit.OtherDetails = details_temp;
                        suit.StitchingJobId = job.JobId;

                        StitchingJobDetails jobdetails1 = new StitchingJobDetails();
                        StitchingJobDetails jobdetails2 = new StitchingJobDetails();
                        StitchingJobDetails jobdetails3 = new StitchingJobDetails();
                        if (a.FabricfromCustomer == 0)
                        {

                            jobdetails1.JobId = job.JobId;
                            jobdetails1.ItemCode = itemcode1;
                            jobdetails1.Quantity = quantity1.ToString();


                            jobdetails2.JobId = job.JobId;
                            jobdetails2.ItemCode = itemcode2;
                            jobdetails2.Quantity = quantity2.ToString();

                            jobdetails3.JobId = job.JobId;
                            jobdetails3.ItemCode = itemcode3;
                            jobdetails3.Quantity = quantity3.ToString();

                            a.StitchingJobDetails.Add(jobdetails1);
                            a.StitchingJobDetails.Add(jobdetails2);
                            a.StitchingJobDetails.Add(jobdetails3);
                        }


                        var myComplexObject = HttpContext.Session.GetObjectFromJson<List<AddStitchingJobViewModel>>("Test");
                        List<AddStitchingJobViewModel> temp_list = (List<AddStitchingJobViewModel>)myComplexObject;
                        temp_list.Add(a);
                        HttpContext.Session.SetObjectAsJson("Test", temp_list);
                        HttpContext.Session.SetObjectAsJson("Tempjob", a);
                        //HttpContext.Session.SetString("helo","bla");
                        //var obj = new AddStitchingJobViewModel();
                        //AddStitchingJobViewModel abc = (AddStitchingJobViewModel)obj;

                        await db.StitchingJob.AddAsync(job);
                        if (a.FabricfromCustomer == 0)
                        {
                            await db.StitchingJobDetails.AddAsync(jobdetails1);
                            await db.StitchingJobDetails.AddAsync(jobdetails2);
                            await db.StitchingJobDetails.AddAsync(jobdetails3);
                        }
                        await db.SuitMeasurements.AddAsync(suit);
                        await db.SaveChangesAsync();
                        ViewBag.successful = "yes";
                        return View(a);
                    }



                }
                else if (a.FabricfromCustomer == 1)
                {
                    StitchingJob job = new StitchingJob();
                    while (true)
                    {
                        int dup = 0;
                        Random rnd = new Random();
                        int rnd_number = rnd.Next(10000, 90000);

                        var temp = from i in db.StitchingJob
                                   where i.JobId == rnd_number
                                   select i;
                        foreach (var b in temp)
                        {
                            dup++;
                        }

                        if (dup == 0)
                        {
                            job.JobId = rnd_number;
                            break;
                        }
                    }

                    a.JobId = job.JobId;
                    job.JobTypeId = a.JobTypeId;
                    job.ShopCode = shop_code;
                    job.WarehouseStatus = 0;
                    job.CurrentStatus = 0;
                    job.FabricfromCustomer = a.FabricfromCustomer;
                    job.ExpectedDate = a.ExpectedDate;
                    job.CustomerId = customer_id;
                    job.OrderDate = System.DateTime.Now;


                    SuitMeasurements suit = new SuitMeasurements();
                    suit.Bicep = m1;
                    suit.Chest = m2;
                    suit.Collar = m3;
                    suit.FullBack = m4;
                    suit.HalfBack = m5;
                    suit.Length = m6;
                    suit.Shoulder = m7;
                    suit.Sleeve = m8;
                    suit.Waist = m9;
                    suit.Wrist = m10;
                    suit.Crotch = m11;
                    suit.Hip = m12;
                    suit.Inseam = m13;
                    suit.Knee = m14;
                    suit.Length = m15;
                    suit.OutSeam = m16;
                    suit.Thigh = m17;
                    suit.OtherDetails = details_temp;
                    suit.StitchingJobId = job.JobId;




                    var myComplexObject = HttpContext.Session.GetObjectFromJson<List<AddStitchingJobViewModel>>("Test");
                    List<AddStitchingJobViewModel> temp_list = (List<AddStitchingJobViewModel>)myComplexObject;
                    temp_list.Add(a);
                    HttpContext.Session.SetObjectAsJson("Test", temp_list);
                    HttpContext.Session.SetObjectAsJson("Tempjob", a);
                    //HttpContext.Session.SetString("helo","bla");
                    //var obj = new AddStitchingJobViewModel();
                    //AddStitchingJobViewModel abc = (AddStitchingJobViewModel)obj;

                    await db.StitchingJob.AddAsync(job);

                    await db.SuitMeasurements.AddAsync(suit);
                    await db.SaveChangesAsync();
                    ViewBag.successful = "yes";
                    return View(a);

                }

            }


            else if (JobTypeId == 2)
            {
                int count = 0;
                bool check = false;
                double m1 = 0, m2 = 0, m3 = 0, m4 = 0, m5 = 0, m6 = 0, m7 = 0, m8 = 0, m9 = 0, m10 = 0;
                if (a.PantItemCode1.HasValue)
                {
                    itemcode1 = a.PantItemCode1.Value;
                }
                if (a.PantItemCode2.HasValue)
                {
                    itemcode2 = a.PantItemCode2.Value;
                }
                if (a.PantQuantity1.HasValue)
                {
                    quantity1 = a.PantQuantity1.Value;
                    if (quantity1 <= 0)
                    {
                        ViewBag.pantquantity1 = "invalid quantity";
                        return View(a);
                    }
                }
                if (a.PantQuantity2.HasValue)
                {
                    quantity2 = a.PantQuantity2.Value;
                    if (quantity2 <= 0)
                    {
                        ViewBag.pantquantity2 = "invalid quantity";
                        return View(a);
                    }
                }

                if (a.PantMeasurements.Crotch.HasValue)
                {
                    m1 = a.PantMeasurements.Crotch.Value;
                }
                if (a.PantMeasurements.Hip.HasValue)
                {
                    m2 = a.PantMeasurements.Hip.Value;
                }
                if (a.PantMeasurements.Inseam.HasValue)
                {
                    m3 = a.PantMeasurements.Inseam.Value;
                }
                if (a.PantMeasurements.Knee.HasValue)
                {
                    m4 = a.PantMeasurements.Knee.Value;
                }
                if (a.PantMeasurements.OutSeam.HasValue)
                {
                    m5 = a.PantMeasurements.OutSeam.Value;
                }
                if (a.PantMeasurements.Thigh.HasValue)
                {
                    m6 = a.PantMeasurements.Thigh.Value;
                }
                if (a.PantMeasurements.Waist.HasValue)
                {
                    m7 = a.PantMeasurements.Waist.Value;
                }

                details_temp = a.PantMeasurements.OtherDetails;

                if (m1 <= 0 || m2 <= 0 || m3 <= 0 || m4 <= 0 || m5 <= 0 || m6 <= 0 || m7 <= 0)
                {
                    ViewBag.pantmeasurements = "Measurements cannot be empty or zero";
                    return View(a);
                }



                var temp_item1 = from u in db.Item
                                 where u.ItemCode == itemcode1 && u.WarehouseCode == warehouse_code
                                 select u;

                var temp_item2 = from u in db.Item
                                 where u.ItemCode == itemcode2 && u.WarehouseCode == warehouse_code
                                 select u;
                if (a.FabricfromCustomer == 0)
                {
                    foreach (var x in temp_item1)
                    {
                        count++;
                    }

                    if (count == 0)
                    {
                        ViewBag.pantitem1 = "item not found";
                        return View(a);
                    }

                    foreach (var x in temp_item2)
                    {
                        count++;
                    }

                    if (count == 0)
                    {
                        ViewBag.pantitem2 = "item not found";
                        return View(a);
                    }
                }

                if (count == 2 && a.FabricfromCustomer == 0)
                {
                    foreach (var x in temp_item1)
                    {
                        if (x.Quantity > quantity1)
                        {
                            check = true;
                        }
                        else
                        {
                            check = false;
                        }
                    }

                    foreach (var y in temp_item2)
                    {
                        if (y.Quantity > quantity2)
                        {
                            check = true;
                        }
                        else
                        {
                            check = false;
                        }
                    }


                    if (check)
                    {
                        if (a.FabricfromCustomer == 0)
                        {
                            foreach (var y in temp_item1)
                            {
                                y.Quantity = y.Quantity - quantity1;

                            }
                            //await db.SaveChangesAsync();
                            foreach (var x in temp_item2)
                            {
                                x.Quantity = x.Quantity - quantity2;

                            }
                        }
                        PantMeasurements pant = new PantMeasurements();
                        pant.Crotch = m1;
                        pant.Hip = m2;
                        pant.Inseam = m3;
                        pant.Knee = m4;
                        pant.OutSeam = m5;
                        pant.Thigh = m6;
                        pant.Waist = m7;
                        pant.OtherDetails = details_temp;


                        StitchingJob job = new StitchingJob();
                        while (true)
                        {
                            int dup = 0;
                            Random rnd = new Random();
                            int rnd_number = rnd.Next(10000, 90000);

                            var temp = from i in db.StitchingJob
                                       where i.JobId == rnd_number
                                       select i;
                            foreach (var b in temp)
                            {
                                dup++;
                            }

                            if (dup == 0)
                            {
                                job.JobId = rnd_number;
                                break;
                            }
                        }

                        a.JobId = job.JobId;
                        job.JobTypeId = a.JobTypeId;
                        job.ShopCode = shop_code;
                        job.WarehouseStatus = 0;
                        job.CurrentStatus = 0;
                        job.FabricfromCustomer = a.FabricfromCustomer;
                        job.ExpectedDate = a.ExpectedDate;
                        job.CustomerId = customer_id;
                        job.OrderDate = System.DateTime.Now;

                        pant.StitchingJobId = job.JobId;



                        StitchingJobDetails jobdetails1 = new StitchingJobDetails();
                        StitchingJobDetails jobdetails2 = new StitchingJobDetails();
                        if (a.FabricfromCustomer == 0)
                        {

                            jobdetails1.JobId = job.JobId;
                            jobdetails1.ItemCode = itemcode1;
                            jobdetails1.Quantity = quantity1.ToString();


                            jobdetails2.JobId = job.JobId;
                            jobdetails2.ItemCode = itemcode2;
                            jobdetails2.Quantity = quantity2.ToString();
                            a.StitchingJobDetails.Add(jobdetails1);
                            a.StitchingJobDetails.Add(jobdetails2);
                        }

                        var myComplexObject = HttpContext.Session.GetObjectFromJson<List<AddStitchingJobViewModel>>("Test");
                        List<AddStitchingJobViewModel> temp_list = (List<AddStitchingJobViewModel>)myComplexObject;
                        temp_list.Add(a);
                        HttpContext.Session.SetObjectAsJson("Test", temp_list);
                        HttpContext.Session.SetObjectAsJson("Tempjob", a);


                        await db.StitchingJob.AddAsync(job);
                        if (a.FabricfromCustomer == 0)
                        {
                            await db.StitchingJobDetails.AddAsync(jobdetails1);
                            await db.StitchingJobDetails.AddAsync(jobdetails2);
                        }
                        await db.PantMeasurements.AddAsync(pant);
                        await db.SaveChangesAsync();
                        ViewBag.successful = "yes";
                        return View(a);
                    }



                }

                else if (a.FabricfromCustomer == 1)
                {
                    PantMeasurements pant = new PantMeasurements();
                    pant.Crotch = m1;
                    pant.Hip = m2;
                    pant.Inseam = m3;
                    pant.Knee = m4;
                    pant.OutSeam = m5;
                    pant.Thigh = m6;
                    pant.Waist = m7;
                    pant.OtherDetails = details_temp;

                    StitchingJob job = new StitchingJob();
                    while (true)
                    {
                        int dup = 0;
                        Random rnd = new Random();
                        int rnd_number = rnd.Next(10000, 90000);

                        var temp = from i in db.StitchingJob
                                   where i.JobId == rnd_number
                                   select i;
                        foreach (var b in temp)
                        {
                            dup++;
                        }

                        if (dup == 0)
                        {
                            job.JobId = rnd_number;
                            break;
                        }
                    }

                    a.JobId = job.JobId;
                    job.JobTypeId = a.JobTypeId;
                    job.ShopCode = shop_code;
                    job.WarehouseStatus = 0;
                    job.CurrentStatus = 0;
                    job.FabricfromCustomer = a.FabricfromCustomer;
                    job.ExpectedDate = a.ExpectedDate;
                    job.CustomerId = customer_id;
                    job.OrderDate = System.DateTime.Now;


                    pant.StitchingJobId = job.JobId;




                    var myComplexObject = HttpContext.Session.GetObjectFromJson<List<AddStitchingJobViewModel>>("Test");
                    List<AddStitchingJobViewModel> temp_list = (List<AddStitchingJobViewModel>)myComplexObject;
                    temp_list.Add(a);
                    HttpContext.Session.SetObjectAsJson("Test", temp_list);
                    HttpContext.Session.SetObjectAsJson("Tempjob", a);


                    await db.StitchingJob.AddAsync(job);

                    await db.PantMeasurements.AddAsync(pant);
                    await db.SaveChangesAsync();
                    ViewBag.successful = "yes";
                    return View(a);

                }

            }


            else if (JobTypeId == 6)
            {
                int count = 0;
                bool check = false;
                double m1 = 0, m2 = 0, m3 = 0, m4 = 0, m5 = 0, m6 = 0, m7 = 0, m8 = 0, m9 = 0, m10 = 0;
                if (a.KurtaItemCode1.HasValue)
                {
                    itemcode1 = a.KurtaItemCode1.Value;
                }
                if (a.KurtaItemCode2.HasValue)
                {
                    itemcode2 = a.KurtaItemCode2.Value;
                }
                if (a.KurtaQuantity1.HasValue)
                {
                    quantity1 = a.KurtaQuantity1.Value;
                    if (quantity1 <= 0)
                    {
                        ViewBag.kurtaquantity1 = "invalid quantity";
                        return View(a);
                    }
                }
                if (a.KurtaQuantity2.HasValue)
                {
                    quantity2 = a.KurtaQuantity2.Value;
                    if (quantity2 <= 0)
                    {
                        ViewBag.kurtaquantity2 = "invalid quantity";
                        return View(a);
                    }
                }

                if (a.KurtaMeasurements.Length.HasValue)
                {
                    m1 = a.KurtaMeasurements.Length.Value;
                }
                if (a.KurtaMeasurements.Chest.HasValue)
                {
                    m2 = a.KurtaMeasurements.Chest.Value;
                }
                if (a.KurtaMeasurements.Daman.HasValue)
                {
                    m3 = a.KurtaMeasurements.Daman.Value;
                }
                if (a.KurtaMeasurements.Shoulder.HasValue)
                {
                    m4 = a.KurtaMeasurements.Shoulder.Value;
                }
                if (a.KurtaMeasurements.Sleeve.HasValue)
                {
                    m5 = a.KurtaMeasurements.Sleeve.Value;
                }
                if (a.KurtaMeasurements.Collar.HasValue)
                {
                    m6 = a.KurtaMeasurements.Collar.Value;
                }

                details_temp = a.KurtaMeasurements.OtherDetails;

                if (m1 <= 0 || m2 <= 0 || m3 <= 0 || m4 <= 0 || m5 <= 0 || m6 <= 0)
                {
                    ViewBag.kurtameasurements = "Measurements cannot be empty or zero";
                    return View(a);
                }



                var temp_item1 = from u in db.Item
                                 where u.ItemCode == itemcode1 && u.WarehouseCode == warehouse_code
                                 select u;

                var temp_item2 = from u in db.Item
                                 where u.ItemCode == itemcode2 && u.WarehouseCode == warehouse_code
                                 select u;
                if (a.FabricfromCustomer == 0)
                {
                    foreach (var x in temp_item1)
                    {
                        count++;
                    }

                    if (count == 0)
                    {
                        ViewBag.kurtaitem1 = "item not found";
                        return View(a);
                    }

                    foreach (var x in temp_item2)
                    {
                        count++;
                    }

                    if (count == 0)
                    {
                        ViewBag.kurtaitem2 = "item not found";
                        return View(a);
                    }
                }

                if (count == 2 && a.FabricfromCustomer == 0)
                {
                    foreach (var x in temp_item1)
                    {
                        if (x.Quantity > quantity1)
                        {
                            check = true;
                        }
                        else
                        {
                            check = false;
                        }
                    }

                    foreach (var y in temp_item2)
                    {
                        if (y.Quantity > quantity2)
                        {
                            check = true;
                        }
                        else
                        {
                            check = false;
                        }
                    }


                    if (check)
                    {
                        if (a.FabricfromCustomer == 0)
                        {
                            foreach (var y in temp_item1)
                            {
                                y.Quantity = y.Quantity - quantity1;

                            }
                            //await db.SaveChangesAsync();
                            foreach (var x in temp_item2)
                            {
                                x.Quantity = x.Quantity - quantity2;

                            }
                        }
                        KurtaMeasurements kurta = new KurtaMeasurements();
                        kurta.Length = m1;
                        kurta.Chest = m2;
                        kurta.Daman = m3;
                        kurta.Shoulder = m4;
                        kurta.Sleeve = m5;
                        kurta.Collar = m6;
                        kurta.OtherDetails = details_temp;

                        StitchingJob job = new StitchingJob();
                        while (true)
                        {
                            int dup = 0;
                            Random rnd = new Random();
                            int rnd_number = rnd.Next(10000, 90000);

                            var temp = from i in db.StitchingJob
                                       where i.JobId == rnd_number
                                       select i;
                            foreach (var b in temp)
                            {
                                dup++;
                            }

                            if (dup == 0)
                            {
                                job.JobId = rnd_number;
                                break;
                            }
                        }

                        a.JobId = job.JobId;
                        job.JobTypeId = a.JobTypeId;
                        job.ShopCode = shop_code;
                        job.WarehouseStatus = 0;
                        job.CurrentStatus = 0;
                        job.FabricfromCustomer = a.FabricfromCustomer;
                        job.ExpectedDate = a.ExpectedDate;
                        job.CustomerId = customer_id;
                        job.OrderDate = System.DateTime.Now;

                        kurta.StitchingJobId = job.JobId;



                        StitchingJobDetails jobdetails1 = new StitchingJobDetails();
                        StitchingJobDetails jobdetails2 = new StitchingJobDetails();
                        if (a.FabricfromCustomer == 0)
                        {

                            jobdetails1.JobId = job.JobId;
                            jobdetails1.ItemCode = itemcode1;
                            jobdetails1.Quantity = quantity1.ToString();


                            jobdetails2.JobId = job.JobId;
                            jobdetails2.ItemCode = itemcode2;
                            jobdetails2.Quantity = quantity2.ToString();
                            a.StitchingJobDetails.Add(jobdetails1);
                            a.StitchingJobDetails.Add(jobdetails2);
                        }

                        var myComplexObject = HttpContext.Session.GetObjectFromJson<List<AddStitchingJobViewModel>>("Test");
                        List<AddStitchingJobViewModel> temp_list = (List<AddStitchingJobViewModel>)myComplexObject;
                        temp_list.Add(a);
                        HttpContext.Session.SetObjectAsJson("Test", temp_list);
                        HttpContext.Session.SetObjectAsJson("Tempjob", a);


                        await db.StitchingJob.AddAsync(job);
                        if (a.FabricfromCustomer == 0)
                        {
                            await db.StitchingJobDetails.AddAsync(jobdetails1);
                            await db.StitchingJobDetails.AddAsync(jobdetails2);
                        }
                        await db.KurtaMeasurements.AddAsync(kurta);
                        await db.SaveChangesAsync();
                        ViewBag.successful = "yes";
                        return View(a);
                    }



                }
                else if (a.FabricfromCustomer == 1)
                {
                    KurtaMeasurements kurta = new KurtaMeasurements();
                    kurta.Length = m1;
                    kurta.Chest = m2;
                    kurta.Daman = m3;
                    kurta.Shoulder = m4;
                    kurta.Sleeve = m5;
                    kurta.Collar = m6;
                    kurta.OtherDetails = details_temp;

                    StitchingJob job = new StitchingJob();
                    while (true)
                    {
                        int dup = 0;
                        Random rnd = new Random();
                        int rnd_number = rnd.Next(10000, 90000);

                        var temp = from i in db.StitchingJob
                                   where i.JobId == rnd_number
                                   select i;
                        foreach (var b in temp)
                        {
                            dup++;
                        }

                        if (dup == 0)
                        {
                            job.JobId = rnd_number;
                            break;
                        }
                    }

                    a.JobId = job.JobId;
                    job.JobTypeId = a.JobTypeId;
                    job.ShopCode = shop_code;
                    job.WarehouseStatus = 0;
                    job.CurrentStatus = 0;
                    job.FabricfromCustomer = a.FabricfromCustomer;
                    job.ExpectedDate = a.ExpectedDate;
                    job.CustomerId = customer_id;
                    job.OrderDate = System.DateTime.Now;

                    kurta.StitchingJobId = job.JobId;






                    var myComplexObject = HttpContext.Session.GetObjectFromJson<List<AddStitchingJobViewModel>>("Test");
                    List<AddStitchingJobViewModel> temp_list = (List<AddStitchingJobViewModel>)myComplexObject;
                    temp_list.Add(a);
                    HttpContext.Session.SetObjectAsJson("Test", temp_list);
                    HttpContext.Session.SetObjectAsJson("Tempjob", a);


                    await db.StitchingJob.AddAsync(job);

                    await db.KurtaMeasurements.AddAsync(kurta);
                    await db.SaveChangesAsync();
                    ViewBag.successful = "yes";
                    return View(a);
                }

            }


            else if (JobTypeId == 4)
            {
                int count = 0;
                bool check = false;
                double m1 = 0, m2 = 0, m3 = 0, m4 = 0, m5 = 0, m6 = 0, m7 = 0, m8 = 0, m9 = 0, m10 = 0;
                if (a.BlazerItemCode1.HasValue)
                {
                    itemcode1 = a.BlazerItemCode1.Value;
                }
                if (a.BlazerItemCode2.HasValue)
                {
                    itemcode2 = a.BlazerItemCode2.Value;
                }
                if (a.BlazerQuantity1.HasValue)
                {
                    quantity1 = a.BlazerQuantity1.Value;
                    if (quantity1 <= 0)
                    {
                        ViewBag.blazerquantity1 = "invalid quantity";
                        return View(a);
                    }
                }
                if (a.BlazerQuantity2.HasValue)
                {
                    quantity2 = a.BlazerQuantity2.Value;
                    if (quantity2 <= 0)
                    {
                        ViewBag.blazerquantity2 = "invalid quantity";
                        return View(a);
                    }
                }

                if (a.BlazerMeasurements.Length.HasValue)
                {
                    m1 = a.BlazerMeasurements.Length.Value;
                }
                if (a.BlazerMeasurements.Chest.HasValue)
                {
                    m2 = a.BlazerMeasurements.Chest.Value;
                }
                if (a.BlazerMeasurements.Bicep.HasValue)
                {
                    m3 = a.BlazerMeasurements.Bicep.Value;
                }
                if (a.BlazerMeasurements.Shoulder.HasValue)
                {
                    m4 = a.BlazerMeasurements.Shoulder.Value;
                }
                if (a.BlazerMeasurements.Sleeve.HasValue)
                {
                    m5 = a.BlazerMeasurements.Sleeve.Value;
                }
                if (a.BlazerMeasurements.Wrist.HasValue)
                {
                    m6 = a.BlazerMeasurements.Wrist.Value;
                }


                details_temp = a.BlazerMeasurements.OtherDetails;

                if (m1 <= 0 || m2 <= 0 || m3 <= 0 || m4 <= 0 || m5 <= 0 || m6 <= 0)
                {
                    ViewBag.blazermeasurements = "Measurements cannot be empty or zero";
                    return View(a);
                }



                var temp_item1 = from u in db.Item
                                 where u.ItemCode == itemcode1 && u.WarehouseCode == warehouse_code
                                 select u;

                var temp_item2 = from u in db.Item
                                 where u.ItemCode == itemcode2 && u.WarehouseCode == warehouse_code
                                 select u;
                if (a.FabricfromCustomer == 0)
                {
                    foreach (var x in temp_item1)
                    {
                        count++;
                    }

                    if (count == 0)
                    {
                        ViewBag.blazeritem1 = "item not found";
                        return View(a);
                    }

                    foreach (var x in temp_item2)
                    {
                        count++;
                    }

                    if (count == 0)
                    {
                        ViewBag.blazeritem2 = "item not found";
                        return View(a);
                    }
                }

                if (count == 2 && a.FabricfromCustomer == 0)
                {
                    foreach (var x in temp_item1)
                    {
                        if (x.Quantity > quantity1)
                        {
                            check = true;
                        }
                        else
                        {
                            check = false;
                        }
                    }

                    foreach (var y in temp_item2)
                    {
                        if (y.Quantity > quantity2)
                        {
                            check = true;
                        }
                        else
                        {
                            check = false;
                        }
                    }


                    if (check)
                    {
                        if (a.FabricfromCustomer == 0)
                        {
                            foreach (var y in temp_item1)
                            {
                                y.Quantity = y.Quantity - quantity1;

                            }
                            //await db.SaveChangesAsync();
                            foreach (var x in temp_item2)
                            {
                                x.Quantity = x.Quantity - quantity2;

                            }
                        }
                        BlazerMeasurements blazer = new BlazerMeasurements();
                        blazer.Length = m1;

                        blazer.Chest = m2;
                        blazer.Bicep = m3;
                        blazer.Shoulder = m4;

                        blazer.Sleeve = m5;
                        blazer.Wrist = m6;
                        blazer.OtherDetails = details_temp;


                        StitchingJob job = new StitchingJob();
                        while (true)
                        {
                            int dup = 0;
                            Random rnd = new Random();
                            int rnd_number = rnd.Next(10000, 90000);

                            var temp = from i in db.StitchingJob
                                       where i.JobId == rnd_number
                                       select i;
                            foreach (var b in temp)
                            {
                                dup++;
                            }

                            if (dup == 0)
                            {
                                job.JobId = rnd_number;
                                break;
                            }
                        }

                        a.JobId = job.JobId;
                        job.JobTypeId = a.JobTypeId;
                        job.ShopCode = shop_code;
                        job.WarehouseStatus = 0;
                        job.CurrentStatus = 0;
                        job.FabricfromCustomer = a.FabricfromCustomer;
                        job.ExpectedDate = a.ExpectedDate;
                        job.CustomerId = customer_id;
                        job.OrderDate = System.DateTime.Now;

                        blazer.StitchingJobId = job.JobId;



                        StitchingJobDetails jobdetails1 = new StitchingJobDetails();
                        StitchingJobDetails jobdetails2 = new StitchingJobDetails();
                        if (a.FabricfromCustomer == 0)
                        {

                            jobdetails1.JobId = job.JobId;
                            jobdetails1.ItemCode = itemcode1;
                            jobdetails1.Quantity = quantity1.ToString();


                            jobdetails2.JobId = job.JobId;
                            jobdetails2.ItemCode = itemcode2;
                            jobdetails2.Quantity = quantity2.ToString();
                            a.StitchingJobDetails.Add(jobdetails1);
                            a.StitchingJobDetails.Add(jobdetails2);
                        }

                        var myComplexObject = HttpContext.Session.GetObjectFromJson<List<AddStitchingJobViewModel>>("Test");
                        List<AddStitchingJobViewModel> temp_list = (List<AddStitchingJobViewModel>)myComplexObject;
                        temp_list.Add(a);
                        HttpContext.Session.SetObjectAsJson("Test", temp_list);
                        HttpContext.Session.SetObjectAsJson("Tempjob", a);


                        await db.StitchingJob.AddAsync(job);
                        if (a.FabricfromCustomer == 0)
                        {
                            await db.StitchingJobDetails.AddAsync(jobdetails1);
                            await db.StitchingJobDetails.AddAsync(jobdetails2);
                        }
                        await db.BlazerMeasurements.AddAsync(blazer);
                        await db.SaveChangesAsync();
                        ViewBag.successful = "yes";
                        return View(a);
                    }



                }

                else if (a.FabricfromCustomer == 1)
                {
                    BlazerMeasurements blazer = new BlazerMeasurements();
                    blazer.Length = m1;

                    blazer.Chest = m2;
                    blazer.Bicep = m3;
                    blazer.Shoulder = m4;

                    blazer.Sleeve = m5;
                    blazer.Wrist = m6;
                    blazer.OtherDetails = details_temp;


                    StitchingJob job = new StitchingJob();
                    while (true)
                    {
                        int dup = 0;
                        Random rnd = new Random();
                        int rnd_number = rnd.Next(10000, 90000);

                        var temp = from i in db.StitchingJob
                                   where i.JobId == rnd_number
                                   select i;
                        foreach (var b in temp)
                        {
                            dup++;
                        }

                        if (dup == 0)
                        {
                            job.JobId = rnd_number;
                            break;
                        }
                    }

                    a.JobId = job.JobId;
                    job.JobTypeId = a.JobTypeId;
                    job.ShopCode = shop_code;
                    job.WarehouseStatus = 0;
                    job.CurrentStatus = 0;
                    job.FabricfromCustomer = a.FabricfromCustomer;
                    job.ExpectedDate = a.ExpectedDate;
                    job.CustomerId = customer_id;
                    job.OrderDate = System.DateTime.Now;

                    blazer.StitchingJobId = job.JobId;



                    var myComplexObject = HttpContext.Session.GetObjectFromJson<List<AddStitchingJobViewModel>>("Test");
                    List<AddStitchingJobViewModel> temp_list = (List<AddStitchingJobViewModel>)myComplexObject;
                    temp_list.Add(a);
                    HttpContext.Session.SetObjectAsJson("Test", temp_list);
                    HttpContext.Session.SetObjectAsJson("Tempjob", a);


                    await db.StitchingJob.AddAsync(job);

                    await db.BlazerMeasurements.AddAsync(blazer);
                    await db.SaveChangesAsync();
                    ViewBag.successful = "yes";
                    return View(a);

                }

            }


            else if (JobTypeId == 5)
            {
                int count = 0;
                bool check = false;
                double m1 = 0, m2 = 0, m3 = 0, m4 = 0, m5 = 0, m6 = 0, m7 = 0, m8 = 0, m9 = 0, m10 = 0;
                if (a.ShalwarItemCode1.HasValue)
                {
                    itemcode1 = a.ShalwarItemCode1.Value;
                }

                if (a.ShalwarQuantity1.HasValue)
                {
                    quantity1 = a.ShalwarQuantity1.Value;
                    if (quantity1 <= 0)
                    {
                        ViewBag.shalwarquantity1 = "invalid quantity";
                        return View(a);
                    }
                }

                if (a.ShalwarMeasurements.Length.HasValue)
                {
                    m1 = a.ShalwarMeasurements.Length.Value;
                }
                if (a.ShalwarMeasurements.Bottom.HasValue)
                {
                    m2 = a.ShalwarMeasurements.Bottom.Value;
                }
                if (a.ShalwarMeasurements.Waist.HasValue)
                {
                    m3 = a.ShalwarMeasurements.Waist.Value;
                }

                details_temp = a.ShalwarMeasurements.OtherDetails;

                if (m1 <= 0 || m2 <= 0 || m3 <= 0)
                {
                    ViewBag.shalwarmeasurements = "Measurements cannot be empty or zero";
                    return View(a);
                }



                var temp_item1 = from u in db.Item
                                 where u.ItemCode == itemcode1 && u.WarehouseCode == warehouse_code
                                 select u;
                if (a.FabricfromCustomer == 0)
                {

                    foreach (var x in temp_item1)
                    {
                        count++;
                    }

                    if (count == 0)
                    {
                        ViewBag.shalwaritem1 = "item not found";
                        return View(a);
                    }


                }

                if (count == 1 && a.FabricfromCustomer == 0)
                {
                    foreach (var x in temp_item1)
                    {
                        if (x.Quantity > quantity1)
                        {
                            check = true;
                        }
                        else
                        {
                            check = false;
                        }
                    }



                    if (check)
                    {
                        if (a.FabricfromCustomer == 0)
                        {
                            foreach (var y in temp_item1)
                            {
                                y.Quantity = y.Quantity - quantity1;

                            }
                            //await db.SaveChangesAsync();

                        }
                        ShalwarMeasurements shalwar = new ShalwarMeasurements();
                        shalwar.Length = m1;
                        shalwar.Bottom = m2;
                        shalwar.Waist = m3;
                        shalwar.OtherDetails = details_temp;

                        StitchingJob job = new StitchingJob();
                        while (true)
                        {
                            int dup = 0;
                            Random rnd = new Random();
                            int rnd_number = rnd.Next(10000, 90000);

                            var temp = from i in db.StitchingJob
                                       where i.JobId == rnd_number
                                       select i;
                            foreach (var b in temp)
                            {
                                dup++;
                            }

                            if (dup == 0)
                            {
                                job.JobId = rnd_number;
                                break;
                            }
                        }

                        a.JobId = job.JobId;
                        job.JobTypeId = a.JobTypeId;
                        job.ShopCode = shop_code;
                        job.WarehouseStatus = 0;
                        job.CurrentStatus = 0;
                        job.FabricfromCustomer = a.FabricfromCustomer;
                        job.ExpectedDate = a.ExpectedDate;
                        job.CustomerId = customer_id;
                        job.OrderDate = System.DateTime.Now;

                        shalwar.StitchingJobId = job.JobId;



                        StitchingJobDetails jobdetails1 = new StitchingJobDetails();

                        if (a.FabricfromCustomer == 0)
                        {

                            jobdetails1.JobId = job.JobId;
                            jobdetails1.ItemCode = itemcode1;
                            jobdetails1.Quantity = quantity1.ToString();



                            a.StitchingJobDetails.Add(jobdetails1);

                        }

                        var myComplexObject = HttpContext.Session.GetObjectFromJson<List<AddStitchingJobViewModel>>("Test");
                        List<AddStitchingJobViewModel> temp_list = (List<AddStitchingJobViewModel>)myComplexObject;
                        temp_list.Add(a);
                        HttpContext.Session.SetObjectAsJson("Test", temp_list);
                        HttpContext.Session.SetObjectAsJson("Tempjob", a);


                        await db.StitchingJob.AddAsync(job);
                        if (a.FabricfromCustomer == 0)
                        {
                            await db.StitchingJobDetails.AddAsync(jobdetails1);

                        }
                        await db.ShalwarMeasurements.AddAsync(shalwar);
                        await db.SaveChangesAsync();
                        ViewBag.successful = "yes";
                        return View(a);
                    }



                }

                else if (a.FabricfromCustomer == 1)
                {
                    ShalwarMeasurements shalwar = new ShalwarMeasurements();
                    shalwar.Length = m1;
                    shalwar.Bottom = m2;
                    shalwar.Waist = m3;
                    shalwar.OtherDetails = details_temp;

                    StitchingJob job = new StitchingJob();

                    while (true)
                    {
                        int dup = 0;
                        Random rnd = new Random();
                        int rnd_number = rnd.Next(10000, 90000);

                        var temp = from i in db.StitchingJob
                                   where i.JobId == rnd_number
                                   select i;
                        foreach (var b in temp)
                        {
                            dup++;
                        }

                        if (dup == 0)
                        {
                            job.JobId = rnd_number;
                            break;
                        }
                    }


                    a.JobId = job.JobId;
                    job.JobTypeId = a.JobTypeId;
                    job.ShopCode = shop_code;
                    job.WarehouseStatus = 0;
                    job.CurrentStatus = 0;
                    job.FabricfromCustomer = a.FabricfromCustomer;
                    job.ExpectedDate = a.ExpectedDate;
                    job.CustomerId = customer_id;
                    job.OrderDate = System.DateTime.Now;


                    shalwar.StitchingJobId = job.JobId;


                    var myComplexObject = HttpContext.Session.GetObjectFromJson<List<AddStitchingJobViewModel>>("Test");
                    List<AddStitchingJobViewModel> temp_list = (List<AddStitchingJobViewModel>)myComplexObject;
                    temp_list.Add(a);
                    HttpContext.Session.SetObjectAsJson("Test", temp_list);
                    HttpContext.Session.SetObjectAsJson("Tempjob", a);


                    await db.StitchingJob.AddAsync(job);

                    await db.ShalwarMeasurements.AddAsync(shalwar);
                    await db.SaveChangesAsync();
                    ViewBag.successful = "yes";
                    return View(a);
                }



            }



            //return RedirectToAction("InvoiceGeneration");
            return View(a);

        }

        //Takes as input customer phonenumber and displays a list of customer orders.
        [HttpPost]
        public IActionResult CustomerOrderHistory(string customerid)
        {
            int customer_id = 0;
            int customer_count = 0;
            var temp_customer = from u in db.Users
                                where u.UserId == customerid
                                select u;

            foreach (var x in temp_customer)
            {
                customer_id = x.Id;
                customer_count++;
            }

            if (customer_count == 0)
            {
                ViewBag.customernotfound = "customer not found";
                return View(customerid);
            }

            CustomerHistory customerhistory = new CustomerHistory(db);
            List<HistoryItem> historyitems = customerhistory.getHistory(customer_id);
            return View(historyitems);


        }

        //redirects to the customer order history page
        [HttpGet]
        public IActionResult CustomerOrderHistory()
        {


            return View(new List<HistoryItem>());


        }

        //takes as input a stitching job id and displays order details of the customer.
        [HttpGet]
        public ActionResult OrderDetails(int stitchingjobid)
        {


            CustomerHistory customerhistory = new CustomerHistory(db);
            HistoryItem item = customerhistory.getOrderDetails(stitchingjobid);
            return View(item);
        }

    }
}
