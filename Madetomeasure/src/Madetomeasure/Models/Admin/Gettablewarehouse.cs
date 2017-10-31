using Madetomeasure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.Admin
{
    public class Gettablewarehouse
    {
        public MadeToMeasureContext _context;
        public Gettablewarehouse(MadeToMeasureContext context)
        {
            _context = context;
        }
        public List<tableshopwarehouse> gettable()
        {
            //var bussinessEntities = from be in _context.BusinessEntity

            //                        select be;
            //from a in Context.DGApprovedLink
            //join h in Context.DGHost on a.HostID equals h.ID
            //join c in Context.DGConfig on a.ResponseCode equals c.SubType
            //where c.Type == "HTTP Status"
            //select new
            //{
            //    a.ID,
            //    a.HostID,
            //    h.URL,
            //    a.SourceURL,
            //    a.TargetURL,
            //    c.Value,
            //    a.ExtFlag
            //};
            var address= from a in _context.Shop
                         join h in _context.BusinessEntity on a.AssociatedWarehouseCode equals h.EntityCode


                         select new
                         {
                      
                             h.EntityAddress
                           



                         };
            var bussinessEntities = from a in _context.Shop
                                    join h in _context.BusinessEntity on a.ShopCode equals h.EntityCode
                                    

                                    select new
                                   {
                                      a.ShopCode,
                                  h.EntityAddress,
                                  a.AssociatedWarehouseCode
                                  


                                   };
 

            List < tableshopwarehouse > bussinessEntitiesList = new List<tableshopwarehouse>();
            foreach (var b in bussinessEntities )
            {
                tableshopwarehouse tab = new tableshopwarehouse();
                tab.shopid = b.ShopCode;
                tab.shopaddress = b.EntityAddress;
                tab.warehouseid = b.AssociatedWarehouseCode;

                bussinessEntitiesList.Add(tab);
             
               
            }
            int i = 0;
            foreach (var add in address)
            {

                bussinessEntitiesList.ElementAt<tableshopwarehouse>(i).wareaddress = add.EntityAddress;

                i++;


            }
            return bussinessEntitiesList;
        }

    }
}
