using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class BusinessEntityType
    {
        public BusinessEntityType()
        {
            BusinessEntity = new HashSet<BusinessEntity>();
        }

        public int Id { get; set; }
        public string EntityName { get; set; }

        public virtual ICollection<BusinessEntity> BusinessEntity { get; set; }
    }
}
