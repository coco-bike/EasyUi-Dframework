using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IDal
{
    public interface IBaseDal<T> where T : class,new()
    {
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda);
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="pageIndex">分页主页</param>
        /// <param name="pageSize">当前页码</param>
        /// <param name="totalCount">页面数</param>
        /// <param name="isDec"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderByLambda"></param>
        /// <returns></returns>
        IQueryable<T> GetPagingList<S>(int pageIndex, int pageSize, out int totalCount, bool isDec, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderByLambda);
        /// <summary>
        /// 单个添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        T Add(T model);
        /// <summary>
        /// 多个添加
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        IEnumerable<T> AddRange(IEnumerable<T> models);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="updateLambda"></param>
        /// <returns></returns>
        int GetUpdate(Expression<Func<T, bool>> whereLambda, Expression<Func<T, T>> updateLambda);
        /// <summary>
        /// 更新数目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Update(T model);
        /// <summary>
        /// 更新列表
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        bool UpdateEntityList(IEnumerable<T> entityList);
        /// <summary>
        /// 真删
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        int DeleteReal(Expression<Func<T, bool>> whereLambda);
        /// <summary>
        /// 假删
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="updateLambda"></param>
        /// <returns></returns>
        int DeleteFake(Expression<Func<T, bool>> whereLambda, Expression<Func<T, T>> updateLambda);
    }
}
