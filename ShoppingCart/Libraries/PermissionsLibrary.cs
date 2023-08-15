using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingCart.ViewModels;
using ShoppingCart.Models;


namespace ShoppingCart.Libraries
{
    public class PermissionsLibrary
    {
        ShoppingCartDBEntities objShoppingCartDBEntities = new ShoppingCartDBEntities();
        //Check whether the user is Admin
        public bool IsAdmin(object strUserId)
        {
            if (strUserId != null)
            {
                
                    return true;    
            }
            else
                return false;
        }

        public UserViewModel GetUserByLogin(string strLogin, string strPawword)
        {

                UserViewModel objUserViewModel = (from objUser in objShoppingCartDBEntities.Users
                                                  where objUser.Login == strLogin && objUser.Password == strPawword
                                                  select new UserViewModel()
                                                  {
                                                      UserId = objUser.UserId,
                                                      Admin = objUser.Admin
                                                  }

                                                      ).FirstOrDefault();
                return objUserViewModel;

        }

        public string currentdirectory()
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            var uri = new Uri(path);
            return uri.GetLeftPart(UriPartial.Path);
        }

        public string rootdirectory()
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            var uri = new Uri(path);
            return uri.GetLeftPart(UriPartial.Authority);
        }       
    }    
}