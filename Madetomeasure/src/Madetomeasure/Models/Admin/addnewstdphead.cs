using Madetomeasure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.Admin
{
    public class addnewstdphead

    {

    public int StitchingunitId { get; set; }

    public string Department { get; set; }

        public async Task addstdp(MadeToMeasureContext context, string name, string id, DateTime date, string password, int entitycode, int deprtmentcode)
        {

            Users user = new Users();
            user.WorksAt = entitycode;
            user.UserId = id;
            user.UserType = 5;
            user.Password = password;
            user.JoiningDate = date;
            user.Name = name;
            
            context.Users.Add(user);
            await context.SaveChangesAsync();


            var a = from k in context.Users
                    where k.UserId == id
                    select k.Id;
            int idd = 0;


            // b.EntityAddress = address;
            // ;/ b.EntityType = 3;
            foreach (var b in a)
            {
                idd = (int)b;
                StitchingUnitDepartmentHead sthead = new StitchingUnitDepartmentHead();
                sthead.Id = idd;
                sthead.AssociatedDepartmentId = deprtmentcode;
            context.StitchingUnitDepartmentHead.Add(sthead);



            }

           

            await context.SaveChangesAsync();
        }
    }
}
