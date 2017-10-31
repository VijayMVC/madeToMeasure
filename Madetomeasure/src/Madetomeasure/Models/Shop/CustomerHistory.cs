using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Madetomeasure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Madetomeasure.Models.Shop
{
    public class CustomerHistory
    {
        private MadeToMeasureContext _context;

        public CustomerHistory(MadeToMeasureContext context)
        {
            _context = context;
        }

        public List<HistoryItem> getHistory(int customerid)
        {
            var jobs = from s in _context.StitchingJob
                       where s.CustomerId == customerid
                       select s;
            List < HistoryItem > historyitems = new List<HistoryItem>();
            foreach (StitchingJob item in jobs)
            {
                var bms = from bm in _context.BlazerMeasurements
                          where item.JobId == bm.StitchingJobId
                          select bm;
                if (bms.Any())
                {
                    foreach (BlazerMeasurements bm in bms)
                    {
                        item.BlazerMeasurements = bm;
                    }
                }

                var ks = from k in _context.KurtaMeasurements
                          where item.JobId == k.StitchingJobId
                          select k;
                if (ks.Any())
                {
                    foreach (KurtaMeasurements k in ks)
                    {
                        item.KurtaMeasurements = k;
                    }
                }

                var ps = from p in _context.PantMeasurements
                          where item.JobId == p.StitchingJobId
                          select p;
                if (ps.Any())
                {
                    foreach (PantMeasurements p in ps)
                    {
                        item.PantMeasurements = p;
                    }
                }

                var ss = from s in _context.ShalwarMeasurements
                          where item.JobId == s.StitchingJobId
                          select s;
                if (ss.Any())
                {
                    foreach (ShalwarMeasurements s in ss)
                    {
                        item.ShalwarMeasurements = s;
                    }
                }

                var shs = from sh in _context.ShirtMeasurements
                          where item.JobId == sh.StitchingJobId
                          select sh;
                if (shs.Any())
                {
                    foreach (ShirtMeasurements sh in shs)
                    {
                        item.ShirtMeasurements = sh;
                    }
                }

                var sus = from su in _context.SuitMeasurements
                          where item.JobId == su.StitchingJobId
                          select su;
                if (sus.Any())
                {
                    foreach (SuitMeasurements su in sus)
                    {
                        item.SuitMeasurements = su;
                    }
                }

                var pas = from pa in _context.ProductionActivity
                          where item.JobId == pa.StitchingJobId
                          select pa;
                List<ProductionActivity> pass = new List<ProductionActivity>();
                if (pas.Any())
                {
                    foreach (ProductionActivity pa in pas)
                    {
                        pass.Add(pa);
                    }
                }
                item.ProductionActivity = pass;

                var jts = from jt in _context.JobType
                          where item.JobTypeId == jt.Id
                          select jt;
                if (jts.Any())
                {
                    foreach (JobType jt in jts)
                    {
                        item.JobType = jt;
                    }
                }

                var shops = from shop in _context.BusinessEntity
                          where item.ShopCode == shop.EntityCode
                          select shop;
                if (shops.Any())
                {
                    foreach (BusinessEntity shop in shops)
                    {
                        item.ShopCodeNavigation = shop;
                    }
                }
                
                var Invoices = from inv in _context.InvoiceDetails
                               where inv.StitchingJobId == item.JobId
                               select inv;
                Invoice invoice = null;
                foreach (InvoiceDetails i in Invoices)
                {
                    var inv1 = from v in _context.Invoice
                               where i.InvoiceId == v.InvoiceId
                               select v;
                    foreach (Invoice inv2 in inv1)
                    {
                        invoice = inv2;
                    }
                }
                if (invoice != null)
                {

                    HistoryItem historyitem = new HistoryItem(item, invoice);
                    historyitems.Add(historyitem);
                }
            }
            return historyitems;
        }

        public HistoryItem getOrderDetails(int stitchingjobid)
        {

            var jobs = from s in _context.StitchingJob
                       where s.JobId == stitchingjobid
                       select s;






            List<HistoryItem> historyitems = new List<HistoryItem>();
            foreach (StitchingJob item in jobs)
            {

                var bms = from bm in _context.BlazerMeasurements
                          where item.JobId == bm.StitchingJobId
                          select bm;
                if (bms.Any())
                {
                    foreach (BlazerMeasurements bm in bms)
                    {
                        item.BlazerMeasurements = bm;
                    }
                }

                var ks = from k in _context.KurtaMeasurements
                         where item.JobId == k.StitchingJobId
                         select k;
                if (ks.Any())
                {
                    foreach (KurtaMeasurements k in ks)
                    {
                        item.KurtaMeasurements = k;
                    }
                }

                var ps = from p in _context.PantMeasurements
                         where item.JobId == p.StitchingJobId
                         select p;
                if (ps.Any())
                {
                    foreach (PantMeasurements p in ps)
                    {
                        item.PantMeasurements = p;
                    }
                }

                var ss = from s in _context.ShalwarMeasurements
                         where item.JobId == s.StitchingJobId
                         select s;
                if (ss.Any())
                {
                    foreach (ShalwarMeasurements s in ss)
                    {
                        item.ShalwarMeasurements = s;
                    }
                }

                var shs = from sh in _context.ShirtMeasurements
                          where item.JobId == sh.StitchingJobId
                          select sh;
                if (shs.Any())
                {
                    foreach (ShirtMeasurements sh in shs)
                    {
                        item.ShirtMeasurements = sh;
                    }
                }

                var sus = from su in _context.SuitMeasurements
                          where item.JobId == su.StitchingJobId
                          select su;
                if (sus.Any())
                {
                    foreach (SuitMeasurements su in sus)
                    {
                        item.SuitMeasurements = su;
                    }
                }

                var pas = from pa in _context.ProductionActivity
                          where item.JobId == pa.StitchingJobId
                          select pa;
                List<ProductionActivity> pass = new List<ProductionActivity>();
                if (pas.Any())
                {
                    foreach (ProductionActivity pa in pas)
                    {
                        pass.Add(pa);
                    }
                }
                item.ProductionActivity = pass;

                var jts = from jt in _context.JobType
                          where item.JobTypeId == jt.Id
                          select jt;
                if (jts.Any())
                {
                    foreach (JobType jt in jts)
                    {
                        item.JobType = jt;
                    }
                }

                var shops = from shop in _context.BusinessEntity
                            where item.ShopCode == shop.EntityCode
                            select shop;
                if (shops.Any())
                {
                    foreach (BusinessEntity shop in shops)
                    {
                        item.ShopCodeNavigation = shop;
                    }
                }

                var Invoices = from inv in _context.InvoiceDetails
                               where inv.StitchingJobId == item.JobId
                               select inv;
                Invoice invoice = null;
                foreach (InvoiceDetails i in Invoices)
                {
                    var inv1 = from v in _context.Invoice
                               where i.InvoiceId == v.InvoiceId
                               select v;
                    foreach (Invoice inv2 in inv1)
                    {
                        invoice = inv2;
                    }
                }
                HistoryItem historyitem = new HistoryItem(item, invoice);
                historyitems.Add(historyitem);
            }
            return historyitems[0];

        }


    }
}
