using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_MSSQL_ModelFirst
{
    class Program
    {
        // 注意：如果我们的模型发生改变，只需要在模型设计视图修改模型，让后保存此时实体类就会相应改变，然后选择“从模型生成到数据库”重新执行生成的脚本即可。
        static void Main(string[] args)
        {
            // 测试
            using (var dbContext = new People_MF_DataModelContainer())
            {
                var a = new TableA();
                a.A_Name = "张三";
                dbContext.TableA.Add(a);
                dbContext.SaveChanges();

                var re = from od in dbContext.TableA
                             select od;

                foreach (TableA order2 in re)
                {
                    Console.WriteLine("OrderID:{0},OrderDate:{1}", order2.A_Id, order2.A_Name);
                }

                Console.Read();
            }
        }
    }
}
