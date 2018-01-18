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

namespace BarryCES.Data.Entity
{
    /// <summary>
    /// 用户角色关系实体
    /// </summary>
    public class UserRoleEntity : BaseEntity
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public virtual UserEntity User { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public virtual RoleEntity Role { get; set; }
    }
}
