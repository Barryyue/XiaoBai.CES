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
    /// 角色表配置
    /// </summary>
    public class UserRoleConfig : EntityTypeConfiguration<UserRoleEntity>
    {
        public UserRoleConfig()
        {
            ToTable("UserRole");
            HasKey(item => item.Id);
            Property(item => item.Id).HasColumnType("varchar").HasMaxLength(20);
            Property(item => item.RoleId).IsRequired();
            Property(item => item.UserId).IsRequired();
            HasRequired(item => item.User).WithMany(item => item.UserRoles).HasForeignKey(item => item.UserId);
            HasRequired(item => item.Role).WithMany(item => item.UserRoles).HasForeignKey(item => item.RoleId);
        }
    }
}
