﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MVCDemo.Models;

namespace MVCDemo.DAL
{
    public class AccountInitializer : DropCreateDatabaseIfModelChanges<AccountContext>
    {
        protected override void Seed(AccountContext context)
        {
            var sysUsers = new List<SysUser> { 
            new SysUser{UserName="Tom",Email="Tom@sohu.com",Password="1"},
            new SysUser{UserName="Jerry",Email="Jerry@sohu.com",Password="1"}
            };
            sysUsers.ForEach(s => context.SysUsers.Add(s));
            context.SaveChanges();

            var sysRoles = new List<SysRole> { 
            new SysRole{RoleName="Administrators",RoleDesc="Administrators have full authorization to perform system administration."},
            new SysRole{RoleName="General Users",RoleDesc="General Users can access the shared data they are authorized for."}
            };
            sysRoles.ForEach(s => context.SysRoles.Add(s));
            context.SaveChanges();

        }
    }
}