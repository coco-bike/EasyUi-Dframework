using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dal
{
    public class DbContextOnly
    {
        public static DbContext CreateEF()
        {
            DbContext dbContext = (DbContext)CallContext.GetData("dbContext");
            if (dbContext == null)
            {
                dbContext = new MyContext();
                CallContext.SetData("dbContext", dbContext);
            }
            return dbContext;
        }
    }
}
