using MVCDemo.DAL;
using MVCDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCDemo.Models;
using System.Data.Entity;
using System.Net;

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

        public ActionResult Edit(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysUser user = db.SysUsers
                .Include(u => u.SysDepartment)
                .Include(u => u.SysUserRoles)
                .Where(u => u.ID == ID)
                .Single();

            //将用户所在的部门选出
            PopulateDepartmentsDropDownList(user.SysDepartmentID);

            //将某个用户下的所有角色取出
            PopulateAssignedRoleData(user);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        private void PopulateDepartmentsDropDownList(int? SysDepartmentID)
        {
            var allDepartments = db.SysDepartments.ToList();
            List<SelectListItem> departmentList = new List<SelectListItem>();
            departmentList.Add(new SelectListItem { Text = "请选择", Value = "", Selected = (SysDepartmentID == null ? true : false) });
            foreach (var item in allDepartments)
            {
                departmentList.Add(new SelectListItem { Text = item.DepartmentName.ToString(), Value = item.ID.ToString(), Selected = (item.ID == SysDepartmentID ? true : false) });
            }
            ViewData["SysDepartmentID"] = departmentList;
        }
        private void PopulateAssignedRoleData(SysUser user)
        {
            var allRoles = db.SysRoles.ToList();
            //获取用户关联角色的Id
            var userRoles = new HashSet<int>(user.SysUserRoles.Select(r => r.SysRoleID));
            var viewModel = new List<AssignedRoleData>();
            foreach (var role in allRoles)
            {
                viewModel.Add(new AssignedRoleData
                {
                    RoleID = role.ID,
                    RoleName = role.RoleName,
                    Assigned = userRoles.Contains(role.ID)
                });
            }
            ViewBag.Roles = viewModel;
        }

        [HttpPost]
        public ActionResult Edit(int? ID, string[] selectedRoles)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userToUpdate = db.SysUsers
                .Include(u => u.SysUserRoles)
                .Where(u => u.ID == ID)
                .Single();
            if (TryUpdateModel(userToUpdate, ""
                , new string[] { "LoginName", "Email", "Password", "CreateDate", "SysDepartmentID" }))
            {
                try
                {
                    UpdateUserRoles(selectedRoles, userToUpdate);

                    db.Entry(userToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    throw;
                }
            }
            //如果失败，重新绑定视图
            PopulateDepartmentsDropDownList(userToUpdate.SysDepartmentID);
            PopulateAssignedRoleData(userToUpdate);

            return View(userToUpdate);
        }
        private void UpdateUserRoles(string[] selectedRoles, SysUser userToUpdate)
        {
            using (AccountContext db2 = new AccountContext())
            {
                //没有选择，全部清空
                if (selectedRoles == null)
                {
                    var sysUserRoles = db2.SysUserRoles.Where(u => u.SysUserID == userToUpdate.ID).ToList();
                    foreach (var item in sysUserRoles)
                    {
                        db2.SysUserRoles.Remove(item);
                    }
                    db2.SaveChanges();
                    return;
                }

                //编辑后的角色
                var selectedRolesHS = new HashSet<string>(selectedRoles);
                //原来的角色
                var userRoles = new HashSet<int>(userToUpdate.SysUserRoles.Select(r => r.SysRoleID));
                foreach (var item in db.SysRoles)
                {
                    //如果被选中，原来没有的要添加
                    if (selectedRolesHS.Contains(item.ID.ToString()))
                    {
                        if (!userRoles.Contains(item.ID))
                        {
                            userToUpdate.SysUserRoles.Add(new SysUserRole { SysUserID = userToUpdate.ID, SysRoleID = item.ID });
                        }
                    }
                    else
                    {
                        //如果没被选中，原来有的要删除
                        if (userRoles.Contains(item.ID))
                        {
                            SysUserRole sysUserRole = db2.SysUserRoles.FirstOrDefault(ur => ur.SysRoleID == item.ID && ur.SysUserID == userToUpdate.ID);
                            db2.SysUserRoles.Remove(sysUserRole);
                            db2.SaveChanges();
                        }
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}