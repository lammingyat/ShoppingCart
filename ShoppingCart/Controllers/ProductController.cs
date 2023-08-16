using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models;
using ShoppingCart.ViewModels;
using ShoppingCart.Libraries;

namespace ShoppingCart.Controllers
{
    public class ProductController : Controller
    {
        private ShoppingCartDBEntities objShoppingCartDBEntities;
        private List<ShoppingCartModel> listOfShoppingCartModels;
      
        public ProductController()
        {
            objShoppingCartDBEntities = new ShoppingCartDBEntities();
            listOfShoppingCartModels = new List<ShoppingCartModel>();            
        }
        // GET: Shopping
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                string strCid = Request.Params["cid"];
                ViewModelLibrary vml = new ViewModelLibrary();
                return View(Tuple.Create(vml.ProductList(strCid, true), vml.GetCategory(true)));
            }
            else return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public JsonResult AddToCart(string ProductId)
        {
            ShoppingCartModel objShoppingCartModel = new ShoppingCartModel();
            Products objProduct = objShoppingCartDBEntities.Products.Single(model => model.ProductId.ToString() == ProductId);

            //create cart if some Product is selected
            if (Session["CartCounter"] != null)
            {
                listOfShoppingCartModels = Session["CartProduct"] as List<ShoppingCartModel>;
            }

            //if the Product is already exist in the cart, accumulate the quantity
            if (listOfShoppingCartModels.Any(model => model.ProductId.ToString() == ProductId))
            {
                objShoppingCartModel = listOfShoppingCartModels.Single(model => model.ProductId.ToString() == ProductId);
                objShoppingCartModel.Quantity = Math.Min(objShoppingCartModel.Quantity + 1, 99);
                objShoppingCartModel.Total = objShoppingCartModel.Quantity * objShoppingCartModel.UnitPrice;
            }
            //if the Product does not exist in the cart, put in the cart
            else
            {
                Guid strProductId = Guid.Parse(ProductId);
                objShoppingCartModel.ProductId = strProductId;
                objShoppingCartModel.ImagePath = objProduct.ImagePath;
                objShoppingCartModel.ProductCode = objProduct.ProductCode;
                objShoppingCartModel.ProductName = objProduct.ProductName;
                objShoppingCartModel.Quantity = 1;
                objShoppingCartModel.Total = objProduct.ProductPrice;
                objShoppingCartModel.UnitPrice = objProduct.ProductPrice;
                listOfShoppingCartModels.Add(objShoppingCartModel);
            }

            Session["CartCounter"] = listOfShoppingCartModels.Count;
            Session["CartProduct"] = listOfShoppingCartModels;
            return Json(new { Success = true, Counter = listOfShoppingCartModels.Count }, JsonRequestBehavior.AllowGet);
        }      
    }
}