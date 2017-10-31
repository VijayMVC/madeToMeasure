using Madetomeasure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.Admin
{
    public class Gettablestitchingunitwarehouse
    {
        public MadeToMeasureContext _context;
        public Gettablestitchingunitwarehouse(MadeToMeasureContext context)
        {
            _context = context;
        }
        public List<Tablestitchingunitwarehouse> gettable()
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
            var bussinessEntities = from a in _context.Warehouse
                                    join h in _context.BusinessEntity on a.AssociatedStitchingUnitCode equals h.EntityCode


                                    select new
                                    {
                                        a.AssociatedStitchingUnitCode,
                                        h.EntityAddress,
                                        a.WarehouseCode



                                    };
            var address = from a in _context.Warehouse
                                    join h in _context.BusinessEntity on a.WarehouseCode equals h.EntityCode


                                    select new
                                    {
                                       h.EntityAddress



                                    };


            List<Tablestitchingunitwarehouse> bussinessEntitiesList = new List<Tablestitchingunitwarehouse>();
            foreach (var b in bussinessEntities)
            {
                Tablestitchingunitwarehouse tab = new Tablestitchingunitwarehouse();
                tab.stitchingunitid = b.AssociatedStitchingUnitCode;
                tab.stitchingunitaddress = b.EntityAddress;
                tab.warehouseid = b.WarehouseCode;
              

                bussinessEntitiesList.Add(tab);


            }
            int i = 0;
            foreach (var add in address)
            {

              bussinessEntitiesList.ElementAt<Tablestitchingunitwarehouse>(i).warehousead= add.EntityAddress;

                i++;





            }
            return bussinessEntitiesList;
        }
    }
}
