using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF_MSSQL_CodeFirst.Models;
using System.Diagnostics;

namespace EF_MSSQL_CodeFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            List<UserInfo> users = new List<UserInfo>();

            //for (int i = 1; i < 2000; i++)
            //{
            //    users.Add(new UserInfo() { Name = "zhang", Age = i });
            //}

            using (var ctx = new People_CodeFirst_Context())
            {
                Stopwatch watch = Stopwatch.StartNew();
                //foreach (var u in users)
                //{
                //    ctx.UserInfo.Add(u); // 20348毫秒
                //}
                //ctx.UserInfo.AddRange(users); // 15314毫秒
                //ctx.SaveChanges();

                //ctx.BulkInsert(users);
                //ctx.BulkSaveChanges(); // 3198毫秒
                watch.Stop();
                Console.WriteLine(string.Format("{0} customers are created, cost {1} milliseconds.", 2000, watch.ElapsedMilliseconds));
                Console.ReadLine();
            }
        }
    }
}
