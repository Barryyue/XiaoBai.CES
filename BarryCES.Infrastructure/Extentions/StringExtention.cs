﻿/*******************************************************************************
* Copyright (C) AspxPet.Com
* 
* Author: Barry.yue
* Create Date: 09/04/2015 11:47:14
* Description: Automated building by service@aspxpet.com 
* 
* Revision History:
* Date         Author               Description
*
*********************************************************************************/

using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace BarryCES.Infrastructure.Extentions
{
    /// <summary>
    /// 字符串扩展类
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 用于判断是否为空字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsBlank(this string s)
        {
            return s == null || (s.Trim().Length == 0);
        }

        /// <summary>
        /// 用于判断是否为空字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNotBlank(this string s)
        {
            return !s.IsBlank();
        }

        /// <summary>
        /// 将字符串转换成MD5加密字符串
        /// </summary>
        /// <param name="orgStr"></param>
        /// <returns></returns>
        public static string ToMd5(this string orgStr)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var encoding = Encoding.UTF8;
                var encryptedBytes = md5.ComputeHash(encoding.GetBytes(orgStr));
                var sb = new StringBuilder(32);
                foreach (var bt in encryptedBytes)
                {
                    sb.Append(bt.ToString("x").PadLeft(2, '0'));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 将对象序列化成XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToXml<T>(this T obj) where T : class
        {
            return ToXml(obj, Encoding.Default.BodyName);
        }
        /// <summary>
        /// 将对象序列化成XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="encodeName"></param>
        /// <returns></returns>
        public static string ToXml<T>(this T obj, string encodeName) where T : class
        {
            if (obj == null) throw new ArgumentNullException("obj", "obj is null.");

            if (obj is string) throw new ArgumentException("obj can't be string object.");

            var en = Encoding.GetEncoding(encodeName);
            var serial = new XmlSerializer(typeof(T));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var ms = new MemoryStream();
            var xt = new XmlTextWriter(ms, en);
            serial.Serialize(xt, obj, ns);
            xt.Close();
            ms.Close();
            return en.GetString(ms.ToArray());
        }
        /// <summary>
        /// 将XML字符串反序列化成对象实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T Deserial<T>(this string s) where T : class
        {
            return Deserial<T>(s, Encoding.Default.BodyName);
        }

        /// <summary>
        /// Deserial
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="encodeName"></param>
        /// <returns></returns>
        public static T Deserial<T>(this string s, string encodeName) where T : class
        {
            if (s.IsBlank())
            {
                throw new ApplicationException("xml string is null or empty.");
            }
            var serial = new XmlSerializer(typeof(T));
            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            return (T)serial.Deserialize(new StringReader(s));
        }
        /// <summary>
        /// 获取扩展名
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetExt(this string s)
        {
            var ret = string.Empty;
            if (!s.Contains('.')) return ret;
            var temp = s.Split('.');
            ret = temp[temp.Length - 1];

            return ret;
        }
        /// <summary>
        /// 验证QQ格式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsQq(this string s)
        {
            return s.IsBlank() || Regex.IsMatch(s, @"^[1-9]\d{4,15}$");
        }

        /// <summary>
        /// 判断是否为有效的Email地址
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmail(this string s)
        {
            if (!s.IsBlank())
            {
                const string pattern = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
                return Regex.IsMatch(s, pattern);
            }
            return false;
        }

        /// <summary>
        /// 验证是否是合法的电话号码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsPhone(this string s)
        {
            if (!s.IsBlank())
            {
                return Regex.IsMatch(s, @"^\+?((\d{2,4}(-)?)|(\(\d{2,4}\)))*(\d{0,16})*$");
            }
            return true;
        }

        /// <summary>
        /// 验证是否是合法的手机号码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsMobile(this string s)
        {
            if (!s.IsBlank())
            {
                return Regex.IsMatch(s, @"^\+?\d{0,4}?[1][3-8]\d{9}$");
            }
            return false;
        }

        /// <summary>
        /// 验证是否是合法的邮编
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsZipCode(this string s)
        {
            if (!s.IsBlank())
            {
                return Regex.IsMatch(s, @"[1-9]\d{5}(?!\d)");
            }
            return true;
        }

        /// <summary>
        /// 验证是否是合法的传真
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsFax(this string s)
        {
            if (!s.IsBlank())
            {
                return Regex.IsMatch(s, @"(^[0-9]{3,4}\-[0-9]{7,8}$)|(^[0-9]{7,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}$)");
            }
            return true;
        }

        /// <summary>
        /// 检查字符串是否为有效的int数字
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsInt(this string val)
        {
            if (IsBlank(val))
                return false;
            int k;
            return int.TryParse(val, out k);
        }

        /// <summary>
        /// 字符串转数字，未转换成功返回0
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int ToInt(this string val)
        {
            if (IsBlank(val))
                return 0;
            int k;
            return int.TryParse(val, out k) ? k : 0;
        }

        /// <summary>
        /// 字符串转数字，未转换成功返回0
        /// </summary>
        /// <param name="val"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int ToIntWithDefaultValue(this string val, int defaultValue = 0)
        {
            if (IsBlank(val))
                return 0;
            int k;
            return int.TryParse(val, out k) ? k : defaultValue;
        }

        /// <summary>
        /// 检查字符串是否为有效的INT64数字
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsInt64(this string val)
        {
            if (IsBlank(val))
                return false;
            long k;
            return long.TryParse(val, out k);
        }

        /// <summary>
        /// 检查字符串是否为有效的Decimal
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsDecimal(this string val)
        {
            if (IsBlank(val))
                return false;
            decimal d;
            return decimal.TryParse(val, out d);
        }
    }
}
