using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EF_MSSQL_CodeFirst
{
    public class People_CodeFirst_Context:DbContext
    {
        public People_CodeFirst_Context()
            : base("CodeFirstDb")
        {
            // 默认数据库连接名：CodeFirstDb

            Database.SetInitializer<People_CodeFirst_Context>(
                new CF_Initializer()
                //new CreateDatabaseIfNotExists<People_CodeFirst_Context>()
            );
        }

        public People_CodeFirst_Context(string connStr)
            : base(connStr)
        {
            // 可自带数据库连接名参数
        }

        public DbSet<Models.UserInfo> UserInfo { get; set; }

        public DbSet<Models.Role> Role { get; set; }
    }
}
