using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Model
{
    public class MyContext : DbContext
    {
        public MyContext()
            : base("name=ConStr")
        {
            //自动创建表，如果Entity有改到就更新到表结构
            Database.SetInitializer<MyContext>(new MigrateDatabaseToLatestVersion<MyContext, ReportingDbMigrationsConfiguration>());
        }

        public DbSet<UserModel> UserDalModals { get; set; }
        public DbSet<AuthorityModel> AuthorityModels { get; set; }
        public DbSet<RoleModel> RoleModels { get; set; }
        public DbSet<ProductModel> ProductModel { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); //生成数据库时表名为类名，不是上面带s的名字  //移除复数表名的契约
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();     //不创建EdmMetadata表  //防止黑幕交易 要不然每次都要访问 EdmMetadata这个表
            modelBuilder.Configurations.Add(new AuthorityModelMap());
            modelBuilder.Configurations.Add(new RoleModelMap());
            modelBuilder.Configurations.Add(new UserModelMap());
        }

        internal sealed class ReportingDbMigrationsConfiguration : System.Data.Entity.Migrations.DbMigrationsConfiguration<MyContext>
        {
            public ReportingDbMigrationsConfiguration()
            {
                AutomaticMigrationsEnabled = true;//任何Model Class的修改將會直接更新DB
                AutomaticMigrationDataLossAllowed = true;
            }
        }
    }
}
