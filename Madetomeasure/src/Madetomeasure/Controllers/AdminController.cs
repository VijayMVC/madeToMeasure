using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Madetomeasure.Models.Admin;
using Madetomeasure.Data;
using Microsoft.AspNetCore.Http;

//Author: Khizar Iqbal
//Last Edited: 2/12/2016
//This Controller is used to manage the customer account and interation with system
//A customer is redirected here after login
//This Controller is used to manage the admin account and interation with system
//The admin is redirected here after login


namespace Madetomeasure.Controllers
{
    public class AdminController : Controller
    {
        private MadeToMeasureContext _context;

        public AdminController(MadeToMeasureContext context)
        {
            _context = context;
        }
        public IActionResult AddShop()
        {


            return View();
        }

        public async Task<IActionResult> AddNewShop(NewShop newshop)
        {


            await newshop.addShop(_context);
            // TempData
            TempData["Message"] = "shop";

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AddNewWarehouse(NewWarehouse newwarehouse)
        {
            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }




            await newwarehouse.AddWarehouse(_context);
            TempData["Message"] = "warehouse";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AddNewStitchingunit(NewStitching newstitchingunit)
        {

            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }



            await newstitchingunit.addStitching(_context);
            TempData["Message"] = "stitchingunit";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> addsthead(addnewstdphead add) {
            if (add.StitchingunitId == 0) {
                TempData["Message"] = "nobusiness";

               return  RedirectToAction("addnewstdphead"); }
            string name = TempData["name"].ToString();
            string userid = TempData["userId"].ToString();
            DateTime dat = (DateTime)TempData["date"];
            string password = TempData["password"].ToString();
            /*
             * 
             *  <option value="Cutting" class="text">Cutting</option>
                                                <option value="Embroidery" class="text">Embroidery</option>

                                                <option value="Stitching" class="text">Stitching</option>
                                                <option value="QualityAssurance" class="text">Quality Assurance</option>
                                                <option value="Packing" class="text">Packing</option>

             * 
             * */
            int id = add.StitchingunitId;
            string dp = add.Department;
            int idof = 0;
            if (dp == "Cutting") { idof = 1; }
           else if (dp == "Embroidery") { idof = 2; }
                else if (dp == "Stitching") { idof = 3; }
                else if (dp == "QualityAssurance") { idof = 4; }
                else if (dp == "Packing") { idof = 5; }

            await add.addstdp(_context, name, userid, dat, password, id, idof);

            return RedirectToAction("Index");

        }
        public async Task<IActionResult> AddNewEmployeeBu(EmployeeBusinessEntity newemployeeentity)
        {

            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }




            string name = TempData["name"].ToString();
            string userid = TempData["userId"].ToString();
            DateTime dat = (DateTime)TempData["date"];
            string password = TempData["password"].ToString();
            int works = newemployeeentity.work;

            string designa = TempData["designation"].ToString();
            int entitycode = 0;
            if (designa == "stunitmngr") { entitycode = 4; }
            if (designa == "stunithead") { entitycode = 5; }
            if (designa == "stunitemployee") { entitycode = 6; }
            if (designa == "shopkeeper") { entitycode = 2; }
            if (designa == "warehousemanager" ) { entitycode = 3; }

            if (newemployeeentity.work == 0) { TempData["Message"] = "nobusiness";
                if (TempData["designation"].ToString() == "shopkeeper") { return RedirectToAction("SelectBusinessEntity", new { type = 1 }); }

                else if (TempData["designation"].ToString() == "warehousemanager") { return RedirectToAction("SelectBusinessEntity", new { type = 2 }); }

                else if (TempData["designation"].ToString() == "stunithead" || TempData["designation"].ToString() == "stunitmngr" || TempData["designation"].ToString() == "stunitemployee") { return RedirectToAction("SelectBusinessEntity", new { type = 3 }); }



                return RedirectToAction("SelectBusinessEntity"); }
            await newemployeeentity.addemployeecom(_context,name,userid,dat,password,works,entitycode);
            TempData["Message"] = "employee";
            // TempData["Message"] = "stitchingunit";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AddNewStEmployee(AddStitchingunitEmployee newemployeeentity)
        {

            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }




            int id = newemployeeentity.StitchingunitId;
            if (id == 0)
            {

                TempData["Message"] = "nostunit";
                return RedirectToAction("AddStitchingunitEmployee");
            }
            int duty = 0;
            if (newemployeeentity.Department == "Cutting") { duty = 1; }
            if (newemployeeentity.Department == "Embroidery") { duty = 2; }
            if (newemployeeentity.Department == "Stitching") { duty = 3; }
            if (newemployeeentity.Department == "QualityAssurance") { duty = 4; }
            if (newemployeeentity.Department == "Packing") { duty = 5; }

            await newemployeeentity.AddStunitEmployee(_context, newemployeeentity.Name, id, duty);
            TempData["Message"] = "stunit";
            return RedirectToAction("Index");





        }
        //Action method to display profile information of a warehouse manager
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddEmployee()

        {
           
            return View();
        }
        public IActionResult AddStitchingunitEmployee()
            
        {


            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }




            int type = 3;//forstitchingunit

            GetEntity ga = new GetEntity(_context);
            IEnumerable<BusinessEntity> categories = ga.getBussinessEntities(type);
            ViewData["stunitid"] = categories;
            return View();
        }


        public IActionResult SelectBusinessEntity(int  type)
    {

            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }
            

            //  @model List< MadetoMeasureT.Models.BusinessEntity >

            GetEntity ga = new GetEntity(_context);
            IEnumerable<BusinessEntity> categories = ga.getBussinessEntities(type);
            //  List<BusinessEntity> blist = new List<BusinessEntity>();
            //   blist = ga.getBussinessEntities(type);
            ViewData["BusinessEntity"] = categories;
            return View();
    }

        public IActionResult AddStitchingunit()
        {
            return View();
        }
        public async Task<IActionResult> AddNewEmployee(NewEmployee newemployee)
        {
            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }





            await newemployee.addEmployee(_context);

            TempData["name"] = newemployee.Name;
            TempData["userid"] = newemployee.UserId;     
            TempData["date"] = newemployee.JoiningDate;
            TempData["password"] = newemployee.Password;

            TempData["designation"] = newemployee.designation;
            DateTime a;
           
            if (newemployee.JoiningDate < DateTime.Now.Date) {
                TempData["dateinvalid"] = "true";
                return RedirectToAction("AddEmployee");

            }
         
         GetUserId newemploye = new GetUserId(_context);
            int listcount=0;
           listcount = newemploye.getuserid(newemployee.UserId.ToString());
            if (listcount != 0){
                TempData["alreadypresent"] = "true";
                return RedirectToAction("AddEmployee");


            }
            

            if (newemployee.designation == "shopkeeper") { return RedirectToAction("SelectBusinessEntity", new { type = 1 }); }

            else if (newemployee.designation == "warehousemanager") { return RedirectToAction("SelectBusinessEntity", new { type = 2 }); }

            else if ( newemployee.designation == "stunitmngr"|| newemployee.designation == "stunitemployee") { return RedirectToAction("SelectBusinessEntity", new { type = 3}); }
            else if (newemployee.designation == "stunithead") { return RedirectToAction("stunitdphead", new { type = 3 }); }
            else
            {
                return RedirectToAction("Index");
            }

            
           
        }
        public IActionResult stunitdphead(int type) {

            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }


            //  @model List< MadetoMeasureT.Models.BusinessEntity >

            GetEntity ga = new GetEntity(_context);
            IEnumerable<BusinessEntity> categories = ga.getBussinessEntities(type);
            //  List<BusinessEntity> blist = new List<BusinessEntity>();
            //   blist = ga.getBussinessEntities(type);
            ViewData["stunitid"] = categories;
            return View();



            }
        public async Task<IActionResult> LinkShopware(LinkShopWarehous newemployee)
        {


            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }




            if (newemployee.shopc == 0)
            { TempData["Message"] = "shoperror";
                return RedirectToAction("LinkShopWarehouse");

            }
            if (newemployee.warehousec == 0)
            {
                TempData["Message"] = "warehouseerror";
                return RedirectToAction("LinkShopWarehouse");
            }

                await newemployee.addShop(_context, newemployee.shopc, newemployee.warehousec);
            TempData["Message"] = "linkshopwarehouse";
            return RedirectToAction("Index");



        }
        public async Task<IActionResult> LinkStitchingunitware(LinkStitchingunitware newemployee)
        {

            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }





            if (newemployee.stitchingco == 0) { TempData["Message"] = "stitchingerror";
                return RedirectToAction("LinkStitchingunitWarehouse");

            }
            if (newemployee.warehouseco == 0) { TempData["Message"] = "warehouseerror";
                return RedirectToAction("LinkStitchingunitWarehouse");

            }

            await newemployee.add(_context, newemployee.stitchingco, newemployee.warehouseco);
            TempData["Message"] = "linkstitchingunitware";
            return RedirectToAction("Index");



        }
       

        public IActionResult AddWarehouse()
        {
            return View();
        }

        public IActionResult LinkShopStitchingunit()
        {
            return View();
        }

        public IActionResult LinkShopWarehouse()

        {

            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }





            Gettablewarehouse get = new Gettablewarehouse(_context);
     
          IEnumerable<tableshopwarehouse> bussinessEntitiesList = get.gettable();
            int type = 2;//for warehouse
            GetEntity ga = new GetEntity(_context);
            IEnumerable<BusinessEntity> warehouses = ga.getBussinessEntities(type);
            ViewData["warehousfree"] = warehouses;
            type = 1;
            IEnumerable<BusinessEntity> shops= ga.getBussinessEntities(type);
            IEnumerable<BusinessEntity> shopsnotassociated = ga.getnotassociatedshops();
            ViewData["shopsfree"] = shopsnotassociated;
            ViewData["table"] = bussinessEntitiesList;

            return View();
        }

        public IActionResult LinkStitchingunitWarehouse()
        {

            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }





            Gettablestitchingunitwarehouse get = new Gettablestitchingunitwarehouse(_context);
            IEnumerable<Tablestitchingunitwarehouse> bussinessEntitiesList = get.gettable();

            GetEntity ga = new GetEntity(_context);
          
           
      
            IEnumerable<BusinessEntity> stitchingunitnotassociated = ga.getnotassociatedstitchingunit();
            ViewData["stitchingunitfree"] = stitchingunitnotassociated;
            IEnumerable<BusinessEntity> warehousenotassociated = ga.getnotassociatedwarehouse();
            ViewData["warehousefree"] = warehousenotassociated;
            ViewData["tablestitching"] = bussinessEntitiesList;
            return View();
        }

        public IActionResult SelectShop()
        {
            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }
            
            int type = 1;//for shop
            GetEntity get = new GetEntity(_context);
            IEnumerable<BusinessEntity> shops = get.getBussinessEntities(type);
            ViewData["Allshops"] = shops;

            return View();
        }
        
        public IActionResult OrderReport()
        {
            return View();
        }
       
        public JsonResult SelectShopor(int EntityCode)
        {


            if (EntityCode == 0)
            {
                TempData["noshop"] = "true";
                return null;

            }

            SelectShopOrder s = new SelectShopOrder(_context);


            List<int> a = new List<int>();
            int[] array = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<StitchingJob> list = s.getreport(EntityCode);
            foreach (StitchingJob sj in list)
            {
                DateTime date = sj.OrderDate;
                if (date.Year == 2016)
                {
                    if (date.Month == 1) { array[0] = array[0] + 1; }
                    if (date.Month == 2) { array[1] = array[1] + 1; }
                    if (date.Month == 3) { array[2] = array[2] + 1; }
                    if (date.Month == 4) { array[3] = array[3] + 1; }
                    if (date.Month == 5) { array[4] = array[4] + 1; }
                    if (date.Month == 6) { array[5] = array[5] + 1; }
                    if (date.Month == 7) { array[6] = array[6] + 1; }
                    if (date.Month == 8) { array[7] = array[7] + 1; }

                    if (date.Month == 9) { array[8] = array[8] + 1; }
                    if (date.Month == 10) { array[9] = array[9] + 1; }
                    if (date.Month == 11) { array[10] = array[10] + 1; }
                    if (date.Month == 12) { array[11] = array[11] + 1; }


                }
            }




            return Json(array);


        }
        
    }
}