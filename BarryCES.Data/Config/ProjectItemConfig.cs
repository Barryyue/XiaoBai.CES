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
    /// 邮件表配置
    /// </summary>
    public class ProjectItemConfig : EntityTypeConfiguration<ProjectItemEntity>
    {
        public ProjectItemConfig()
        {
            ToTable("ProjectItems");
            HasKey(item => item.Id);
            Property(item => item.Id).HasColumnType("varchar").HasMaxLength(20);
            Property(item => item.Name).HasColumnType("nvarchar").IsRequired().HasMaxLength(100);
            Property(item => item.Price).IsRequired();

            HasRequired(item => item.Project).WithMany(p => p.ProjectItems).HasForeignKey(item => item.ProjectId);
        }
    }
}
