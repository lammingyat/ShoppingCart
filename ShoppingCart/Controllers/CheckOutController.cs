using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Libraries;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Controllers
{
    public class CheckOutController : Controller
    {
        ViewModelLibrary vml = new ViewModelLibrary();
       
        //Get the view of order
        public ActionResult Index()
        {
            Session["UserId"] = "1";
            int intOrderId;
            if (Session["UserId"]!=null && Request["oid"] != null && int.TryParse(Request["oid"].ToString(), out intOrderId))
            {                          
                  OrderViewModel objOrderViewModel = vml.GetOrderByOrderId(intOrderId);
                    if (objOrderViewModel != null)
                        return View(objOrderViewModel);
                    else return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}