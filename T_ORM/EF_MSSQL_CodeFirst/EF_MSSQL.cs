using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF_MSSQL_CodeFirst.Models;
using System.Data.Entity;
using System.Data.SqlClient;

namespace EF_MSSQL_CodeFirst
{
    public class EF_MSSQL
    {
        #region 增
        /// <summary>
        /// 单一添加
        /// </summary>
        /// <returns></returns>
        public void Add(UserInfo model)
        {
            int result = 0;

            using (var ctx = new People_CodeFirst_Context())
            {
                try
                {
                    // 添加用户
                    ctx.UserInfo.Add(model);

                    // 批量添加权限
                    if (model.Roles != null && model.Roles.Count > 0)
                    {
                        // 添加用户权限
                        ctx.BulkInsert(model.Roles);
                    }
                    ctx.BulkSaveChanges();
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <returns></returns>
        public void AddBulk(List<UserInfo> modelList)
        {
            using (var ctx = new People_CodeFirst_Context())
            {
                try
                {
                    // 批量添加用户
                    ctx.BulkInsert(modelList);
                    ctx.BulkSaveChanges();
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #region 删
        /// <summary>
        /// 单一删除
        /// </summary>
        /// <returns></returns>
        public void Delete(int userId)
        {
            UserInfo ui = new UserInfo() { Id = userId };
            using (var ctx = new People_CodeFirst_Context())
            {
                ctx.Entry(ui).State = EntityState.Deleted;
                ctx.SaveChanges();

                //ctx.UserInfo.Attach(ui);
                //ctx.UserInfo.Remove(ui);
                //ctx.SaveChanges();
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        public void Delete(int[] userIds)
        {
            using (var ctx = new People_CodeFirst_Context())
            {
                var users = ctx.UserInfo.Where(x => userIds.Contains(x.Id)).ToList();
                ctx.UserInfo.RemoveRange(users);
                ctx.SaveChanges();

                //ctx.BulkDelete(users);
                //ctx.BulkSaveChanges();
                SqlParameter para = new SqlParameter("@Ids", string.Join(",", userIds));
                ctx.Database.ExecuteSqlCommand("delete from UserInfo where Id in (@Ids)", para);
                //var result = db.Database.SqlQuery<UserInfo>("SELECT * FROM test.student WHERE name = '萝莉'");
            }
        }
        #endregion

//        #region 改
//        /// <summary>
//        /// 单一修改
//        /// </summary>
//        /// <returns></returns>
//        public int Update(Employee model)
//        {
//            int result = 0;
//            string sqlCommandText = @"update TB_Employee set FirstName=@FirstName,LastName=@LastName,Salary=@Salary where EmployeeId=@EmployeeId ";

//            using (SqlConnection conn = new SqlConnection(_strConn))
//            {
//                result = conn.Execute(sqlCommandText, model);
//            }
//            return result;
//        }

//        /// <summary>
//        /// 批量修改
//        /// </summary>
//        /// <returns></returns>
//        public int UpdateBulk(List<Employee> modelList)
//        {
//            int result = 0;
//            string sqlCommandText = @"update TB_Employee set FirstName=@FirstName,LastName=@LastName,Salary=@Salary where EmployeeId=@EmployeeId ";

//            using (SqlConnection conn = new SqlConnection(_strConn))
//            {
//                result = conn.Execute(sqlCommandText, modelList);
//            }
//            return result;
//        }
//        #endregion

//        #region 查
//        /// <summary>
//        /// 单表查询
//        /// </summary>
//        /// <returns></returns>
//        public List<Employee> Get(int[] employeeIds)
//        {
//            List<Employee> result = new List<Employee>();
//            string sqlCommandText = @"select * from TB_Employee where EmployeeId in @EmployeeIds";
//            using (SqlConnection conn = new SqlConnection(_strConn))
//            {
//                result = conn.Query<Employee>(sqlCommandText, new { EmployeeIds = employeeIds }).ToList();
//            }
//            return result;
//        }

//        /// <summary>
//        /// 多表查询
//        /// </summary>
//        /// <returns></returns>
//        public Employee Get(int employeeId)
//        {
//            Employee result = new Employee();
//            string sqlCommandText = @"select * from TB_Employee where EmployeeId=@EmployeeId;select * from TB_Role where TB_Role.RoleId in (SELECT DISTINCT TB_ER.RId FROM TB_ER where TB_ER.EId=@EmployeeId );";
//            using (SqlConnection conn = new SqlConnection(_strConn))
//            {
//                var multi = conn.QueryMultiple(sqlCommandText, new { EmployeeId = employeeId });
//                result = multi.Read<Employee>().ToList().FirstOrDefault();
//                result.RoleList = multi.Read<Role>().ToList();
//            }
//            return result;
//        }

//        /// <summary>
//        /// 联合查询
//        /// </summary>
//        /// <returns></returns>
//        public List<Employee> Get_Join(int[] employeeIds)
//        {
//            var resultList = new List<Employee>();

//            using (SqlConnection conn = new SqlConnection(_strConn))
//            {
//                string sqlCommandText = @"SELECT e.EmployeeId,e.FirstName,e.LastName,e.Salary,r.RoleId,r.RoleName 
//                                          FROM TB_Employee e WITH(NOLOCK) 
//                                          INNER JOIN TB_ER er ON er.EId = e.EmployeeId 
//                                          INNER JOIN TB_Role r ON r.RoleId = er.RId
//                                          WHERE e.EmployeeId in @EmployeeIds";
//                resultList = conn.Query<Employee, Role, Employee>(sqlCommandText, (user, role) => { user.Role = role; return user; }, new { EmployeeIds = employeeIds }, splitOn: "RoleId").ToList();
//            }

//            return resultList;
//        }

//        /// <summary>
//        /// 存储过程
//        /// </summary>
//        /// <returns></returns>
//        public List<UserInfo> Get_Proc(int userId)
//        {
//            var result = new UserInfo();
//            using (var ctx = new People_CodeFirst_Context())
//            {
//                SqlParameter para = new SqlParameter("@Id", userId);
//                result = ctx.Database.SqlQuery<UserInfo>("Proc_GetEmployee @Id", para);
//            }

//            return result;
//        }
//        #endregion
    }
}
