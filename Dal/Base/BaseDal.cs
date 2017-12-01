using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;

namespace Dal
{
    public class BaseDal<T> where T : class,new()
    {
        protected DbContext entity
        {
            get
            {
                return DbContextOnly.CreateEF();
            }
        }

        #region 查询
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="whereLamba"></param>
        /// <returns></returns>
        public IQueryable<T> GetList(Expression<Func<T, bool>> whereLamba)
        {
            return entity.Set<T>().Where<T>(whereLamba);
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
            var temp = entity.Set<T>().Where<T>(whereLambda);
            totalCount = temp.Count();
            if (isDec)
            {
                temp = temp.OrderByDescending<T, S>(orderByLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            return temp;
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增单个
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public T Add(T model)
        {
            model.GetType().GetProperty("State").SetValue(model, 1);

            model.GetType().GetProperty("BuildTime").SetValue(model, DateTime.Now);

            model.GetType().GetProperty("UpdateTime").SetValue(model, DateTime.Now);

            return entity.Set<T>().Add(model);
        }

        /// <summary>
        /// 新增多个
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public IEnumerable<T> AddRange(IEnumerable<T> models)
        {
            return entity.Set<T>().AddRange(models);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 获取更新数目
        /// </summary>
        /// <param name="whereLamba"></param>
        /// <param name="updateLamda"></param>
        /// <returns></returns>
        public int GetUpdate(Expression<Func<T, bool>> whereLamba, Expression<Func<T, T>> updateLamda)
        {
            return entity.Set<T>().Where(whereLamba).Update(updateLamda);
        }

        /// <summary>
        /// 更新单个
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(T model)
        {
            this.entity.Entry<T>(model).State = System.Data.Entity.EntityState.Modified;
            return this.entity.SaveChanges();
        }
        /// <summary>
        /// 更新多个
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public bool UpdateEntityList(IEnumerable<T> entityList)
        {
            foreach (var entity in entityList)
            {
                this.entity.Entry(entity).State = EntityState.Modified;
            }
            return true;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 真删
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>

        public int DeleteReal(Expression<Func<T, bool>> whereLambda)
        {
            return entity.Set<T>().Where(whereLambda).Delete();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="updateLambda"></param>
        /// <returns></returns>
        public int DeleteFake(Expression<Func<T, bool>> whereLambda, Expression<Func<T, T>> updateLambda)
        {
            return entity.Set<T>().Where(whereLambda).Update(updateLambda);
        }
        #endregion


    }

}
