using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernat
{
    class Program
    {
        /// <summary>
        /// http://www.cnblogs.com/GoodHelper/archive/2011/02/14/nhiberante_01.html
        /// http://www.huiyaosoft.com/html/nhibernate.htm
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //new NHibernateInit().Init();

            ProductDao productDao = new ProductDao();
            var product = new Product
            {
                ID = Guid.NewGuid(),
                BuyPrice = 10M,
                Code = "ABC1232",
                Name = "电脑2",
                QuantityPerUnit = "20x12",
                SellPrice = 11M,
                Unit = "2台"
            };

            var obj = productDao.Save(product);

        }
    }
}
