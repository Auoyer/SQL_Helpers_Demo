using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace Dapper_MSSQL
{
    public class DBHelper
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        private static string dbProviderName = string.Empty;

        /// <summary>
        /// 数据库连接字符窜
        /// </summary>
        private static string dbConnectionString = string.Empty;

        /// <summary>
        /// 当前程序集名称
        /// </summary>
        private static string assemblyName = string.Empty;

        private DBHelper()
        {
        }

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static DBHelper()
        {
            //assemblyName = typeof(DBHelper).Assembly.GetName().Name;
            dbProviderName = "System.Data.SqlClient";
            dbConnectionString = ConfigurationManager.AppSettings["DapperSqlConn"];
        }

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public static DbConnection CreateConnection()
        {
            #region 通过配置文件
            ////根据数据库名获取工厂
            //DbProviderFactory dbfactory = DbProviderFactories.GetFactory(dbProviderName);
            ////创建连接
            //DbConnection dbconn = dbfactory.CreateConnection();
            ////设置连接字符窜
            //dbconn.ConnectionString = dbConnectionString;
            ////返回连接
            //return dbconn; 
            #endregion

            return new SqlConnection("Data Source=10.10.21.74;Initial Catalog=People;uid=sa;password=@hzq151510;");
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <typeparam name="T">要获取实体</typeparam>
        /// <param name="pageIndex">要获取的页数</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="model">分页参数</param>
        /// <param name="totalCount">数据总数量</param>
        /// <returns></returns>
        public static List<T> GetPageList<T>(PageModel model, out int totalCount)
        {
            totalCount = 0;
            List<T> result = new List<T>();
            var param = new DynamicParameters();

            using (var conn = CreateConnection())
            {
                conn.Open();
                param.Add("@Tables", model.Tables, dbType: DbType.String);
                param.Add("@PK", model.PKey, dbType: DbType.String);
                param.Add("@Sort", model.Sort, dbType: DbType.String);
                param.Add("@PageNumber", model.PageIndex, dbType: DbType.Int32);
                param.Add("@PageSize", model.PageSize, dbType: DbType.Int32);
                param.Add("@Fields", model.Fields, dbType: DbType.String);
                param.Add("@Filter", model.Filter, dbType: DbType.String);
                param.Add("@isCount", model.IsCount, dbType: DbType.Boolean);
                param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);

                result = conn.Query<T>("Proc_CommonPagingStoredProcedure", param, commandType: CommandType.StoredProcedure).ToList();
                totalCount = param.Get<int>("@Total");
            }

            return result;
        }
    }

    /// <summary>
    /// 分页参数模型
    /// </summary>
    public class PageModel
    {
        public PageModel()
        {
            PageIndex = 1;
            PageSize = 10;
            IsCount = true;
        }

        public PageModel(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            IsCount = true;
        }

        /// <summary>
        /// 表名,多表请使用 tableA a inner join tableB b On a.AID = b.AID
        /// </summary>
        public string Tables { get; set; }

        /// <summary>
        /// 主键，可以带表头 a.AID
        /// </summary>
        public string PKey { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 开始页码即要查询的页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 读取字段
        /// </summary>
        public string Fields { get; set; }

        /// <summary>
        /// Where条件
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// 是否获得总记录数
        /// </summary>
        public bool IsCount { get; set; }
    }
}
