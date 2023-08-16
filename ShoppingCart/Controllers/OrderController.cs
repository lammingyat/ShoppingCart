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
    public class OrderController : Controller
    {
        private ShoppingCartDBEntities objShoppingCartDBEntities = new ShoppingCartDBEntities();
        ViewModelLibrary vml = new ViewModelLibrary();

        //Get the view of order
        public ActionResult OrderHistory()
        {
            if (Session["UserId"] != null)
            {
                return View(vml.GetOrderListByUserId(int.Parse(Session["UserId"].ToString())));
            }
            else return RedirectToAction("Login", "Account");
        }

        public ActionResult OrderDetail()
        {
            if (Session["UserId"] != null)
            {
                int intOrderId;
                if (Request["oid"] != null && int.TryParse(Request["oid"].ToString(), out intOrderId))
                {
                    OrderViewModel objOrderViewModel = vml.GetOrderByOrderId(intOrderId);
                    //prevent another user access
                    if (objOrderViewModel.UserID.ToString().Equals(Session["UserId"].ToString()))
                        return View(Tuple.Create(objOrderViewModel, vml.GetOrderDetailByOrderId(intOrderId)));
                    else return RedirectToAction("OrderHistory", "Order");
                }
                else return RedirectToAction("OrderHistory", "Order");
            }
            else return RedirectToAction("Login", "Account");
        }

        public JsonResult CancelOrder(string OrderId)
        {
            int intOrderId = int.Parse(OrderId);
            Orders order = objShoppingCartDBEntities.Orders.First(model => model.OrderId.ToString() == OrderId);
            order.Valid = false;
            order.InvalidDate = DateTime.Now;
            objShoppingCartDBEntities.SaveChanges();

            return Json(new { Success = true , Message="success to cancel the order(Id:" + OrderId + ")"}, JsonRequestBehavior.AllowGet);
        }
    }
}