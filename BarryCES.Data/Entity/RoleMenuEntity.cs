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
    /// 角色菜单关系实体
    /// </summary>
    public class RoleMenuEntity : BaseEntity
    {

        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public string MenuId { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public virtual RoleEntity Role { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        public virtual MenuEntity Menu { get; set; }
    }
}
