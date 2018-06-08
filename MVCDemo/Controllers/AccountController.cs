using MVCDemo.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCDemo.Controllers
{
    public class AccountController : Controller
    {
        private AccountContext db = new AccountContext();
        //
        // GET: /Account/
        public ActionResult Index()
        {
            return View(db.SysUsers);
        }

        public ActionResult Details(int ID)
        {
            SysUser sysUser = db.SysUsers.Find(ID);
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
                var user = db.SysUsers.Where(o => o.Email == email && o.Password == password).FirstOrDefault();

                ViewBag.LoginState = user.UserName + " 登录后。。。";
            }

            return View();
        }
        public ActionResult Register()
        {
            return View();
        }

    }
}