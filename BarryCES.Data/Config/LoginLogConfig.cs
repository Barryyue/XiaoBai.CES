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

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BarryCES.Data.Entity;

namespace BarryCES.Data.Config
{
    /// <summary>
    /// 登录日志表配置
    /// </summary>
    public class LoginLogConfig : EntityTypeConfiguration<LoginLogEntity>
    {
        public LoginLogConfig()
        {
            ToTable("LoginLog");
            HasKey(item => item.Id);
            Property(item => item.Id).HasColumnType("varchar").HasMaxLength(20);

            Property(item => item.LoginName).HasColumnType("varchar").IsRequired().HasMaxLength(20);
            Property(item => item.UserId).IsRequired();
            Property(item => item.IP).HasColumnType("varchar").IsOptional().HasMaxLength(15);
            Property(item => item.Mac).HasColumnType("varchar").IsOptional().HasMaxLength(40);
        }
    }
}
