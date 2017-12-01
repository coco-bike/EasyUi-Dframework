using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    /// <summary>
    /// 分页实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagingResult<T>
    {
        public PagingResult()
        {
            List = new List<T>();
        }
        public PagingResult(List<T> list, int total)
        {
            List = list;
            Total = total;
        }
        /// <summary>
        /// 数据（数组）
        /// </summary>
        public List<T> List { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; }
    }

    /// <summary>
    /// 分页实体(扩展实体)
    /// </summary>
    /// <typeparam name="T1">分页数据结构</typeparam>
    /// <typeparam name="T2">扩展数据结构</typeparam>
    public class PagingResultExtendModel<T1, T2> : PagingResult<T1>
    {
        public PagingResultExtendModel()
        {
            List = new List<T1>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list">实体数据</param>
        /// <param name="total">总记录数</param>
        /// <param name="data">扩展数据</param>
        public PagingResultExtendModel(List<T1> list, int total, T2 data)
        {
            List = list ?? new List<T1>();
            Total = total;
            Data = data;
        }

        /// <summary>
        /// 分页数据转换成带扩展数据的分页结构数据
        /// </summary>
        /// <param name="padingData">分页数据</param>
        public PagingResultExtendModel(PagingResult<T1> padingData)
        {
            List = padingData.List;
            Total = padingData.Total;
        }

        /// <summary>
        /// 分页数据转换成带扩展数据的分页结构数据
        /// </summary>
        /// <param name="padingData">分页数据</param>
        /// <param name="extendData">扩展数据</param>
        public PagingResultExtendModel(PagingResult<T1> padingData, T2 extendData)
        {
            List = padingData.List;
            Total = padingData.Total;
            Data = extendData;
        }

        /// <summary>
        /// 扩展数据
        /// </summary>
        public T2 Data { get; set; }
    }

    /// <summary>
    /// 分页实体(扩展数组)
    /// </summary>
    /// <typeparam name="T1">分页数据结构</typeparam>
    /// <typeparam name="T2">扩展数据结构</typeparam>
    public class PagingResultExtendList<T1, T2> : PagingResult<T1>
    {
        public PagingResultExtendList()
        {
            List = new List<T1>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list">实体数据</param>
        /// <param name="total">总记录数</param>
        /// <param name="data">扩展数据</param>
        public PagingResultExtendList(List<T1> list, int total, List<T2> data)
        {
            List = list ?? new List<T1>();
            Total = total;
            Data = data ?? new List<T2>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">分页数据</param>
        public PagingResultExtendList(PagingResult<T1> data)
        {
            List = data.List;
            Total = data.Total;
        }

        /// <summary>
        /// 分页数据转换成带扩展数据的分页结构数据
        /// </summary>
        /// <param name="padingData">分页数据</param>
        /// <param name="extendData">扩展数据</param>
        public PagingResultExtendList(PagingResult<T1> padingData, List<T2> extendData)
        {
            List = padingData.List;
            Total = padingData.Total;
            Data = extendData;
        }

        /// <summary>
        /// 扩展数据（数组）
        /// </summary>
        public List<T2> Data { get; set; }
    }
}