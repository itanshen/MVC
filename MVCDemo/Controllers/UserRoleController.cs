using MVCDemo.DAL;
using MVCDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCDemo.Models;
using System.Data.Entity;

namespace MVCDemo.Controllers
{
    public class UserRoleController : Controller
    {
        private AccountContext db = new AccountContext();
        //
        // GET: /UserRole/
        public ActionResult Index(int? ID)
        {
            var viewModel = new UserRoleIndexData();

            viewModel.SysUsers = db.SysUsers.Include(u => u.SysDepartment)
            .Include(u => u.SysUserRoles.Select(ur => ur.SysRole))
            .OrderBy(u => u.UserName);

            if (ID != null)
            {
                ViewBag.UserID = ID.Value;
                viewModel.SysUserRoles = viewModel.SysUsers.Where(u => u.ID == ID.Value).Single().SysUserRoles;
                viewModel.SysRoles = (viewModel.SysUserRoles.Where(ur => ur.SysUserID == ID.Value)).Select(ur => ur.SysRole);
            }
            return View(viewModel);

        }

    }
}