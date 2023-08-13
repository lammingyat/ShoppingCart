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
            Session["UserId"] = "1";
            if (Session["UserId"] != null)
            {
                return View(vml.GetOrderListByUserId(int.Parse(Session["UserId"].ToString())));
            }
            else return RedirectToAction("Index", "Home");
        }

        public ActionResult OrderDetail()
        {
            Session["UserId"] = "1";
            int intOrderId;
            if (Session["UserId"] != null && Request["oid"]!=null && int.TryParse(Request["oid"].ToString(), out intOrderId))
            {
                return View(Tuple.Create(vml.GetOrderByOrderId(intOrderId), vml.GetOrderDetailByOrderId(intOrderId)));
            }
            else return RedirectToAction("Index", "Home");

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