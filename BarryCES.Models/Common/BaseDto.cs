using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarryCES.Models
{
    public class BaseDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        public DateTime EditDateTime { get; set; }

        public string CreateUserId { get; set; }

        public string EditUserId { get; set; }
        
    }
}
