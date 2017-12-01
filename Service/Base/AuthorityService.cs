using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IService;
using Model;

namespace Service
{
    public class AuthorityService:BaseService<AuthorityModel>,IAuthorityService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.AuthorityDal;
        }
    }
}
