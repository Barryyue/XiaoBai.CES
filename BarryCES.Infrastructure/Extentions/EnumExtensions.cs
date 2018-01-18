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
using System.ComponentModel;
using System.Reflection;


namespace BarryCES.Infrastructure.Extentions
{
    /// <summary>
    ///     枚举扩展方法类
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        ///     获取枚举项的Description特性的描述文字
        /// </summary>
        /// <param name="value">枚举对象</param>
        /// <returns> </returns>
        public static string ToDescription(this Enum value)
        {
            if (value == null)
                return string.Empty;
            return GetDescriptionForEnum(value);
        }

        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescriptionForEnum(this object value)
        {
            if (value == null) return string.Empty;
            var type = value.GetType();
            var field = type.GetField(Enum.GetName(type, value));
            if (field != null)
            {
                var des = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (des != null)
                    return des.Description;
            }
            return value.ToString();
        }
    }
}