using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IDal;
using Dal;
using Session;

namespace Service
{
    public abstract class BaseService<T> where T : class,new()
    {
        /// <summary>
        /// 创建线程内唯一的DbSession
        /// </summary>
        public IDbSession dbSession
        {
            get
            {
                return DbSessionOnly.CreateDbSession();
            }
        }

        #region 此段断码为了在不同的Service里传入不同的Dal
        public IBaseDal<T> CurrentDal { get; set; }
        public abstract void SetCurrentDal();
        public BaseService()
        {
            SetCurrentDal();
        }
        #endregion

        #region 查询
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda)
        {
            return CurrentDal.GetList(whereLambda);

        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="isDec"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderByLambda"></param>
        /// <returns></returns>
        public IQueryable<T> GetPagingList<S>(int pageIndex, int pageSize, out int totalCount, bool isDec, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderByLambda)
        {
            return CurrentDal.GetPagingList<S>(pageIndex, pageSize, out totalCount, isDec, whereLambda, orderByLambda);
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增（单个）
        /// </summary>
        /// <param name="model"></param>
        public T Add(T model)
        {
            var res = CurrentDal.Add(model);
            if (dbSession.SaveChanges())
            {
                return res;
            }
            return null;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> models)
        {
            var res = CurrentDal.AddRange(models);
            if (dbSession.SaveChanges())
            {
                return res;
            }
            return null;
        }
        #endregion

        #region 修改
        public int GetUpdate(Expression<Func<T, bool>> whereLambda, Expression<Func<T, T>> updateLambda)
        {
            return CurrentDal.GetUpdate(whereLambda, updateLambda);
        }


        public int Update(T model)
        {
            return CurrentDal.Update(model);
        }

        public bool UpdateEntityList(IEnumerable<T> entityList)
        {
            return CurrentDal.UpdateEntityList(entityList);
        }

        #endregion

        #region 删除
        /// <summary>
        ///    真删
        /// </summary>
        /// <param name="whereLambda"></param>
        public int DeleteReal(Expression<Func<T, bool>> whereLambda)
        {
            return CurrentDal.DeleteReal(whereLambda);
        }

        /// <summary>
        /// 假删（更新数据库state字段为0）
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="updateLambda"></param>
        /// <returns></returns>
        public int DeleteFake(Expression<Func<T, bool>> whereLambda, Expression<Func<T, T>> updateLambda)
        {
            return CurrentDal.DeleteFake(whereLambda, updateLambda);
        }
        #endregion

    }
}
