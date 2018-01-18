/*******************************************************************************
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
    /// 用户表配置
    /// </summary>
    public class SystemConfig : EntityTypeConfiguration<SystemConfigEntity>
    {
        public SystemConfig()
        {
            ToTable("SystemConfig");
            HasKey(item => item.Id);
            Property(item => item.Id).HasColumnType("varchar").HasMaxLength(20);
            Property(item => item.SystemName).HasColumnType("varchar").IsRequired().HasMaxLength(20);
            Property(item => item.IsDataInited).IsRequired();
            Property(item => item.DataInitedDate).IsRequired();
        }
    }
}
