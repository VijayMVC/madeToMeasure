using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class Users
    {
        public Users()
        {
            Invoice = new HashSet<Invoice>();
            OrderfromVendor = new HashSet<OrderfromVendor>();
            StitchingJob = new HashSet<StitchingJob>();
            StitchingUnitDepartmentHead = new HashSet<StitchingUnitDepartmentHead>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public int UserType { get; set; }
        public string Name { get; set; }
        public DateTime JoiningDate { get; set; }
        public int? WorksAt { get; set; }

        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<OrderfromVendor> OrderfromVendor { get; set; }
        public virtual ICollection<StitchingJob> StitchingJob { get; set; }
        public virtual ICollection<StitchingUnitDepartmentHead> StitchingUnitDepartmentHead { get; set; }
        public virtual UserType UserTypeNavigation { get; set; }
        public virtual BusinessEntity WorksAtNavigation { get; set; }
    }
}
