using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDal
{
    public interface IDbSession
    {
        DbContext entity { get; }
        IUserAdminDal UserDal { get; set; }
        IAuthorityAdminDal AuthorityDal { get; set; }
        IRoleAdminDal RoleDal { get; set; }
        IProductDal ProductDal { get; set; }
        int ExcuteSql(string sql, object[] parameters);
        bool SaveChanges();
    }
}
