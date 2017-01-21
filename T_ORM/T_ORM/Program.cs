using System;
using System.Collections.Generic;
using System.ComponentModel;
using Utils;
using Models;
using System.Linq;
using System.Data;
using System.Text.RegularExpressions;

namespace T_ORM
{
    class Program
    {
        public static SynchronisedList<Employee> UserCache1;
        public static SynchronisedDictionary<int, Employee> UserCache2;
        static void Main(string[] args)
        {
            DataTable dt = ExcelHelper.GetExcelToDataTableByAspose(@"C:\en.xlsx");
            List<Class1> modelList = new List<Class1>();
            int index = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (index == 0)
                {
                    index++;
                    continue;
                }
                if (index < 140)
                {
                    Class1 item = new Class1();
                    item.TaskId = index;
                    item.TaskName = splitstr(row[5].ToString());
                    item.TaskTip = splitstr(row[6].ToString());
                    item.Remark = splitstr(row[7].ToString());
                    item.ProblemDescrible = splitstr(row[8].ToString());
                    item.AContent = splitstr(row[12].ToString());
                    item.BContent = splitstr(row[13].ToString());
                    item.CContent = splitstr(row[14].ToString());
                    item.DContent = splitstr(row[15].ToString());
                    item.RelativeName = splitstr(row[22].ToString());

                    modelList.Add(item);
                    index++;
                }
                else
                {
                    break;
                }
            }
            if (modelList.Count == 139)
            {
                new Dapper_MSSQL.Dapper_MSSQL().MyUpdateBulk(modelList);
            }
            

            //LogHelper.Log.WriteInfo("123");
            //string str = "123木头人";
            Console.WriteLine();
            //Console.WriteLine(str.GetStartStr(4,"..."));
            //Console.WriteLine(str.GetEndStr(4, "..."));
            //Console.WriteLine(str.ReplaceStartStr(4, "..."));
            //Console.WriteLine(str.ReplaceEndStr(4, "..."));
            //Console.WriteLine(str.ReplaceMidStr(2,4, "..."));
            //Console.WriteLine(str.ReplaceMidStrByLength(2, 2, "..."));
            //Console.WriteLine(ObjectHelper.GetGuid());
            //Console.WriteLine(ObjectHelper.GuidTo16String());
            //Console.WriteLine(ObjectHelper.GuidToLongID());
            //Console.WriteLine(ObjectHelper.GetUniqueString());
            //CookieHelper.Add("aaa", "haha");
            //Console.WriteLine(CookieHelper.GetValue("aaa"));
            //UserCache.GetList().where()
            //UserCache1 = new SynchronisedList<Employee>(new Dapper_MSSQL.Dapper_MSSQL().GetList());
            //UserCache2 = new SynchronisedDictionary<int, Employee>(x => x.EmployeeId, new Dapper_MSSQL.Dapper_MSSQL().GetList());
            //UserCache2.SyncCache(1, new Employee() { FirstName = "aFN", LastName = "aLN", Salary = 220 });
            //UserCache2.SyncCache(2, new Employee() { FirstName = "bFN", LastName = "bLN", Salary = 220 });

            //foreach (var i in UserCache1)
            //{
            //    Console.WriteLine(i.FirstName);
            //}
            //CodeHelper.CreateBarCode("123", @"D:\12334.jpg");
            //CodeHelper.CreateQRCode("你好", @"D:\1234.png");
            //CodeHelper.CreateQRCodeHasLog("你好", @"D:\12345.png", @"D:\favicon.ico", 12);
            //List<Employee> aa = UserCache2.Values.ToList();
            //IEnumerator<Employee> aat = UserCache2.Values.GetEnumerator();
            //Dictionary<int, Employee> aad = UserCache2.Values.ToDictionary(x => x.EmployeeId);
            //Employee[] aaa = UserCache2.Values.ToArray();
            //IEnumerable<Employee> aai = UserCache2.Values.Where(x => x.EmployeeId > 5);
            
            //List<Employee> bb = UserCache1.GetEnumerator();
            
            Console.ReadLine();

            // ICollection<T> -> IEnumerator<T> : ICollection<T>.GetEnumerator()
            // ICollection<T> -> List<T> : ICollection<T>.ToList()
            // ICollection<T> -> Dictionary<K,V> : ICollection<T>.ToDictionary(x=>x.EmployeeId)
            // ICollection<T> -> T[] : ICollection<T>.ToArray()
            // ICollection<T> -> IEnumerable<T> : ICollection<T>.Where(x=>x.EmployeeId>5)

            // List<T> -> IEnumerator<T> : List<T>.GetEnumerator()
            // List<T> -> Dictionary<K,V> : List<T>.ToDictionary(x=>x.EmployeeId)
            // List<T> -> T[] : List<T>.ToArray()
            // List<T> -> IEnumerable<T> : List<T>.Where(x=>x.EmployeeId>5)
        }

        public static string splitstr(string oldstr)
        {
            return Regex.Split(oldstr, @"\n|\r|\n\r").LastOrDefault();
        }
    }

    enum MemberLevel
    {
        [Description("金牌会员")]
        Gold = 1,

        [Description("银牌会员")]
        Silver = 2,

        [Description("铜牌会员")]
        Copper = 3
    }


}
