using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public class PagingResult<T>
    {
        public PagingResult()
        {
            List = new List<T>();
        }
        public PagingResult(IEnumerable<T> list, int totalCount)
        {
            List = list;
            TotalCount = totalCount;
        }
        /// <summary>
        /// 数据
        /// </summary>
        public IEnumerable<T> List { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; }
    }
}