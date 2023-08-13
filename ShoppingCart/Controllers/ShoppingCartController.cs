using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models;
using ShoppingCart.ViewModels;
using ShoppingCart.Libraries;

namespace ShoppingCart.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        private ShoppingCartDBEntities objShoppingCartDBEntities = new ShoppingCartDBEntities();
        private List<ShoppingCartModel> listOfShoppingCartModels;
        public ActionResult Index()
        {
            listOfShoppingCartModels = Session["CartProduct"] as List<ShoppingCartModel>;
            return View(listOfShoppingCartModels);
        }

        //Remove the Product from the cart
        public JsonResult RemoveProduct(string ProductId)
        {
            if (Session["CartCounter"] != null)
            {
                listOfShoppingCartModels = Session["CartProduct"] as List<ShoppingCartModel>;
                //find out the Product and remove
                if (listOfShoppingCartModels.Any(model => model.ProductId.ToString() == ProductId))
                {
                    ShoppingCartModel objShoppingCartModel = new ShoppingCartModel();
                    objShoppingCartModel = listOfShoppingCartModels.Single(model => model.ProductId.ToString() == ProductId);
                    listOfShoppingCartModels.Remove(objShoppingCartModel);
                }

                Session["CartCounter"] = listOfShoppingCartModels.Count;
                Session["CartProduct"] = listOfShoppingCartModels;
                return Json(new { Success = true, Counter = listOfShoppingCartModels.Count }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { Success = false, Counter = 0 }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ChangeQuantity(string ProductId, int Num)
        {
            if (Session["CartCounter"] != null)
            {
                listOfShoppingCartModels = Session["CartProduct"] as List<ShoppingCartModel>;

                //find out the Product and change the quantity
                if (listOfShoppingCartModels.Any(model => model.ProductId.ToString() == ProductId))
                {
                    ShoppingCartModel objShoppingCartModel = new ShoppingCartModel();
                    objShoppingCartModel = listOfShoppingCartModels.Single(model => model.ProductId.ToString() == ProductId);
                    objShoppingCartModel.Quantity = Math.Max(Math.Min(objShoppingCartModel.Quantity + Num, 99), 1);
                    objShoppingCartModel.Total = objShoppingCartModel.Quantity * objShoppingCartModel.UnitPrice;
                }

                Session["CartCounter"] = listOfShoppingCartModels.Count;
                Session["CartProduct"] = listOfShoppingCartModels;

                return Json(new { Success = true, Counter = listOfShoppingCartModels.Count }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { Success = false, Counter = 0 }, JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public JsonResult CheckOut()
        {
            try
            {
                int OrderId = 0;
                listOfShoppingCartModels = Session["CartProduct"] as List<ShoppingCartModel>;

                if (listOfShoppingCartModels != null && listOfShoppingCartModels.Count > 0)
                {
                    //create order
                    Orders orderObj = new Orders()
                    {
                        OrderDate = DateTime.Now,
                        OrderNumber = String.Format("{0:yyyymmddHHmmsss}", DateTime.Now),
                        UserId = Int32.Parse(Session["UserId"].ToString()),
                        Paid = false,
                        Valid = true
                    };

                    objShoppingCartDBEntities.Orders.Add(orderObj);
                    objShoppingCartDBEntities.SaveChanges();
                    OrderId = orderObj.OrderId;

                    decimal total = 0;

                    //create order detail for each Product
                    foreach (var Product in listOfShoppingCartModels)
                    {
                        OrderDetails objOrderDetail = new OrderDetails();
                        objOrderDetail.OrderId = OrderId;
                        objOrderDetail.ProductId = Product.ProductId.ToString();
                        objOrderDetail.Quantity = Product.Quantity;
                        objOrderDetail.UnitPrice = Product.UnitPrice;
                        objOrderDetail.Total = Product.Total;
                        total += Product.Total;
                        objOrderDetail.Valid = true;
                        objShoppingCartDBEntities.OrderDetails.Add(objOrderDetail);                        
                    }

                    orderObj.Total = total;
                    objShoppingCartDBEntities.SaveChanges();

                    Session["CartProduct"] = null;
                    Session["CartCounter"] = null;
                    return Json(new { Success = true, Message ="",  Counter =0, OrderId= OrderId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Success = false, Message= "You must choose at least 1 Product" },  JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { Success = false, Message = "Unable to finish the order, please contact the developer." },  JsonRequestBehavior.AllowGet);
            }
        }
    }
}