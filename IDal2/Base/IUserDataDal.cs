using Model.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDal
{
    public partial interface IUserDataDal : IBaseDal<UserDataModel>
    {
        bool CollectGood(long id, string nickName,bool type);
    }
}
