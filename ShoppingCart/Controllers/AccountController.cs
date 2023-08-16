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
        private ViewModelLibrary vml = new ViewModelLibrary();
        // GET: Account
        public ActionResult Register()
        {
            UserViewModel objUserViewModel = new UserViewModel();
            return View(objUserViewModel);
        }

        public JsonResult GoToRegister(UserViewModel objUserViewModelInput)
        {
            UserViewModel objUserViewModel = vml.GetUserByLogin(objUserViewModelInput.Login);
            //Account doesn't exist, then register
            if (objUserViewModel == null)                
            {
                //Insert new user
                Users user = new Users();
                user.Login = objUserViewModelInput.Login;
                user.Password = objUserViewModelInput.Password;
                user.Admin = false;
                user.Valid = true;
                objShoppingCartDBEntities.Users.Add(user);
                objShoppingCartDBEntities.SaveChanges();

                //update the reference of the new user
                Users userSave = objShoppingCartDBEntities.Users.First(x => x.UserId == user.UserId);
                userSave.CreateUserId = userSave.UserId;
                userSave.CreateDate = DateTime.Now;
                userSave.LastModifyUserId = userSave.UserId;
                userSave.LastModifyDate = DateTime.Now;
                objShoppingCartDBEntities.SaveChanges();

                Session["UserId"] = userSave.UserId.ToString();
                Session["Admin"] = userSave.Admin.ToString();
                Session["Login"] = userSave.Login;

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Success = false, Message = "The login name is already used." }, JsonRequestBehavior.AllowGet);
            }                
        }

        public ActionResult Login()
        {
            string strHashedPassword = SecurePasswordHasher.Hash("cantek");
            bool b = SecurePasswordHasher.Verify("cantek", strHashedPassword);


            UserViewModel objUserViewModel = new UserViewModel();
            return View(objUserViewModel);
        }

        public JsonResult GoToLogin(UserViewModel objUserViewModelInput)
        {            
            UserViewModel objUserViewModel = vml.GetUserByLoginAndPassword(objUserViewModelInput.Login, objUserViewModelInput.Password);
            //Login and password correct, then Login
            if (objUserViewModel != null)
            {
                if (objUserViewModel.Valid)
                {
                    Session["UserId"] = objUserViewModel.UserId.ToString();
                    Session["Admin"] = objUserViewModel.Admin.ToString();
                    Session["Login"] = objUserViewModel.Login;
                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { Success = false, Message = "This account is invalid." }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Success = false, Message = "Login or password is wrong." }, JsonRequestBehavior.AllowGet);
                
        }

        public ActionResult Logout()
        {
            Session["UserId"] = null;
            Session["Admin"] = null;
            return View();// RedirectToAction("Login", "Account");            
        }       

    }
}