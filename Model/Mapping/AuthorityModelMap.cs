using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AuthorityModelMap:EntityTypeConfiguration<AuthorityModel>
    {
        public AuthorityModelMap()
        {
            //配置主键
            this.HasKey(t => t.Id);

            //给ID配置自动增长
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //配置字段属性
            this.Property(t => t.Id).IsRequired();
            this.Property(t => t.BuildTime).IsRequired();
            this.Property(t => t.Description).IsRequired().HasMaxLength(500);
            this.Property(t => t.Name).IsRequired().HasMaxLength(500);
            this.Property(t => t.State).IsRequired();
            this.Property(t => t.Type).IsRequired();
            this.Property(t => t.UpdateTime).IsRequired();

            //关系配置 RoleModel
            this.HasMany(s => s.RoleModels).WithMany(s => s.AuthorityModels).Map(s => s.ToTable("RoleAuthority").MapLeftKey("AuthorityID").MapRightKey("RoleID"));
        }
    }
}
