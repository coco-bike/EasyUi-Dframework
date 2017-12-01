using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Configuration;
using IDal;

namespace Session
{
        /// <summary>
        ///抽象工厂
        /// </summary>
        public class DalFactory
        {
            private static IContainer _container;
            private static IContainer container
            {
                get
                {
                    if (_container == null)
                    {
                        var builder = new ContainerBuilder();
                        builder.RegisterModule(new ConfigurationSettingsReader("autofac"));
                        _container = builder.Build();
                    }
                    return _container;
                }
            }
            /// <summary>
            /// 范例
            /// </summary>
            /// <returns></returns>
            //public static IProductAdminDal CreateProductDal()
            //{
            //    using (var scope=container.BeginLifetimeScope())
            //    {
            //        return scope.Resolve<IProductAdminDal>();
            //    }
            //}

            public static IAuthorityAdminDal CreateAuthorityDal()
            {
                using(var scope=container.BeginLifetimeScope())
                {
                    return scope.Resolve<IAuthorityAdminDal>();
                }
            }

            public static IUserAdminDal CreateUserDal()
            {
                using (var scope=container.BeginLifetimeScope())
                {
                    return scope.Resolve<IUserAdminDal>();
                }
            }
            public static IRoleAdminDal CreateRoleDal()
            {
                using (var scope = container.BeginLifetimeScope())
                {
                    return scope.Resolve<IRoleAdminDal>();
                }
            }
            public static IProductAdminDal CreateProductDal()
            {
                using (var scope = container.BeginLifetimeScope())
                {
                    return scope.Resolve<IProductAdminDal>();
                }
            }
        }
    }

