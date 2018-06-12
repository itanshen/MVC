using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCDemo.DAL;
using MVCDemo.Models;

namespace MVCDemo.Repositories
{
    public class SysUserRepository : IsysUserRepository
    {
        protected AccountContext db = new AccountContext();

        /// <summary>
        /// 查询所有用户
        /// </summary>
        public IQueryable<SysUser> SelectAll()
        {
            IQueryable<SysUser> list = db.SysUsers;
            return list;
        }

        /// <summary>
        /// 通过用户ID查询用户
        /// </summary>
        public SysUser SelectByID(int ID)
        {
            SysUser sysUser = db.SysUsers.Find(ID);
            return sysUser;
        }

        /// <summary>
        /// 通过用户名查询用户
        /// </summary>
        public SysUser SelectByUserName(string userName)
        {
            SysUser sysUser = db.SysUsers.FirstOrDefault(o => o.UserName == userName);
            return sysUser;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        public void Add(SysUser sysUser)
        {
            db.SysUsers.Add(sysUser);
            db.SaveChanges();
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        public void Edit(SysUser sysUser)
        {
            db.Entry(sysUser).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        public bool Delete(int ID)
        {
            var delSysUser = db.SysUsers.Find(ID);
            if (delSysUser != null)
            {
                db.SysUsers.Remove(delSysUser);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}