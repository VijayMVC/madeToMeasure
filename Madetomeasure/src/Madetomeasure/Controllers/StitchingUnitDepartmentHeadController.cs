using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Madetomeasure.Models.StUnitAndCustomer;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Madetomeasure.Data;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

//Author: Ryuzaki Sultan
//Last Edited: 2/12/2016
//This Controller is used to manage the Stitching UnitDepartment Head account and interation with system
//the Stitching UnitDepartment Head is redirected here after login

namespace Madetomeasure.Controllers
{
    
    public class StitchingUnitDepartmentHeadController : Controller
    {
        
        private MadeToMeasureContext _context;

        public StitchingUnitDepartmentHeadController(MadeToMeasureContext context) {
            _context = context;
        }
        public async void SendSms(int stitchingjobid) {
            CustomerHistory customerhistory = new CustomerHistory(_context);
            HistoryItem item = customerhistory.getOrderDetails(stitchingjobid);

            StitchingJob stitchingjob = _context.StitchingJob.Find(stitchingjobid);

            Users customer = _context.Users.Find(stitchingjob.CustomerId);
            string phonenumber = customer.UserId;
            
            if (phonenumber[0] == '0')
            {
                phonenumber = "+92" + phonenumber.TrimStart(new char[] { '0' });

            }
            




            string statusname = null;
            if (item.sj.WarehouseStatus == 1) {
                int status = -1;
                foreach (ProductionActivity pa in item.sj.ProductionActivity) {
                    if (status < pa.ProgressStatus) {
                        status = pa.ProgressStatus;
                    }
                }

                switch (status) {
                    case 1: statusname = "Cutting کٹنگ"; break;
                    case 2: statusname = "Embroidary کڑھائی"; break;
                    case 3: statusname = "Stitching سلائی"; break;
                    case 4: statusname = "Quality Assurance کوالٹی اشورینس"; break;
                    case 5: statusname = "Packing پیکنگ"; break;
                    case 6: statusname = "Finished تیار"; break;
                    default:
                        break;
                }
            } else {
                statusname = "Warehouse Processing ویئرہاؤس پراسیسنگ";

            }
            string jobname = null;
            if (item.sj.JobType.JobName == "Kurta") {
                jobname = "Kurta کرتا";
            } else if (item.sj.JobType.JobName == "Shalwar") {
                jobname = "Shalwar شلوار ";
            } else if (item.sj.JobType.JobName == "Blazer") {
                jobname = "Blazer بلیزر";
            } else if (item.sj.JobType.JobName == "Suit") {
                jobname = "Suit سوٹ";
            } else if (item.sj.JobType.JobName == "Shirt") {
                jobname = "Shirt شرٹ";
            } else if (item.sj.JobType.JobName == "KameezShalwar") {
                jobname = "KameezShalwar قمیض  شلوار";
            } else if (item.sj.JobType.JobName == "Pant") {
                jobname = "Pant پینٹ";
            }

            string message = "Your order has passed " + statusname + ", " + "Invoice Number انوائس نمبر:" + item.invoice.InvoiceId;
            /*string ordername = "Order Name آرڈر کا نام:" + jobname + ",";
            string invoicenumber = "Invoice Number انوائس نمبر:" + item.invoice.InvoiceId + ",";
            string orderdate = "Order Date آرڈر کی تاریخ:" + item.invoice.Date.ToString("dd/MM/yyyy") + ",";
            string expecteddate = "Expected Date متوقع تاریخ:" + item.sj.ExpectedDate.Value.ToString("dd/MM/yyyy");*/
            using (var client = new HttpClient { BaseAddress = new Uri("https://api.twilio.com") }) {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes($"{"AC2371684484ff1369244789f5b99d84cd"}:{"bcb8ebb17ed4d204dcb1c4f9630f030e"}")));

                var content = new FormUrlEncodedContent(new[]
                {
            new KeyValuePair<string, string>("To",phonenumber),
            new KeyValuePair<string, string>("From", "+12013081067"),
            new KeyValuePair<string, string>("Body", message)
        });
                try {
                    await client.PostAsync("/2010-04-01/Accounts/AC2371684484ff1369244789f5b99d84cd/Messages.json", content).ConfigureAwait(false);
                } catch (Exception ex) {
                    
                }
                
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }

            return View();
        }

        [HttpGet]
        public ActionResult ProcessJobRole()
        {

            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }

            //Use session to pass getEmployee function departmenttype and stitching unit, can be get from current stitching unit department head

            List<StitchingUnitEmployee> stitchingUnitEmployees = (new StitchingUnitEmployees()).getEmployees(_context, Convert.ToInt32(HttpContext.Session.GetString("WorksAt")), Convert.ToInt32(HttpContext.Session.GetString("Id")));
            return View(stitchingUnitEmployees);
        }

        [HttpPost]
        public async Task<ActionResult> saveJob(int EmployeeId, int stitchingjobid) {



            if (HttpContext.Session.GetString("Id") == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Account");
            }


            //get from barcode or from input field

            Users user = _context.Users.Find(Convert.ToInt32(HttpContext.Session.GetString("Id")));

            StitchingUnitDepartmentHead sudh = (from s in _context.StitchingUnitDepartmentHead
                                                where s.Id == user.Id
                                                select s).ToList()[0];

            var pa = (from p in _context.ProductionActivity
                                 where p.StitchingJobId == stitchingjobid && p.ProgressStatus == sudh.AssociatedDepartmentId
                                 select p);
            if (pa.Any())
            {
                TempData["alreadyadded"] = true;
                return RedirectToAction("ProcessJobRole");

            }
                                 

            int stitchingstatus = sudh.AssociatedDepartmentId; //get from session based on which deparment head is stitching unit deparment head
            await new SaveProductionActivity().logProductionActivity(_context, EmployeeId, stitchingjobid, stitchingstatus);

            SendSms(stitchingjobid);
            TempData["stitchingjobsave"] = true;


            //get Current User from Session
            return RedirectToAction("ProcessJobRole");
        }
    }
}
