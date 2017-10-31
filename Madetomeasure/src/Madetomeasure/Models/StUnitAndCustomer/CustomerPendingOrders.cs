using Madetomeasure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.StUnitAndCustomer
{
    public class CustomerPendingOrders
    {
        private MadeToMeasureContext _context;

        public CustomerPendingOrders(MadeToMeasureContext context)
        {
            _context = context;
        }

        public List<HistoryItem> getPendingOrders(string id)
        {
            var customers = from c in _context.Users
                            where c.UserId == id
                            select c;
            int customerid = 0;
            foreach (Users user in customers)
            {
                customerid = user.Id;
            }

            var jobs = from s in _context.StitchingJob
                       where s.CustomerId == customerid
                       select s;
            List<HistoryItem> historyitems = new List<HistoryItem>();
            foreach (StitchingJob item in jobs)
            {
                //check stitching job isn't finished
                if (item.CurrentStatus != 6)
                {
                    var Invoices = from inv in _context.InvoiceDetails
                                   where inv.StitchingJobId == item.JobId
                                   select inv;
                    Invoice invoice = null;
                    foreach (InvoiceDetails i in Invoices)
                    {
                        invoice = i.Invoice;
                    }
                    HistoryItem historyitem = new HistoryItem(item, invoice);
                    historyitems.Add(historyitem);
                }
            }
            return historyitems;
        }
    }
}
