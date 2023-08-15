using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Libraries;
using ShoppingCart.ViewModels;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class CheckOutController : Controller
    {
        private ShoppingCartDBEntities objShoppingCartDBEntities = new ShoppingCartDBEntities();
        private ViewModelLibrary vml = new ViewModelLibrary();
       
        //Get the view of order
        public ActionResult Checkout()
        {
            if (Session["UserId"] != null)
            {
                int intOrderId;
                if (Request["oid"] != null && int.TryParse(Request["oid"].ToString(), out intOrderId))
                {
                    OrderViewModel objOrderViewModel = vml.GetOrderByOrderId(intOrderId);
                    if (objOrderViewModel != null)
                        return View(objOrderViewModel);
                    else return RedirectToAction("OrderHistory", "Order");
                }
                else return RedirectToAction("OrderHistory", "Order");
            }
            else return RedirectToAction("Login", "Account");
        }

        public JsonResult Pay(OrderViewModel objOrderViewModel)
        {
            if (true)// (ModelState.IsValid)
            {
                Orders order = objShoppingCartDBEntities.Orders.First(model => model.OrderId == objOrderViewModel.OrderId);
                order.Address = objOrderViewModel.Address;
                order.City = objOrderViewModel.City;
                order.Province = objOrderViewModel.Province;
                order.PostalCode = objOrderViewModel.PostalCode;
                order.Paid = true;
                order.PaidDate = DateTime.Now;
                objShoppingCartDBEntities.SaveChanges();
                return Json(new { Success = true , OrderId= objOrderViewModel.OrderId.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Success = false, Message = "Some data is invalid." }, JsonRequestBehavior.AllowGet);
        }

        //Get the 
        public ActionResult Confirmation()
        {
            if  (Request["oid"]!=null)
                ViewBag.OrderId = Request["oid"].ToString();
            return View();
        }
    }
}