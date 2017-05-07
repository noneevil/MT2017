using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Mvc4Example.Models
{
    /// <summary>
    /// 实体框架容器类
    /// </summary>
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class dbContext : DbContext
    {
        static dbContext()
        {
            //实体生成相应数据库
            //Database.SetInitializer<dbContext>(new CreateDatabaseIfNotExists<dbContext>());
            //Database.SetInitializer<dbContext>(new DropCreateDatabaseIfModelChanges<dbContext>());
            //Database.SetInitializer<dbContext>(new DropCreateDatabaseAlways<dbContext>());
            //Database.SetInitializer<dbContext>(new dbContextInitializer());//自行实现数据库建立
        }

        public dbContext()
            : base("dbSQLite3")
        {
            //Disable initializer
            Database.SetInitializer<dbContext>(null);
#if DEBUG
            this.Database.Log = s => Debug.WriteLine(s);
#endif
        }
        /// <summary>
        /// 区县
        /// </summary>
        public DbSet<T_AreaEntities> Area { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public DbSet<T_CityEntities> City { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public DbSet<T_ProvinceEntities> Province { get; set; }
        /// <summary>
        /// 新闻
        /// </summary>
        public DbSet<T_NewsEntities> News { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public DbSet<T_GroupEntities> Group { get; set; }

        public DbSet<T_RadiosEntities> Radios { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}