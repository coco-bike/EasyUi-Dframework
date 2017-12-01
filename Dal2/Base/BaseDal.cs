﻿// <autogenerated>
//   This file was generated by T4 code generator Configuration.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

//=============================================================================
// <auto-generated>
//     此代码由工具生成。
//     对此文件的更改可能会导致不正确的行为，并且如果重新生成代码，这些更改将会丢失。
// </auto-generated>
//=============================================================================
using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using System.Runtime.Remoting.Messaging;
using System.Reflection;
namespace Dal
{
    public class BaseDal<T> where T : class,new()
    {
        /// <summary>
        /// 创建线程内唯一的上下文对象
        /// </summary>
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
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda)
        {
            return entity.Set<T>().Where<T>(whereLambda);
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
            else
            {
                temp = temp.OrderBy<T, S>(orderByLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            return temp;
        }
        #endregion


        #region 新增
        /// <summary>
        /// 新增（单个）
        /// </summary>
        /// <param name="model"></param>
        public T Add(T model)
        {
            model.GetType().GetProperty("State").SetValue(model, 1);
            //model.GetType().GetProperty("Id").SetValue(model, PrimaryKey.GetId());
            model.GetType().GetProperty("BuildTime").SetValue(model, DateTime.Now);
            model.GetType().GetProperty("UpdateTime").SetValue(model, DateTime.Now);
            return entity.Set<T>().Add(model);
        }
        #endregion

        /// <summary>
        /// 新增多个
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public IEnumerable<T> AddRange(IEnumerable<T> models)
        {
            return entity.Set<T>().AddRange(models);
        }

        #region 修改
        public int GetUpdate(Expression<Func<T, bool>> whereLambda, Expression<Func<T, T>> updateLambda)
        {
            return entity.Set<T>().Where(whereLambda).Update(updateLambda);
        }
        #endregion

        public int Update(T model)
        {
            this.entity.Entry<T>(model).State = System.Data.Entity.EntityState.Modified;

           return this.entity.SaveChanges(); 
        }

        public bool UpdateEntityList(IEnumerable<T> entityList)
        {
            foreach (var entity in entityList)
            {
                this.entity.Entry(entity).State = EntityState.Modified;
            }
            return true;
        }

        #region 删除
        /// <summary>
        ///    真删
        /// </summary>
        /// <param name="whereLambda"></param>
        public int DeleteReal(Expression<Func<T, bool>> whereLambda)
        {
            return entity.Set<T>().Where(whereLambda).Delete();
        }




        /// <summary>
        /// 假删（更新数据库state字段为0）
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="updateLambda"></param>
        /// <returns></returns>
        public int DeleteFake(Expression<Func<T, bool>> whereLambda, Expression<Func<T, T>> updateLambda)
        {
            return entity.Set<T>().Where(whereLambda).Update(updateLambda);
        }
        #endregion


        /// <summary>
        /// 创建线程内唯一对象
        /// </summary>
        /// <returns></returns>
        public static DbContext CreateEF()
        {
            DbContext dbContext = (DbContext)CallContext.GetData("ef");
            if (dbContext == null)
            {
                dbContext = new MyContext();
                CallContext.SetData("ef", dbContext);
            }
            return dbContext;
        }
    }
}
