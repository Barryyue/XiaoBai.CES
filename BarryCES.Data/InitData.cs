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

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BarryCES.Data.Entity;
using BarryCES.Infrastructure.Utilities;

namespace BarryCES.Data
{
    /// <summary>
    /// 初始化数据
    /// </summary>
    public static class InitData
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BarryCESContext, DbConfiguration>());
        }

        /// <summary>
        /// 获取路径码
        /// </summary>
        /// <returns></returns>
        public static List<PathCodeEntity> GetPathCodes()
        {
            var instance = BaseIdGenerator.Instance;
            //生成路径码
            var codes = new List<string>(26);
            for (int i = 65; i <= 90; i++)
            {
                codes.Add(((char)i).ToString());
            }
            int len = 2;
            //求组合
            List<string[]> ermutation = PermutationAndCombination<string>.GetCombination(codes.ToArray(), len);
            var list = new List<PathCodeEntity>();
            ermutation.ForEach(item =>
            {
                list.Add(new PathCodeEntity
                {
                    Id = instance.GetId(),
                    Code = string.Join(string.Empty, item),
                    Len = len
                });
                list.Add(new PathCodeEntity
                {
                    Id = instance.GetId(),
                    Code = string.Join(string.Empty, item.Reverse()),
                    Len = len
                });
            });
            Func<IEnumerable<PathCodeEntity>> getSameKeyFunc = () =>
            {
                return codes.Select(key => new PathCodeEntity
                {
                    Id = instance.GetId(),
                    Code = string.Join(string.Empty, key, key),
                    Len = len
                });
            };
            list.AddRange(getSameKeyFunc());
            list = list.OrderBy(item => item.Code).ToList();

            return list;
        }
    }
}
