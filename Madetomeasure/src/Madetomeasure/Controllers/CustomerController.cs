using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Madetomeasure.Models.StUnitAndCustomer;
using Madetomeasure.Data;
using Microsoft.AspNetCore.Http;


//Author: Ryuzaki Sultan
//Last Edited: 2/12/2016
//This Controller is used to manage the customer account and interation with system
//A customer is redirected here after login


namespace Madetomeasure.Controllers {
    public class CustomerController : Controller {
        private MadeToMeasureContext _context;

        public CustomerController(MadeToMeasureContext context) {
            _context = context;

        }

        //Action method to obtain history of customer orders
        [HttpGet]
        public ActionResult History() {

            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }


            int customerid = Convert.ToInt32( HttpContext.Session.GetString("Id"));//get customer id from session customer id is phone number 
            CustomerHistory customerhistory = new CustomerHistory(_context);
            List<HistoryItem> historyitems = customerhistory.getHistory(customerid);
            return View(historyitems);
        }

        
        //Action method to obtain the details of order clicked in pending order or hisotory orders
        [HttpGet]
        public ActionResult OrderDetails(int stitchingjobid) {
            CustomerHistory customerhistory = new CustomerHistory(_context);
            HistoryItem item = customerhistory.getOrderDetails(stitchingjobid);
            return View(item);
        }

        [HttpGet]
        public ActionResult PendingOrders() {
            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }
            
            int customerid = Convert.ToInt32(HttpContext.Session.GetString("Id")); //get customer id from session customer id is phone number 
            CustomerHistory customerhistory = new CustomerHistory(_context);
            List<HistoryItem> historyitems = customerhistory.getPendingOrders(customerid);
            return View(historyitems);
        }
        //Action method to display profile details of a customer
        [HttpGet]
        public ActionResult Index() {
            return View();
        }
    }
}
