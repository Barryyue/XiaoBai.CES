using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BarryCES.Models
{
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleDto: BaseDto
    {

        /// <summary>
        /// 角色名称
        /// </summary>
        [DisplayName("角色名称"), MinLength(2), MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DisplayName("角色描述"), MaxLength(50)]
        public string Description { get; set; }
    }
}
