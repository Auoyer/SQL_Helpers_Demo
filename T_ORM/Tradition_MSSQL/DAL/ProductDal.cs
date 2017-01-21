using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tradition_MSSQL.DAL
{
    public class ProductDal
    {
        public static string conn = "Data Source=10.10.21.74;Database=nhibernate_Test;UID=sa;PWD=@hzq151510;";
        public int addProduct(Product model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into T_Product(");
            strSql.Append("ID,Code,Name,QuantityPerUnit,Unit,SellPrice,BuyPrice,Remark");
            strSql.Append(") values (");
            strSql.Append("@ID,@Code,@Name,@QuantityPerUnit,@Unit,@SellPrice,@BuyPrice,@Remark");
            strSql.Append(") ");
            SqlParameter[] param = new SqlParameter[]
            { 
                new SqlParameter("@ID",model.ID),
                new SqlParameter("@Code",model.Code),
                new SqlParameter("@Name",model.Name),
                new SqlParameter("@QuantityPerUnit",model.QuantityPerUnit),
                new SqlParameter("@Unit", model.Unit),
                new SqlParameter("@SellPrice",model.SellPrice),
                new SqlParameter("@BuyPrice",model.BuyPrice),
                new SqlParameter("@Remark",model.Remark),
           };

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), param);
        }
    }
}
