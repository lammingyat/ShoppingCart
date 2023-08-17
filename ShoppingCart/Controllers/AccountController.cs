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
        private StringLibrary sl = new StringLibrary();
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
            Session["CartProduct"] = null;
            Session["CartCounter"] = null;
            return View();// RedirectToAction("Login", "Account");            
        } 

        public ActionResult AddUser()
        {
            if (Session["UserId"] != null)
            {
                if (Session["Admin"].ToString().Equals("True"))
                {
                    //mode for adding new user
                    UserViewModel objUserViewModel = new UserViewModel();
                    objUserViewModel.Valid = true;
                    objUserViewModel.Admin = false;                    

                    return View(Tuple.Create(objUserViewModel, vml.GetAllUser()));
                }
                else return RedirectToAction("Index", "Product");
            }
            else return RedirectToAction("Login", "Account");

        }

        //function for adding new category
        [HttpPost]
        public JsonResult AddUser(UserViewModel objUserViewModel)
        {
            if (ModelState.IsValid)
            {
                Users objUser = new Users();
                objUser.Login = objUserViewModel.Login;
                objUser.Password = sl.GetHashString(objUserViewModel.Password);
                objUser.Valid = objUserViewModel.Valid;
                objUser.Admin = objUserViewModel.Admin;
                objUser.CreateUserId = Int32.Parse(Session["UserId"].ToString());
                objUser.CreateDate = DateTime.Now;
                objUser.LastModifyUserId = Int32.Parse(Session["UserId"].ToString());
                objUser.LastModifyDate = DateTime.Now;
                objShoppingCartDBEntities.Users.Add(objUser);
                objShoppingCartDBEntities.SaveChanges();

                return Json(new { Success = true, Message = "User(Login:" + objUser.Login + ") is added Successfully." }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Success = false, Message = "Some data is invalid." }, JsonRequestBehavior.AllowGet);
        }

        //The view for editing user
        public ActionResult EditUser()
        {
            if (Session["UserId"] != null)
            {
                if (Session["Admin"].ToString().Equals("True"))
                {
                    int intUid;
                    if (Request["uid"] != null && int.TryParse(Request["uid"].ToString(), out intUid))
                    {
                        UserViewModel objUserViewModel = vml.GetUserById(intUid);

                        if (objUserViewModel != null)
                            return View(objUserViewModel);
                        else return RedirectToAction("AddUser", "Account");
                    }
                    else return RedirectToAction("AddUser", "Account");
                }
                else return RedirectToAction("Index", "Product");
            }
            else return RedirectToAction("Login", "Account");
        }

        //Function for saving the user
        public JsonResult SaveUser(UserViewModel objUserViewModel)
        {
            if (ModelState.IsValid)
            {
                Users User = objShoppingCartDBEntities.Users.FirstOrDefault(x => x.UserId == objUserViewModel.UserId);
                User.Login = objUserViewModel.Login;
                if (sl.IsHashStyle(objUserViewModel.Password))
                    User.Password = objUserViewModel.Password;
                else
                    User.Password = sl.GetHashString(objUserViewModel.Password);

                User.Valid = objUserViewModel.Valid;
                User.LastModifyUserId = Int32.Parse(Session["UserId"].ToString());
                User.LastModifyDate = DateTime.Now;
                objShoppingCartDBEntities.SaveChanges();
                return Json(new { Success = true, Message = "User(Id:" + User.UserId + ") is edited Successfully." }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Success = false, Message = "Some data is invalid." }, JsonRequestBehavior.AllowGet);
        }

        //The view for editing password
        public ActionResult EditPassword()
        {
            if (Session["UserId"] != null && Request.Params["uid"]!=null && Session["UserId"].ToString().Equals(Request.Params["uid"].ToString()))
            {
                int intUid;
                if (int.TryParse(Request["uid"].ToString(), out intUid))
                {
                    UserViewModel objUserViewModel = vml.GetUserById(intUid);

                    if (objUserViewModel != null)
                        return View(objUserViewModel);
                    else return RedirectToAction("Index", "Product");
                }
                else return RedirectToAction("Index", "Product");              
            }
            else return RedirectToAction("Index", "Product");
        }


        //Function for saving the password
        public JsonResult SavePassword(UserViewModel objUserViewModel)
        {
            if (true)// (ModelState.IsValid)
            {
                Users User = objShoppingCartDBEntities.Users.First(x => x.UserId == objUserViewModel.UserId);
                   if (sl.IsHashStyle(objUserViewModel.Password))
                    User.Password = objUserViewModel.Password;
                else
                    User.Password = sl.GetHashString(objUserViewModel.Password);
                User.LastModifyUserId = Int32.Parse(Session["UserId"].ToString());
                User.LastModifyDate = DateTime.Now;
                objShoppingCartDBEntities.SaveChanges();
                return Json(new { Success = true, Message = "The password of user(Id:" + User.UserId + ") is edited Successfully." }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Success = false, Message = "Some data is invalid." }, JsonRequestBehavior.AllowGet);
        }

    }
}