using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Madetomeasure.ViewModels.Account;
using Madetomeasure.Models.Account;
using Madetomeasure.Models;
using Microsoft.AspNetCore.Http;
using Madetomeasure.Data;

//Controller to manage the accounts of the users
namespace Madetomeasure.Views
{
    public class AccountController :  Controller
    {
        MadeToMeasureContext db;
      

        public AccountController(MadeToMeasureContext dbContext)
        {
            db = dbContext;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();


            return View("Index");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View("Index", model);

            }

            
                LoginVerification loginVerify = new LoginVerification(db, HttpContext.Session);
                String UserType = loginVerify.getUserType(model);

           if (UserType == "Warehouse Manager")
            {
                return RedirectToAction("Index", "Warehouse");

            }

            else if (UserType == "Admin")
            {
                return RedirectToAction("Index", "Admin");

            }

            else if (UserType == "Shopkeeper")
            {
                return RedirectToAction("Index", "Shop");

            }

            else if (UserType == "Stitching Unit Manager")
            {
                return RedirectToAction("Index", "StitchingUnitManager");

            }
            else if (UserType == "Stitching Unit Department Head")
            {
                return RedirectToAction("Index", "StitchingUnitDepartmentHead");

            }
            else if (UserType == "Customer")
            {
                return RedirectToAction("Index", "Customer");

            }


            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View("Index", model);
            }


        }

    }
}
