using System;

namespace BarryCES.Models
{
    /// <summary>
    /// 访问记录
    /// </summary>
    public class VisitDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// UserName
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 访问者IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 访问时间
        /// </summary>
        public DateTime VisitDate { get; set; }
    }

    /// <summary>
    /// 访问数据统计DTO
    /// </summary>
    public class VisitDataDto
    {
        /// <summary>
        /// 访问日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 访问量
        /// </summary>
        public int Number { get; set; }
    }
}
