using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetaPoco_MSSQL
{
    /// <summary>
    /// http://www.cnblogs.com/youring2/archive/2012/06/04/2532130.html
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var db = new PetaPoco.Database("PetaPocoSqlConn");

            // Show all articles    
            foreach (var a in db.Query<PetaPoco_MSSQL.Models.Poco.Product>("SELECT * FROM T_Product"))
            {
                Console.WriteLine("{0} - {1}", a.ID, a.Name);
            }
        }
    }
}
