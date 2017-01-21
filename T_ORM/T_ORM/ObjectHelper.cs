using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Security.Cryptography;


namespace Utils
{
    /// <summary>
    /// 字符串<see cref="String"/>类型的扩展辅助操作类
    /// </summary>
    public static class ObjectHelper
    {
        #region 正则

        /// <summary>
        /// 指示所指定的正则表达式在指定的输入字符串中是否找到了匹配项
        /// </summary>
        /// <param name="value">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <returns>如果正则表达式找到匹配项,则为 true；否则,为 false</returns>
        public static bool IsMatch(this string value, string pattern)
        {
            if (value == null)
            {
                return false;
            }
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        /// 在指定的输入字符串中搜索指定的正则表达式的第一个匹配项
        /// </summary>
        /// <param name="value">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <returns>一个对象,包含有关匹配项的信息</returns>
        public static string Match(this string value, string pattern)
        {
            if (value == null)
            {
                return null;
            }
            return Regex.Match(value, pattern).Value;
        }

        /// <summary>
        /// 在指定的输入字符串中搜索指定的正则表达式的所有匹配项的字符串集合
        /// </summary>
        /// <param name="value"> 要搜索匹配项的字符串 </param>
        /// <param name="pattern"> 要匹配的正则表达式模式 </param>
        /// <returns> 一个集合,包含有关匹配项的字符串值 </returns>
        public static IEnumerable<string> Matches(this string value, string pattern)
        {
            if (value == null)
            {
                return new string[] { };
            }
            MatchCollection matches = Regex.Matches(value, pattern);
            return from Match match in matches select match.Value;
        }

        /// <summary>
        /// 是否电子邮件
        /// </summary>
        public static bool IsEmail(this string value)
        {
            const string pattern = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否是IP地址
        /// </summary>
        public static bool IsIpAddress(this string value)
        {
            const string pattern = @"^(\d(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])\d\.){3}\d(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])\d$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否是整数
        /// </summary>
        public static bool IsNumeric(this string value)
        {
            const string pattern = @"^\-?[0-9]+$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否是Unicode字符串
        /// </summary>
        public static bool IsUnicode(this string value)
        {
            const string pattern = @"^[\u4E00-\u9FA5\uE815-\uFA29]+$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否Url字符串
        /// </summary>
        public static bool IsUrl(this string value)
        {
            const string pattern = @"^(http|https|ftp|rtsp|mms):(\/\/|\\\\)[A-Za-z0-9%\-_@]+\.[A-Za-z0-9%\-_@]+[A-Za-z0-9\.\/=\?%\-&_~`@:\+!;]*$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否身份证号,验证如下3种情况：
        /// 1.身份证号码为15位数字；
        /// 2.身份证号码为18位数字；
        /// 3.身份证号码为17位数字+1个字母
        /// </summary>
        public static bool IsIdentityCard(this string value)
        {
            const string pattern = @"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否手机号码
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isRestrict">是否按严格格式验证</param>
        public static bool IsMobileNumber(this string value, bool isRestrict = false)
        {
            string pattern = isRestrict ? @"^[1][3-8]\d{9}$" : @"^[1]\d{10}$";
            return value.IsMatch(pattern);
        }

        #endregion

        #region Json对象转换
        /// <summary>
        /// 把对象序列化成Json字符串格式
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static string FromModelToJson(object @object)
        {
            // DoNet2.0 需要借助于Newtonsoft.Json.dll
            string json = JsonConvert.SerializeObject(@object);

            //// Donet3.5自带了DLL处理json串
            //StringBuilder stringBuilder = new StringBuilder();
            //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            //jsonSerializer.Serialize(@object, stringBuilder);
            //json = stringBuilder.ToString();  

            return JsonDateTimeFormat(json);
        }

        /// <summary>
        /// 将JSON字符串还原为对象
        /// </summary>
        /// <typeparam name="T">要转换的目标类型</typeparam>
        /// <param name="json">JSON字符串 </param>
        /// <returns></returns>
        public static T FromJsonToModel<T>(string json)
        {
            json = JsonDateTimeFormat(json);
            // DoNet2.0 需要借助于Newtonsoft.Json.dll
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            return Serializer.Deserialize<T>(json);

            //// Donet3.5自带了DLL处理json串
            //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            //return jsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// 将JSON字符串还原为对象List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="JsonStr"></param>
        /// <returns></returns>
        public static List<T> JSONStringToList<T>(string JsonStr)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            //设置转化JSON格式时字段长度
            List<T> objs = Serializer.Deserialize<List<T>>(JsonStr);
            return objs;
        }

        /// <summary>
        /// 处理Json的时间格式为正常格式
        /// </summary>
        public static string JsonDateTimeFormat(string json)
        {
            json = Regex.Replace(
                json,
                @"\\/Date\((\d+)\)\\/",
                match =>
                {
                    DateTime dt = new DateTime(1970, 1, 1);
                    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    dt = dt.ToLocalTime();
                    return dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                });
            return json;
        } 
        #endregion

        #region Byte字符串转换

        /// <summary>
        /// 将字符串转换为<see cref="byte"/>[]数组,默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        public static byte[] ToBytes(this string value, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding.GetBytes(value);
        }

        /// <summary>
        /// 将<see cref="byte"/>[]数组转换为字符串,默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        public static string ToString(this byte[] bytes, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding.GetString(bytes);
        }

        #endregion

        #region 合并分割截取
        /// <summary>
        /// 数组转字符串
        /// </summary>
        /// <param name="values"></param>
        /// <param name="temp"></param>
        /// <returns></returns>
        public static string ArrayJoinToStr<T>(T[] values, string temp = ",")
        {
            return string.Join(temp, values);
        }

        /// <summary>
        /// 列表转字符串
        /// </summary>
        /// <param name="values"></param>
        /// <param name="temp"></param>
        /// <returns></returns>
        public static string ListJoinToStr<T>(List<T> values, string temp = ",")
        {
            return string.Join(temp, values);
        }

        /// <summary>
        /// 以指定字符串作为分隔符将指定字符串分隔成数组
        /// </summary>
        /// <param name="value">要分割的字符串</param>
        /// <param name="strSplit">字符串类型的分隔符</param>
        /// <param name="removeEmptyEntries">是否移除数据中元素为空字符串的项</param>
        /// <returns>分割后的数据</returns>
        public static string[] Split(this string value, string strSplit, bool removeEmptyEntries = false)
        {
            return value.Split(new[] { strSplit }, removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }

        /// <summary>
        /// 保留开头指定数量的字符串，结尾替换为指定字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="num"></param>
        /// <param name="temp"></param>
        /// <returns></returns>
        public static string GetStartStr(this string str, int num, string temp = "")
        {
            if (str != null)
            {
                if (str.Length > num)
                {
                    str = str.Substring(0, num);
                    if (!string.IsNullOrEmpty(temp))
                        return str + temp;
                }
                return str;
            }
            return "";
        }

        /// <summary>
        /// 截取结尾指定数量的字符串，结尾为指定字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="num"></param>
        /// <param name="temp"></param>
        /// <returns></returns>
        public static string ReplaceEndStr(this string str, int num, string temp = "")
        {
            if (str != null)
            {
                if (str.Length > num)
                {
                    str = str.Substring(0, str.Length - num);
                    if (!string.IsNullOrEmpty(temp))
                        return str + temp;
                }
                return str;
            }
            return "";
        }

        /// <summary>
        /// 保留结尾指定数量的字符串，开头替换为指定字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="num"></param>
        /// <param name="temp"></param>
        /// <returns></returns>
        public static string GetEndStr(this string str, int num, string temp = "")
        {
            string resStr = string.Empty;
            if (str != null)
            {
                if (str.Length > num)
                {
                    str = str.Substring(str.Length - num);
                    if (!string.IsNullOrEmpty(temp))
                        return temp + str;
                }
                return str;
            }
            return "";
        }

        /// <summary>
        /// 截取开头指定数量的字符串，开头替换为指定字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="num"></param>
        /// <param name="temp"></param>
        /// <returns></returns>
        public static string ReplaceStartStr(this string str, int num, string temp = "")
        {
            if (str != null)
            {
                if (str.Length > num)
                {
                    str = str.Substring(num);
                    if (!string.IsNullOrEmpty(temp))
                        return temp + str;
                }
                return str;
            }
            return "";
        }

        /// <summary>
        /// 截取中间指定位置的字符，中间替换为指定字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="num"></param>
        /// <param name="temp"></param>
        /// <returns></returns>
        public static string ReplaceMidStr(this string str, int startNum, int endNum, string temp = "")
        {
            if (str != null)
            {
                if (str.Length > startNum && str.Length > endNum && startNum < endNum)
                {
                    if (!string.IsNullOrEmpty(temp))
                    {
                        string s1 = str.Substring(0, startNum);
                        string s2 = str.Substring(endNum);
                        return s1 + temp + s2;
                    }
                    else
                        return str.Remove(startNum, endNum - startNum);
                }
                return str;
            }
            return "";
        }

        /// <summary>
        /// 截取中间指定长度的字符，中间替换为指定字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="num"></param>
        /// <param name="temp"></param>
        /// <returns></returns>
        public static string ReplaceMidStrByLength(this string str, int startNum, int length, string temp = "")
        {
            if (str != null)
            {
                if (str.Length > startNum + length && length>0)
                {
                    if (!string.IsNullOrEmpty(temp))
                    {
                        string s1 = str.Substring(0, startNum);
                        string s2 = str.Substring(length + length);
                        return s1 + temp + s2;
                    }
                    else
                        return str.Remove(startNum, length);
                }
                return str;
            }
            return "";
        }

        #endregion

        #region 克隆对象
        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static object CloneObject(object o)
        {
            Type t = o.GetType();
            PropertyInfo[] properties = t.GetProperties();
            Object p = t.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, o, null);
            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    object value = pi.GetValue(o, null);
                    pi.SetValue(p, value, null);
                }
            }
            return p;
        } 
        #endregion

        #region 唯一值
        /// <summary>
        /// 获取GUID唯一字符串
        /// （js和sql中也可生成唯一标识）http://www.jb51.net/article/43823.htm
        /// </summary>
        /// <returns></returns>
        public static string GetGuid()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>  
        /// 根据GUID获取16位的唯一字符串  
        /// </summary>  
        /// <param name=\"guid\"></param>  
        /// <returns></returns>  
        public static string GuidTo16String()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>  
        /// 根据GUID获取唯一数字序列  
        /// </summary>  
        /// <returns></returns>  
        public static long GuidToLongID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }  

        /// <summary>
        /// 获取唯一字符串
        /// 注：循环中连续取值是相同的
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueString()
        {
            Random rand = new Random();
            return ((int)(rand.NextDouble() * 10000)).ToString() + DateTime.Now.Ticks.ToString();
        } 
        #endregion

        #region 字符过滤
        /// <summary>
        /// 删除所有的html标记
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceHtml(string str)
        {
            string[] Regexs =
                                {
                                    @"<script[^>]*?>.*?</script>",
                                    @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
                                    @"([\r\n])[\s]+",
                                    @"&(quot|#34);",
                                    @"&(amp|#38);",
                                    @"&(lt|#60);",
                                    @"&(gt|#62);",
                                    @"&(nbsp|#160);",
                                    @"&(iexcl|#161);",
                                    @"&(cent|#162);",
                                    @"&(pound|#163);",
                                    @"&(copy|#169);",
                                    @"&#(\d+);",
                                    @"-->",
                                    @"<!--.*\n"
                                };

            string[] Replaces =
                                {
                                    "",
                                    "",
                                    "",
                                    "\"",
                                    "&",
                                    "<",
                                    ">",
                                    " ",
                                    "\xa1", //chr(161),
                                    "\xa2", //chr(162),
                                    "\xa3", //chr(163),
                                    "\xa9", //chr(169),
                                    "",
                                    "\r\n",
                                    ""
                                };

            string s = str;
            for (int i = 0; i < Regexs.Length; i++)
            {
                s = new Regex(Regexs[i], RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(s, Replaces[i]);
            }
            s.Replace("<", "");
            s.Replace(">", "");
            s.Replace("\r\n", "");
            return s;
        }

        /// <summary>
        /// 删除所有的html元素
        /// </summary>
        /// <param name="html"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ReplaceHtmlEle(string html, int length = 0)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }

        /// <summary>
        /// 过滤非法字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceBadChar(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            string strBadChar, tempChar;
            string[] arrBadChar;
            strBadChar = "@@,+,',--,%,^,&,?,(,),<,>,[,],{,},/,\\,;,:,\",\"\"";
            arrBadChar = strBadChar.Split(',');
            tempChar = str;
            for (int i = 0; i < arrBadChar.Length; i++)
            {
                if (arrBadChar[i].Length > 0)
                    tempChar = tempChar.Replace(arrBadChar[i], "");
            }
            return tempChar;
        } 
        #endregion

        #region 时间
        /// <summary>
        /// 获取日期间隔
        /// </summary>
        /// <param name="dtStar"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public static string GetTimeDelay(DateTime dtStar, DateTime dtEnd)
        {
            long lTicks = (dtEnd.Ticks - dtStar.Ticks) / 10000000;
            string sTemp = (lTicks / 3600).ToString().PadLeft(2, '0') + ":";
            sTemp += ((lTicks % 3600) / 60).ToString().PadLeft(2, '0') + ":";
            sTemp += ((lTicks % 3600) % 60).ToString().PadLeft(2, '0');

            return sTemp;
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp(DateTime datetime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, datetime.Kind);
            return Convert.ToInt64((datetime - start).TotalSeconds);
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTime(long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        } 
        #endregion

        #region 字符串加解密

        /// <summary>
        /// MD5 hash加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5(string str, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = UnicodeEncoding.UTF8;
            }
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var result = BitConverter.ToString(md5.ComputeHash(encoding.GetBytes(str.Trim())));
            return result;
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="str">待加密的明文</param>
        /// <returns></returns>
        public static string EncodeBase64(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            string result = Convert.ToBase64String(bytes);
            return result;
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="str">待解密的密文</param>
        /// <returns></returns>
        public static string DecodeBase64(string str)
        {
            byte[] bytes = Convert.FromBase64String(str);
            string result = Encoding.UTF8.GetString(bytes);
            return result;
        }

        /// <summary>
        /// 获取字符串的SHA1哈希值
        /// </summary>
        public static string GetSha1(string value)
        {
            StringBuilder sb = new StringBuilder();
            SHA1Managed hash = new SHA1Managed();
            byte[] bytes = hash.ComputeHash(Encoding.ASCII.GetBytes(value));
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取字符串的Sha256哈希值
        /// </summary>
        public static string GetSha256(string value)
        {
            StringBuilder sb = new StringBuilder();
            SHA256Managed hash = new SHA256Managed();
            byte[] bytes = hash.ComputeHash(Encoding.ASCII.GetBytes(value));
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取字符串的Sha512哈希值
        /// </summary>
        public static string GetSha512(string value)
        {
            StringBuilder sb = new StringBuilder();
            SHA512Managed hash = new SHA512Managed();
            byte[] bytes = hash.ComputeHash(Encoding.ASCII.GetBytes(value));
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }
        #endregion
    }
}