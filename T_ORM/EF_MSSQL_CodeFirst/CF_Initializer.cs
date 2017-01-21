using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF_MSSQL_CodeFirst.Models;

namespace EF_MSSQL_CodeFirst
{
    public class CF_Initializer : DropCreateDatabaseIfModelChanges<People_CodeFirst_Context>
    {
        protected override void Seed(People_CodeFirst_Context context)
        {
            context.UserInfo.Add(new UserInfo() { Name = "admin", Age = 22 });
            context.SaveChanges();


            var roles = new List<Role>();
            roles.Add(new Role() { UId = 1, RoleName = "超管" });
            roles.Add(new Role() { UId = 1, RoleName = "用户" });
            roles.ForEach(s => context.Role.Add(s));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
