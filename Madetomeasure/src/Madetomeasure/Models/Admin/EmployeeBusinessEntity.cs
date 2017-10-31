using Madetomeasure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.Admin
{
    public class EmployeeBusinessEntity
    {
        [Required(ErrorMessage = "Please enter your UserId")]
        [Display(Name = "work")]
        public int work { get; set; }
        public async Task addemployeecom(MadeToMeasureContext context,string name,string id,DateTime date,string password,int works,int entitycod)
        {

            Users user = new Users();
            user.WorksAt = works;
            user.UserId = id;
            user.UserType = entitycod;
            user.Password = password;
            user.JoiningDate = date;
            user.Name = name;

            // b.EntityAddress = address;
            // ;/ b.EntityType = 3;

            context.Users.Add(user);
            await context.SaveChangesAsync();
        }
    }
}
