using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class UserType
    {
        public UserType()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
