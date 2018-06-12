using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCDemo.Models
{
    public class SysUser
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "登录名应在5到30个字符之间")]
        [Display(Name = "登录名")]
        [Column("LoginName")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }
        public virtual ICollection<SysUserRole> SysUserRoles { get; set; }
        public int? SysDepartmentID { get; set; }
        public virtual SysDepartment SysDepartment { get; set; }
    }
}