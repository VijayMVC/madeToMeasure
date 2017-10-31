using Madetomeasure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.Admin
{
    public class GetUserId
    {
        public MadeToMeasureContext _context;
        public GetUserId(MadeToMeasureContext context)
        {
            _context = context;
        }

        public int getuserid(string type)
        {



            int a = 0;
            var bussinessEntities = from be in _context.Users
                                    where be.UserId == type
                                    select be;

            if (bussinessEntities != null)
            {
                foreach (Users b in bussinessEntities)
                {

                    a = a + 1;
                }


            }
            return a;
        }
    }
}
