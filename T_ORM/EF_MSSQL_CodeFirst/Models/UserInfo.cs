using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EF_MSSQL_CodeFirst.Models
{
    public class UserInfo
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(30)]
        public string Name { get; set; }

        public int Age { get; set; }

        public string Like { get; set; }

        public List<Role> Roles { get; set; }
    }
}
