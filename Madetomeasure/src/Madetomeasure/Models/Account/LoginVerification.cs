using Madetomeasure.Data;
using Madetomeasure.ViewModels.Account;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.Account
{
    public class LoginVerification
    {
        //private static IHttpContextAccessor _httpContextAccessor;
        private ISession _session; 


        MadeToMeasureContext db;
       
        public LoginVerification(MadeToMeasureContext dbContext, ISession session)
        {

            db = dbContext;
            _session = session;
           

        }

     //   public static void Configure(IHttpContextAccessor httpContextAccessor)
       // {
         //   _httpContextAccessor = httpContextAccessor;
        //}

        public String getUserType(LoginViewModel model)
        {

            String type = "None";
           

             var user = from u in db.Users
                         where u.UserId == model.Email && u.Password == model.Password
                         select u;

            //var user = from u in db.Users
            //           select u;



            foreach (var x in user)
            {
                type = "Smooth";

                var userType = from m in db.UserType
                               where m.Id == x.UserType
                               select m;

                foreach (var y in userType)
                {
                    type = y.Type;

                }

                _session.SetString("Id", x.Id.ToString());
                _session.SetString("UserId", x.UserId);
                _session.SetString("UserType", type);
                _session.SetString("Name", x.Name);
                _session.SetString("JoiningDate", x.JoiningDate.Month.ToString() + "/" + x.JoiningDate.Day.ToString() + "/" + x.JoiningDate.Year.ToString()  );

                if (x.WorksAt == null)
                {
                    _session.SetString("WorksAt", "");
                }

                else
                {
                    _session.SetString("WorksAt", x.WorksAt.ToString());

                }

               
                
              
            }


            
            return type;


        }



    }
}
