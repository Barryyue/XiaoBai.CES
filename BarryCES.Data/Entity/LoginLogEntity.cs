﻿/*******************************************************************************
* Copyright (C) BarryCES.Com
* 
* Author: Barry.yue
* Create Date: 09/04/2015 11:47:14
* Description: Automated building by service@BarryCES.com 
* 
* Revision History:
* Date         Author               Description
*
*********************************************************************************/


namespace BarryCES.Data.Entity
{
    /// <summary>
    /// 用户登录日志实体
    /// </summary>
    public class LoginLogEntity : BaseEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 登录IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 登录结果信息
        /// </summary>
        public string Mac { get; set; }
    }
}
