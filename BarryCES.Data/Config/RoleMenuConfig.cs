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
    /// 角色菜单关系表配置
    /// </summary>
    public class RoleMenuConfig : EntityTypeConfiguration<RoleMenuEntity>
    {
        public RoleMenuConfig()
        {
            ToTable("RoleMenu");
            HasKey(item => item.Id);
            Property(item => item.Id).HasColumnType("varchar").HasMaxLength(20);

            Property(item => item.RoleId).IsRequired();
            Property(item => item.MenuId).IsRequired();

            HasRequired(item => item.Menu).WithMany(item => item.RoleMenus).HasForeignKey(item => item.MenuId);
            HasRequired(item => item.Role).WithMany(item => item.RoleMenus).HasForeignKey(item => item.RoleId);
        }
    }
}
