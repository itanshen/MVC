using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCDemo.Models;

namespace MVCDemo.Repositories
{
    public interface IsysUserRepository
    {
        /// <summary>
        /// 查询所有用户
        /// </summary>
        IQueryable<SysUser> SelectAll();

        /// <summary>
        /// 通过用户ID查询用户
        /// </summary>
        SysUser SelectByID(int ID);

        /// <summary>
        /// 通过用户名查询用户
        /// </summary>
        SysUser SelectByUserName(string userName);

        /// <summary>
        /// 添加用户
        /// </summary>
        void Add(SysUser sysUser);

        /// <summary>
        /// 修改用户
        /// </summary>
        void Edit(SysUser sysUser);

        /// <summary>
        /// 删除用户
        /// </summary>
        bool Delete(int ID);

    }
}
