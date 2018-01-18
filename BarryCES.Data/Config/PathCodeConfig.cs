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
    /// 路径码表配置
    /// </summary>
    public class PathCodeConfig : EntityTypeConfiguration<PathCodeEntity>
    {
        public PathCodeConfig()
        {
            ToTable("PathCodes");
            HasKey(item => item.Id);
            Property(item => item.Id).HasColumnType("varchar").HasMaxLength(20);

            Property(item => item.Code).HasColumnType("char").IsRequired().HasMaxLength(40);
            Property(item => item.Len).IsRequired();
        }
    }
}
