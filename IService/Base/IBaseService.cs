using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IDal;

namespace IService
{
    public interface IBaseService<T> where T : class,new()
    {
        IDbSession dbSession { get; }
        IBaseDal<T> CurrentDal { get; set; }

        IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda);

        IQueryable<T> GetPagingList<S>(int pageIndex, int pageSize, out int totalCount, bool isDec, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderByLambda);

        T Add(T model);

        IEnumerable<T> AddRange(IEnumerable<T> models);

        int GetUpdate(Expression<Func<T, bool>> whereLambda, Expression<Func<T, T>> updateLambda);

        int Update(T model);

        bool UpdateEntityList(IEnumerable<T> entityList);

        int DeleteReal(Expression<Func<T, bool>> whereLambda);

        int DeleteFake(Expression<Func<T, bool>> whereLambda, Expression<Func<T, T>> updateLambda);
    }
}
