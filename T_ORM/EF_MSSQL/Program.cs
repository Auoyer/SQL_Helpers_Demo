using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_MSSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            // 注意：如果你的数据库表结构发生改变后，只需在模型设计视图空白处右键，选择“从数据库更新模型”接着按照向导操作即可。

            // 测试
            using (var db = new People_EF_DBFirstEntities())
            {
                var persons = from people in db.TB_Employee
                                             where people.LastName == "三"
                                             select people;
               //var persons = db.TB_Employee.Where(x => x.LastName == "三").ToList();
                foreach (TB_Employee p in persons)
                {
                    Console.WriteLine("first name is :{0}", p.FirstName);
                    if (p.FirstName == "张")
                    {
                        p.Salary = 999;
                    }
                }

                var persons2 = from people in db.TB_Employee
                               where people.LastName == "三" && people.FirstName == "张"
                               select people;
                if (persons2.Any())
                {
                    Console.WriteLine("张三的工资 :{0}", persons2.First().Salary);
                }
                //上面虽然可以查出来AddtionalContactInfo，但是实际省并未保存到数据库，具体保存方法在此不再详细描述
            }
        }
    }
}
