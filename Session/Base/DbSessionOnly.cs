using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using IDal;

namespace Session
{
    public class DbSessionOnly
    {
        public static IDbSession CreateDbSession()
        {
            DbSession dbSession = (DbSession)CallContext.GetData("dbsession");
            if (dbSession == null)
            {
                dbSession = new DbSession();
                CallContext.SetData("dbSession", dbSession);
            }
            return dbSession;
        }
    }
}
