using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class StringExt
    { 
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool CanPrint(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        public static string JoinStr(this IEnumerable<string> strList, string separator)
        {
            return string.Join(separator, strList);
        }
        public static string JoinStr(this IEnumerable<string> strList)
        {
            return string.Join("", strList);
        }


        /// <summary> 输入字符串错误时将抛出异常 </summary>
        public static int ToInt32(this string str)
        {
            return int.Parse(str);
        }

        /// <summary> 输入字符串错误时将抛出异常 </summary>
        public static long ToInt64(this string str)
        {
            return long.Parse(str);
        }
        public static bool TryParseInt(this string str, out int result)
        {
            return int.TryParse(str, out result);
        }



        public static DateTime ToDateTime(this string str)
        {
            return DateTime.Parse(str);
        }

        /// <summary> 输入字符串错误时将抛出异常 </summary>
        public static bool ToBool(this string str)
        {
            return bool.Parse(str);
        }


        public static decimal ToDecimal(this string str)
        {
            return decimal.Parse(str == "" ? "0" : str);
        }

        public static bool In(this string str, IEnumerable<string> list)
        {
            return list.Contains(str);
        }

        public static string Match(this string str, string pattern)
        {
            return Regex.Match(str, pattern, RegexOptions.Singleline).Value;
        }

        public static IEnumerable<string> Matches(this string str, string pattern)
        {
            return from Match match in Regex.Matches(str, pattern, RegexOptions.Singleline) select match.Value;
        }

        public static string ReplaceRegex(this string str, string pattern, string newString)
        {
            return Regex.Replace(str, pattern, newString);
        }


        public static bool IsMatch(this string str, string pattern)
        {
            return Regex.IsMatch(str, pattern, RegexOptions.Singleline);
        }

        public static T Parse<T>(this string str, Func<string, T> parse)
        {
            return parse(str);
        }

        public static string UrlEncode(this string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        public static string UrlDecode(this string str)
        {
            return HttpUtility.UrlDecode(str);
        }
        public static string UrlDecode(this string str, Encoding encoding)
        {
            return HttpUtility.UrlDecode(str, encoding);
        }

        public static string FormatExt(this string str, params object[] param)
        {
            return string.Format(str, param);
        }

        //public static T JsonDeserialize<T>(this string str)
        //{
        //    return JsonParser.Deserialize<T>(str);
        //}

        public static T XmlDeserialize<T>(this string str)
        {
            return (T)new XmlSerializer(typeof(T)).Deserialize(new StringReader(str));
        }

        //public static bool TryXmlDeserialize<T>(this string str, out T model)
        //{
        //    try
        //    {
        //        model = (T)new XmlSerializer(typeof(T)).Deserialize(new StringReader(str));
        //    }
        //    catch (Exception ex)
        //    {
        //        model = default(T);
        //        ex.Log("xml反序列化时发生异常,");
        //        return false;
        //    }
        //    return true;
        //}

        //public static XNode Json2XNode(this string str)
        //{
        //    return JsonParser.DeserializeXNode(str);
        //}

        //public static XmlNode Json2XmlNode(this string str)
        //{
        //    return JsonParser.DeserializeXmlNode(str);
        //}
        ///// <summary> 字符串默认MD5化 </summary>
        //public static string MD5Default(this string str)
        //{
        //    return str.GetUTF8Bytes().MD5Default().Select(m => m.ToString("X2")).JoinStr("");
        //}

        //public static string MD5GBK(this string str)
        //{
        //    return str.GetGBKBytes().MD5Default().Select(m => m.ToString("X2")).JoinStr("");
        //}

        //public static string MD5Unicode(this string str)
        //{
        //    return str.GetBytes(Encoding.Unicode).MD5Default().Select(m => m.ToString("X2")).JoinStr("");
        //}

        public static byte[] GetUTF8Bytes(this string str)
        {
            return str.GetBytes(Encoding.UTF8);
        }

        public static byte[] GetGBKBytes(this string str)
        {
            return str.GetBytes(Encoding.GetEncoding("GBK"));
        }

        public static byte[] GetBytes(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        //public static void ToLogger(this string str, LogLevel level)
        //{
        //    ModulesFactory.Logger.Log(level, null, str);
        //}
        //public static void ToLogger(this string str)
        //{
        //    ModulesFactory.Logger.Log(LogLevel.Information, null, str);
        //}

        public static string Encode2GBK(this string str)
        {
            return str.Encode(Encoding.GetEncoding("GBK"));
        }

        public static string Encode(this string str, Encoding encoding)
        {
            //todo 不必要代码
            Encoding utf8 = Encoding.UTF8;
            Encoding gbk = encoding;
            byte[] unicodeBytes = utf8.GetBytes(str);
            byte[] ascIiBytes = Encoding.Convert(utf8, gbk, unicodeBytes);
            var ascIiChars = new char[gbk.GetCharCount(ascIiBytes, 0, ascIiBytes.Length)];
            gbk.GetChars(ascIiBytes, 0, ascIiBytes.Length, ascIiChars, 0);
            var gb2312Info = new string(ascIiChars);
            return gb2312Info;
        }

        public static string TrimAll(this string str)
        {
            if (str.IsNullOrWhiteSpace())
            {
                return "";
            }
            int count = 0;
            char[] chars = str.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (Char.IsWhiteSpace(chars[i]))
                {
                    int j = i;
                    while (Char.IsWhiteSpace(chars[j]) && j < chars.Length - 1)
                    {
                        j++;
                    }
                    var temp = chars[i];
                    chars[i] = chars[j];
                    chars[j] = temp;
                }
            }
            for (int i = 0; i < chars.Length; i++)
            {
                if (!Char.IsWhiteSpace(chars[i]))
                {
                    count = i;
                }
            }
            return new string(chars, 0, count + 1);
        }
         
        /// <summary>
        /// 获取字节长度
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetBytesLenght(this string str)
        {
            int len = Encoding.Default.GetBytes(str).Length;
            //if (len > 32)
            //{
            //    return len - 32;
            //}
            //else
            //{
            return len;
            //}
        }

        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetAppSetting(this string str)
        {
            return ConfigurationManager.AppSettings[str];
        }

    }
}