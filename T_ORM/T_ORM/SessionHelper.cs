using System.Web;
using System.Web.SessionState;

namespace Utils
{
    /// <summary>
    /// Session帮助类
    /// </summary>
    public class SessionHelper : IRequiresSessionState
    {
        // 定义私有静态Session操作对象
        private static HttpSessionState _session = HttpContext.Current.Session;

        /// <summary>
        /// 根据session名获取session对象
        /// </summary>
        /// <param name="key">session名</param>
        /// <returns>session对象</returns>
        public static object GetSession(string key)
        {
            return _session[key];
        }

        /// <summary>
        /// 根据session名获取session数字（默认为0）
        /// </summary>
        /// <param name="key">session名</param>
        /// <returns></returns>
        public static int GetSessionNum(string key)
        {
            int result = 0;
            if (_session[key] != null)
            {
                int.TryParse(_session[key].ToString(), out result);
            }
            return result;
        }

        /// <summary>
        /// 根据session名获取session字符串（默认为""）
        /// </summary>
        /// <param name="key">session名</param>
        /// <returns></returns>
        public static string GetSessionStr(string key)
        {
            string result = "";
            if (_session[key] != null)
            {
                result = _session[key].ToString();
            }
            return result;
        }

        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="key">session名</param>
        /// <param name="val">session值</param>
        public static void SetSession(string key, object val)
        {
            _session.Remove(key);
            _session.Add(key, val);
        }

        /// <summary>
        /// 删除一个指定的session
        /// </summary>
        /// <param name="key">session名</param>
        public static void RemoveSession(string key)
        {
            _session.Remove(key);
        }

        /// <summary>
        /// 清空所有的session
        /// </summary>
        public static void ClearAll()
        {
            //Session.RemoveAll()通用调用Clear()方法
            _session.Clear();
        }

        /// <summary>
        /// (全局)设置session过期时间
        ///  Timeout属性不能设置为超过 525,600 分钟(1年)的值。 默认值为 20 分钟。 
        ///  <param name="iExpires">调动有效期(分钟)</param>
        /// <remarks>同时可以在web.config中system.web节点中使用sessionState配置timeout属性</remarks>
        /// </summary>
        public static void SetTimeOut(int iExpires)
        {
            _session.Timeout = iExpires;
        }
    }
}