﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EF_MSSQL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class People_EF_DBFirstEntities : DbContext
    {
        public People_EF_DBFirstEntities()
            : base("name=People_EF_DBFirstEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TB_Employee> TB_Employee { get; set; }
        public virtual DbSet<TB_ER> TB_ER { get; set; }
        public virtual DbSet<TB_Role> TB_Role { get; set; }
    
        public virtual ObjectResult<Proc_GetEmployees_Result> Proc_GetEmployees()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Proc_GetEmployees_Result>("Proc_GetEmployees");
        }
    }
}
