using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper_MSSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            //#region 单一添加
            //Employee model = new Employee() { FirstName = "ariter9", LastName = "ariter9", Salary = 400 };
            //model.RoleList.Add(new Role() { RoleName = "游客" });
            //model.RoleList.Add(new Role() { RoleName = "其他" });

            //new Dapper_MSSQL().Add(model);
            //#endregion

            //#region 批量添加
            List<Employee> modelList = new List<Employee>();
            modelList.Add(new Employee() { FirstName = "ariter5", LastName = "ariter5", Salary = 200 });
            modelList.Add(new Employee() { FirstName = "ariter6", LastName = "ariter6", Salary = 200 });
            modelList.FirstOrDefault();
            //new Dapper_MSSQL().AddBulk(modelList);
            //#endregion

            //#region 单一删除
            //int employeeId = 1;

            //new Dapper_MSSQL().Delete(employeeId);
            //#endregion

            //#region 批量删除
            //int[] employeeIds = new int[] { 1, 2, 3, 4 };

            //new Dapper_MSSQL().Delete(employeeIds);
            //#endregion

            //#region 单一更新
            //model = new Employee() { FirstName = "ariter5", LastName = "ariter5", Salary = 200, EmployeeId = 1 };

            //new Dapper_MSSQL().Update(model);
            //#endregion

            //#region 批量更新
            //modelList = new List<Employee>();
            //modelList.Add(new Employee() { FirstName = "ariter52", LastName = "ariter52", Salary = 2001 });
            //modelList.Add(new Employee() { FirstName = "ariter62", LastName = "ariter62", Salary = 2002 });

            //new Dapper_MSSQL().UpdateBulk(modelList);
            //#endregion

            //#region 单表查询
            //employeeIds = new int[] { 1, 2, 3, 4 };

            //new Dapper_MSSQL().Get(employeeIds);
            //#endregion

            //#region 多表查询
            //employeeId = 1;

            //new Dapper_MSSQL().Get(employeeId);
            //#endregion

            //#region 联合查询
            //employeeIds = new int[] { 1, 2, 3, 4 };

            //new Dapper_MSSQL().Get_Join(employeeIds);
            //#endregion

            #region 存储过程 + 分页
            // DECLARE @Total INT
            // exec Proc_CommonPagingStoredProcedure 'TB_Employee','','EmployeeId',1,5,'*','',TRUE,@Total out
            new Dapper_MSSQL().Get_Proc("TB_Employee");
            #endregion 
        }
    }
}
