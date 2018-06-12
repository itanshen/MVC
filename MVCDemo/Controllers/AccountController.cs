using MVCDemo.DAL;
using MVCDemo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCDemo.Repositories;
using PagedList;

namespace MVCDemo.Controllers
{
    public class AccountController : Controller
    {
        private AccountContext db = new AccountContext();
        IsysUserRepository iSysUserR = new SysUserRepository();
        //
        // GET: /Account/
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            //var users = iSysUserR.SelectAll();
            var users = db.SysUsers.Include(u => u.SysDepartment);
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.UserName.Contains(searchString) || u.Email.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(u => u.UserName);
                    break;
                default:
                    users = users.OrderBy(u => u.UserName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));

            //return View(iSysUserR.SelectAll());
        }

        public ActionResult Details(int ID)
        {
            SysUser sysUser = iSysUserR.SelectByID(ID);// db.SysUsers.Find(ID);
            return View(sysUser);
        }

        public ActionResult Login()
        {
            ViewBag.LoginState = "登录前。。。";
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            //获取表单数据
            string email = fc["txtEmail"];
            string password = fc["txtPassword"];
            if (db.SysUsers.Where(o => o.Email == email).Count() == 0)
            {
                ViewBag.LoginState = email + " 用户不存在。。。";
            }
            else if (db.SysUsers.Where(o => o.Email == email && o.Password == password).Count() == 0)
            {
                ViewBag.LoginState = email + " 用户密码错误。。。";
            }
            else
            {
                //var user = db.SysUsers.Where(o => o.Email == email && o.Password == password).FirstOrDefault();
                var user = db.SysUsers.FirstOrDefault(o => o.Email == email && o.Password == password);

                ViewBag.LoginState = user.UserName + " 登录后。。。";
            }

            return View();
        }
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 新建用户
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(SysUser sysUser)
        {
            //db.SysUsers.Add(sysUser);
            //db.SaveChanges();
            if (ModelState.IsValid)
            {
                iSysUserR.Add(sysUser);
                return RedirectToAction("Index");
            }
            return View();
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        public ActionResult Edit(int ID)
        {
            SysUser sysUser = iSysUserR.SelectByID(ID); //db.SysUsers.Find(ID);
            return View(sysUser);
        }
        [HttpPost]
        public ActionResult Edit(SysUser sysUser)
        {
            //db.Entry(sysUser).State = EntityState.Modified;
            //db.SaveChanges();
            iSysUserR.Edit(sysUser);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        public ActionResult Delete(int ID)
        {
            SysUser sysUser = iSysUserR.SelectByID(ID);// db.SysUsers.Find(ID);

            return View(sysUser);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int ID)
        {
            //SysUser sysUser = db.SysUsers.Find(ID);
            //db.SysUsers.Remove(sysUser);
            //db.SaveChanges();
            if (iSysUserR.Delete(ID))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}