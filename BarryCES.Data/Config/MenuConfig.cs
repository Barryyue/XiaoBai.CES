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
    /// 菜单表配置
    /// </summary>
    public class MenuConfig : EntityTypeConfiguration<MenuEntity>
    {
        public MenuConfig()
        {
            ToTable("Menu");
            HasKey(item => item.Id);
            Property(item => item.Id).HasColumnType("varchar").HasMaxLength(20);

            Property(item => item.Name).HasColumnType("nvarchar").IsRequired().HasMaxLength(20);
            Property(item => item.Url).HasColumnType("varchar").IsRequired().HasMaxLength(300);
            Property(item => item.Type).IsRequired();
        }
    }
}
