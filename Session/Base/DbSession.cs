using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using IDal;

namespace Session
{
    public class DbSession : IDbSession
    {
        /// <summary>
        /// 创建线程内唯一上下文对象
        /// </summary>
        public DbContext entity
        {
            get
            {
                return DbContextOnly.CreateEF();
            }
        }
        ///范例
        //private IProductAdminDal _ProductDal;
        //public IProductAdminDal ProductDal
        //{
        //    get
        //    {
        //        if (_ProductDal == null)
        //        {
        //            _ProductDal = DalFactory.CreateProductDal();
        //        }
        //        return _ProductDal;
        //    }
        //    set { _ProductDal = value; }
        //}
        private IUserAdminDal _UserDal;
        public IUserAdminDal UserDal
        {
            get
            {
                if (_UserDal == null)
                {
                    _UserDal = DalFactory.CreateUserDal();
                }
                return _UserDal;
            }
            set { _UserDal = value; }
        }
        private IAuthorityAdminDal _AuthorityDal;
        public IAuthorityAdminDal AuthorityDal
        {
            get
            {
                if (_AuthorityDal == null)
                {
                    _AuthorityDal = DalFactory.CreateAuthorityDal();
                }
                return _AuthorityDal;
            }
            set { _AuthorityDal = value; }
        }

        public IRoleAdminDal _RoleDal;
        public IRoleAdminDal RoleDal
        {
            get
            {
                if(_RoleDal==null)
                {
                    _RoleDal = DalFactory.CreateRoleDal();
                }
                return _RoleDal;
            }
            set { _RoleDal = value; }
        }
        public IProductAdminDal _ProductDal;
        public IProductAdminDal ProductDal
        {
            get
            {
                if (_ProductDal == null)
                {
                    _ProductDal = DalFactory.CreateProductDal();
                }
                return _ProductDal;
            }
            set { _ProductDal = value; }
        }
        /// <summary>
        /// 执行Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExcuteSql(string sql, object[] parameters)
        {
            return this.entity.Database.ExecuteSqlCommand(sql, parameters);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            if (this.entity.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
