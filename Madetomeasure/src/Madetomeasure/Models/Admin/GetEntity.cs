using Madetomeasure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.Admin
{
    public class GetEntity
    {
        public MadeToMeasureContext _context;
        public GetEntity(MadeToMeasureContext context)
        {
            _context = context;
        }
        
        public List<BusinessEntity> getBussinessEntities(int type)
        {
            var bussinessEntities = from be in _context.BusinessEntity
                                    where be.EntityType == type
                                    select be;

            List<BusinessEntity> bussinessEntitiesList = new List<BusinessEntity>();
            foreach(BusinessEntity b in bussinessEntities)
            {
                bussinessEntitiesList.Add(b);
            }
            return bussinessEntitiesList;
                }
        public List<BusinessEntity> getnotassociatedshops()
        {
            //var bussinessEntities = from be in _context.BusinessEntity

            //                        where !_context.Shop.Any(es => (es.ShopCode == be.EntityCode) && (be.EntityType == 1))
            //select be;

            var bussinessEntities= from c in _context.BusinessEntity
                                   
                                   where !(from o in _context.Shop
                                           select o.ShopCode)
                                          .Contains(c.EntityCode)
                                          
                                    where c.EntityType==1
                                   select c;

            List<BusinessEntity> bussinessEntitiesList = new List<BusinessEntity>();
            foreach (BusinessEntity b in bussinessEntities)
            {
                bussinessEntitiesList.Add(b);
            }
            return bussinessEntitiesList;
        }
        public List<BusinessEntity> getnotassociatedwarehouse()
        {
            //var bussinessEntities = from be in _context.BusinessEntity

            //                        where !_context.Shop.Any(es => (es.ShopCode == be.EntityCode) && (be.EntityType == 1))
            //select be;

            var bussinessEntities = from c in _context.BusinessEntity

                                    where !(from o in _context.Warehouse
                                            select o.WarehouseCode)
                                           .Contains(c.EntityCode)

                                    where c.EntityType == 2
                                    select c;

            List<BusinessEntity> bussinessEntitiesList = new List<BusinessEntity>();
            foreach (BusinessEntity b in bussinessEntities)
            {
                bussinessEntitiesList.Add(b);
            }
            return bussinessEntitiesList;
        }
        public List<BusinessEntity> getnotassociatedstitchingunit()
        {
            //var bussinessEntities = from be in _context.BusinessEntity

            //                        where !_context.Shop.Any(es => (es.ShopCode == be.EntityCode) && (be.EntityType == 1))
            //select be;

            var bussinessEntities = from c in _context.BusinessEntity

                                    where !(from o in _context.Warehouse
                                            select o.AssociatedStitchingUnitCode)
                                           .Contains(c.EntityCode)

                                    where c.EntityType == 3
                                    select c;

            List<BusinessEntity> bussinessEntitiesList = new List<BusinessEntity>();
            foreach (BusinessEntity b in bussinessEntities)
            {
                bussinessEntitiesList.Add(b);
            }
            return bussinessEntitiesList;
        }

    }
}
