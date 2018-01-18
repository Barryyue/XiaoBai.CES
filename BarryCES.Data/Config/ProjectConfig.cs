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

using System.Data.Entity.ModelConfiguration;
using BarryCES.Data.Entity;

namespace BarryCES.Data.Config
{
    /// <summary>
    /// 项目配置
    /// </summary>
    public class ProjectConfig : EntityTypeConfiguration<ProjectEntity>
    {
        public ProjectConfig()
        {
            ToTable("Project");
            HasKey(item => item.Id);
            Property(item => item.Id).HasColumnType("varchar").HasMaxLength(20);
            Property(item => item.Name).HasColumnType("nvarchar").IsRequired().HasMaxLength(100);
        }
    }
}
