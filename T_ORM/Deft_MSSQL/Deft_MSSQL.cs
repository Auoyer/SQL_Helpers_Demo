using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deft;
using Models;
using System.Configuration;

namespace Deft_MSSQL
{
    public class Deft_MSSQL
    {
        static string _strConn = "Data Source=10.10.21.74;Initial Catalog=People2;uid=sa;password=@hzq151510;";

        #region 增
        /// <summary>
        /// 单一添加
        /// </summary>
        /// <returns></returns>
        public int Add(Employee model)
        {
            int result = 0;
            JuggConfig.DefaultDbConCfg = new ConnectionStringSettings { ProviderName = "System.Data.SqlClient", ConnectionString = _strConn };

            using (JuggContext context = new JuggContext())
            {
                try
                {
                    context.BeginTransaction();

                    result = context.Insert<Employee>((x => new { x.FirstName, x.LastName,x.Salary }),model.FirstName,model.LastName,model.Salary);

                    // 批量添加权限
                    if (model.RoleList != null && model.RoleList.Count > 0)
                    {
                        foreach (var item in model.RoleList)
                        {
                            result = context.Insert<Role>((x => new { x.RoleName }), item.RoleName);
                        }
                    }

                    if (result > 0)
                    {
                        context.Commit();
                    }
                    else
                    {
                        context.Rollback();
                    }
                }
                catch
                {
                    context.Rollback();
                }
            }

            return result;
        }
        #endregion
    }
}
