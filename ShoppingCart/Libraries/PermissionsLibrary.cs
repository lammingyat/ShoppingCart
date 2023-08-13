using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ShoppingCart.Libraries
{
    public class PermissionsLibrary
    {
        //Check whether the user is Admin
        public bool IsAdmin(object strUserId)
        {
            if (strUserId != null)
            {
                if (strUserId.ToString() == "1")
                    return true;
                else
                    return false;
            }
            else
                return false;
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