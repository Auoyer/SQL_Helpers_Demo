using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tradition_MSSQL.DAL;

namespace Tradition_MSSQL
{
    class Program
    {
        static void Main(string[] args)
        {

            var product = new Product
            {
                ID = Guid.NewGuid(),
                BuyPrice = 10M,
                Code = "ABC1232",
                Name = "电脑2",
                QuantityPerUnit = "20x12",
                SellPrice = 11M,
                Unit = "2台",
                Remark="asa"
            };

          int a = new ProductDal().addProduct(product);

        }
    }
}
