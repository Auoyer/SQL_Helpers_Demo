using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace Dapper_MSSQL
{
    public class Dapper_MSSQL
    {
        static string _strConn = "Data Source=10.10.21.74;Initial Catalog=People;uid=sa;password=@hzq151510;";

        #region 增
        /// <summary>
        /// 单一添加
        /// </summary>
        /// <returns></returns>
        public int Add(Employee model)
        {
            int result = 0;
            string sqlCommandText = @"insert into TB_Employee (FirstName,LastName,Salary) values (@FirstName,@LastName,@Salary)";

            using (SqlConnection conn = new SqlConnection(_strConn))
            {
                conn.Open();
                // 开启事物
                var tran = conn.BeginTransaction();
                try
                {
                    // 添加用户
                    result = conn.Execute(sqlCommandText, model, tran);

                    // 批量添加权限
                    if (model.RoleList != null && model.RoleList.Count > 0)
                    {
                        sqlCommandText = @"insert into TB_Role (RoleName) values (@RoleName)";
                        result = conn.Execute(sqlCommandText, model.RoleList, tran);
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }

            }
            return result;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <returns></returns>
        public int AddBulk(List<Employee> modelList)
        {
            int result = 0;
            string sqlCommandText = @"insert into TB_Employee (FirstName,LastName,Salary) values (@FirstName,@LastName,@Salary)";
            using (SqlConnection conn = new SqlConnection(_strConn))
            {
                conn.Open();
                // 开启事物
                var tran = conn.BeginTransaction();
                try
                {
                    // 批量添加用户
                    result = conn.Execute(sqlCommandText, modelList);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }
            }
            return result;
        }
        #endregion

        #region 删
        /// <summary>
        /// 单一删除
        /// </summary>
        /// <returns></returns>
        public int Delete(int employeeId)
        {
            int result = 0;
            string sqlCommandText = @"delete from TB_Employee where EmployeeId=@EmployeeId";
            using (SqlConnection conn = new SqlConnection(_strConn))
            {
                result = conn.Execute(sqlCommandText, new { EmployeeId = employeeId });
            }
            return result;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        public int Delete(int[] employeeIds)
        {
            int result = 0;
            string sqlCommandText = @"delete from TB_Employee where EmployeeId in @EmployeeIds";
            using (SqlConnection conn = new SqlConnection(_strConn))
            {
                result = conn.Execute(sqlCommandText, new { EmployeeIds = employeeIds });
            }
            return result;
        }
        #endregion

        #region 改
        /// <summary>
        /// 单一修改
        /// </summary>
        /// <returns></returns>
        public int Update(Employee model)
        {
            int result = 0;
            string sqlCommandText = @"update TB_Employee set FirstName=@FirstName,LastName=@LastName,Salary=@Salary where EmployeeId=@EmployeeId ";

            using (SqlConnection conn = new SqlConnection(_strConn))
            {
                result = conn.Execute(sqlCommandText, model);
            }
            return result;
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <returns></returns>
        public int UpdateBulk(List<Employee> modelList)
        {
            int result = 0;
            string sqlCommandText = @"update TB_Employee set FirstName=@FirstName,LastName=@LastName,Salary=@Salary where EmployeeId=@EmployeeId ";

            using (SqlConnection conn = new SqlConnection(_strConn))
            {
                result = conn.Execute(sqlCommandText, modelList);
            }
            return result;
        }
        #endregion

        #region 查
        /// <summary>
        /// 单表查询
        /// </summary>
        /// <returns></returns>
        public List<Employee> Get(int[] employeeIds)
        {
            List<Employee> result = new List<Employee>();
            string sqlCommandText = @"select * from TB_Employee where EmployeeId in @EmployeeIds";
            using (SqlConnection conn = new SqlConnection(_strConn))
            {
                result = conn.Query<Employee>(sqlCommandText, new { EmployeeIds = employeeIds }).ToList();
            }
            return result;
        }

        /// <summary>
        /// 多表查询
        /// </summary>
        /// <returns></returns>
        public Employee Get(int employeeId)
        {
            Employee result = new Employee();
            string sqlCommandText = @"select * from TB_Employee where EmployeeId=@EmployeeId;select * from TB_Role where TB_Role.RoleId in (SELECT DISTINCT TB_ER.RId FROM TB_ER where TB_ER.EId=@EmployeeId );";
            using (SqlConnection conn = new SqlConnection(_strConn))
            {
                var multi = conn.QueryMultiple(sqlCommandText, new { EmployeeId = employeeId });
                result = multi.Read<Employee>().ToList().FirstOrDefault();
                result.RoleList = multi.Read<Role>().ToList();
            }
            return result;
        }

        /// <summary>
        /// 联合查询
        /// </summary>
        /// <returns></returns>
        public List<Employee> Get_Join(int[] employeeIds)
        {
            var resultList = new List<Employee>();

            using (SqlConnection conn = new SqlConnection(_strConn))
            {
                string sqlCommandText = @"SELECT e.EmployeeId,e.FirstName,e.LastName,e.Salary,r.RoleId,r.RoleName 
                                          FROM TB_Employee e WITH(NOLOCK) 
                                          INNER JOIN TB_ER er ON er.EId = e.EmployeeId 
                                          INNER JOIN TB_Role r ON r.RoleId = er.RId
                                          WHERE e.EmployeeId in @EmployeeIds";
                resultList = conn.Query<Employee, Role, Employee>(sqlCommandText, (user, role) => { user.Role = role; return user; }, new { EmployeeIds = employeeIds }, splitOn: "RoleId").ToList();
            }

            return resultList;
        }

        /// <summary>
        /// 单表查询
        /// </summary>
        /// <returns></returns>
        public List<Employee> GetList()
        {
            List<Employee> result = new List<Employee>();
            string sqlCommandText = @"select * from TB_Employee";
            using (SqlConnection conn = new SqlConnection(_strConn))
            {
                result = conn.Query<Employee>(sqlCommandText).ToList();
            }
            return result;
        }

        /// <summary>
        /// 存储过程分页查询
        /// </summary>
        /// <returns></returns>
        public List<Employee> Get_Proc(string tableName)
        {
            var result = new List<Employee>();
            int totalCount = 0;
            using (SqlConnection conn = new SqlConnection(_strConn))
            {
                result = DBHelper.GetPageList<Employee>(new PageModel(2,5) { Tables = tableName, Sort = "EmployeeId", Fields="*" }, out totalCount);
                //result = dbconn.Query<Employee>("Proc_CommonPagingStoredProcedure", new { EmployeeId = employeeId }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }

            return result;
        }
        #endregion

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <returns></returns>
        public int MyUpdateBulk(List<Class1> modelList)
        {
            _strConn = "Data Source=10.10.21.42;Initial Catalog=HOTEL3DDataBase_EN;uid=sa;password=111;";
            int result = 0;
            string sqlCommandText = @"update CM_PostExperieDetail set TaskName=@TaskName,TaskTip=@TaskTip where TaskId=@TaskId ";

            using (SqlConnection conn = new SqlConnection(_strConn))
            {
                result = conn.Execute(sqlCommandText, modelList);
            }
            return result;
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <returns></returns>
        public int MyUpdateBulk2(List<Class1> modelList)
        {
            _strConn = "Data Source=10.10.21.42;Initial Catalog=HOTEL3DDataBase_EN;uid=sa;password=111;";
            int result = 0;
            string sqlCommandText = @"update CM_Topic set TaskName=@TaskName,TaskTip=@TaskTip,Remark=@Remark,ProblemDescrible=@ProblemDescrible,AContent=@AContent,BContent=@BContent,CContent=@CContent,DContent=@DContent,RelativeName=@RelativeName where TaskId=@TaskId ";

            using (SqlConnection conn = new SqlConnection(_strConn))
            {
                result = conn.Execute(sqlCommandText, modelList);
            }
            return result;
        }
    }
}
