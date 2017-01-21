using Utils;

namespace System.Web.Mvc
{
    /// <summary>
    /// 本地化帮助类
    /// </summary>
    public static class LocalizationHelpers
    {
        /// <summary>
        /// 在Html中直接使用，对页面元素输出字符串
        /// webform中：<%= Html.Lang("String1") %>
        /// mvc中：@Html.Lang("TaskType")
        /// </summary>
        /// <param name="htmlhelper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Lang(this HtmlHelper htmlhelper, string key)
        {
            // 资源文件路径
            string FilePath = htmlhelper.ViewContext.HttpContext.Server.MapPath("/") + "Resource\\";
            return GetLangString(htmlhelper.ViewContext.HttpContext, key, FilePath);
        }

        /// <summary>
        /// 在Html中直接使用，对JS输出字符串
        /// JS中：<%= Html.LangOutJsVar("String1")%> || @Html.Lang("String1")
        /// JS中：alert(String1)
        /// </summary>
        /// <param name="htmlhelper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string LangOutJsVar(this HtmlHelper htmlhelper, string key)
        {
            // 资源文件路径
            string FilePath = htmlhelper.ViewContext.HttpContext.Server.MapPath("/") + "Resource\\";
            string langstr = GetLangString(htmlhelper.ViewContext.HttpContext, key, FilePath);
            return string.Format("var {0} = '{1}'", key, langstr);
        }

        /// <summary>
        /// 在C#中使用，对后台输出字符串
        /// ViewData["Message"] = LocalizationHelpers.InnerLang(this.ControllerContext.HttpContext, "String1");
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string InnerLang(HttpContextBase httpContext, string key)
        {
            string FilePath = httpContext.Server.MapPath("/") + "Resource\\";
            return GetLangString(httpContext, key, FilePath);
        }

        /// <summary>
        /// 输出字符串
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="key"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        private static string GetLangString(HttpContextBase httpContext, string key, string FilePath)
        {
            int langtype = 0;
            if (SessionHelper.GetSession("Lang") != null)
            {
                langtype = (SessionHelper.GetSession("Lang").ToString().Equals("en")) ? 1 : 0;
            }

            try
            {
                System.Resources.ResourceManager rs;
                switch (langtype)
                {
                    case 0: rs = new System.Resources.ResourceManager("Utils.Lang_zh", typeof(Lang_zh).Assembly); break;
                    case 1: rs = new System.Resources.ResourceManager("Utils.Lang_en", typeof(Lang_en).Assembly); break;
                    default: rs = new System.Resources.ResourceManager("Utils.Lang_zh", typeof(Lang_zh).Assembly); break;
                }

                return rs.GetString(key);
            }
            catch (Exception ex)
            {
            }
            return "";
        }
    }
}