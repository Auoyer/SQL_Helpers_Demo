using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EF_MSSQL_CodeFirst.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UId { get; set; }

        [Required, MaxLength(10)]
        public string RoleName { get; set; }

        public UserInfo user { get; set; }
    }
}
