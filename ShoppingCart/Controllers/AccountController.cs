using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models;
using ShoppingCart.ViewModels;
using ShoppingCart.Models;
using ShoppingCart.Libraries;


namespace ShoppingCart.Controllers
{   
    public class AccountController : Controller
    {
        private ShoppingCartDBEntities objShoppingCartDBEntities = new ShoppingCartDBEntities();
        private PermissionsLibrary pl = new PermissionsLibrary();
        // GET: Account
        public ActionResult Register()
        {
            UserViewModel objUserViewModel = new UserViewModel();
            return View(objUserViewModel);
        }

        public ActionResult Login()
        {
            UserViewModel objUserViewModel = new UserViewModel();
            return View(objUserViewModel);
        }

        public JsonResult GoToLogin(UserViewModel objUserViewModel2)
        {            
            UserViewModel objUserViewModel = pl.GetUserByLogin(objUserViewModel2.Login, objUserViewModel2.Password);
            if (objUserViewModel != null)
            {
                Session["UserId"] = objUserViewModel.UserId.ToString();
                Session["Admin"] = objUserViewModel.Admin.ToString();
            }
            else
            {
                Session["UserId"] = null;
                Session["Admin"] = null;
            }

            if (objUserViewModel!=null)
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Success = false, Message = "Login or password is wrong" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session["UserId"] = null;
            Session["Admin"] = null;
            return View();// RedirectToAction("Login", "Account");            
        }

    }
}