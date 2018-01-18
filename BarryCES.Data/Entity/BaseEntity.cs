using System;

namespace BarryCES.Data.Entity
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            IsDeleted = false;
            CreateDateTime = DateTime.Now;
            EditDateTime = DateTime.Now;
        }

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

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
