using Madetomeasure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.StUnitAndCustomer
{
    public class StitchingUnitEmployees {
        public List<StitchingUnitEmployee> getEmployees(MadeToMeasureContext _context, int stitchingUnitId, int userid) {

            var dept = from d in _context.StitchingUnitDepartmentHead
                       where d.Id == userid
                       select d;

            int departmentType = 0;
            foreach (var x in dept)
            {

                departmentType = x.AssociatedDepartmentId;
            }




            return (from employee in _context.StitchingUnitEmployee
                    where employee.DepartmentId == departmentType && employee.WarehouseId == stitchingUnitId
                    select employee
                    ).ToList();
        }
    }
}
