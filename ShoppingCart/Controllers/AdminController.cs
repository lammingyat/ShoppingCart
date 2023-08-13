using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models;
using ShoppingCart.ViewModels;
using ShoppingCart.Libraries;

namespace ShoppingCart.Controllers
{
    public class AdminController : Controller
    {
        ShoppingCartDBEntities objShoppingCartDBEntities = new ShoppingCartDBEntities();
        ViewModelLibrary vml = new ViewModelLibrary();
        PermissionsLibrary pl = new PermissionsLibrary();

        public AdminController()
        {
           
        }

        //
        //The view for add new Product and choose Product to edit
        public ActionResult AddProduct()
        {
            Session["UserId"] = "1";
            string strCid = Request.Params["cid"];            

            //model for  adding new Product
            ProductViewModel objProductViewModel = new ProductViewModel();
            objProductViewModel.CategorySelectListItem = vml.CategorySelectList();
            objProductViewModel.Valid = true;          
           
            return View(Tuple.Create(objProductViewModel, vml.ProductList(strCid, false), vml.CategoryList()));
        }

        [HttpPost]
        public JsonResult AddProduct(ProductViewModel objProductViewModel)
        {
            if (ModelState.IsValid)
            {
                Products objProduct = new Products();
                if (objProductViewModel.ImagePath2 != null)
                {
                    string NewImage = Guid.NewGuid() + Path.GetExtension(objProductViewModel.ImagePath2.FileName);
                    objProductViewModel.ImagePath2.SaveAs(Server.MapPath("~/Images/" + NewImage));
                    objProduct.ImagePath = "~/Images/" + NewImage;
                }

                objProduct.CategoryId = objProductViewModel.CategoryId;
                objProduct.Description = objProductViewModel.Description;
                objProduct.ProductCode = objProductViewModel.ProductCode;
                objProduct.ProductId = Guid.NewGuid();
                objProduct.ProductName = objProductViewModel.ProductName;
                objProduct.ProductPrice = objProductViewModel.ProductPrice;
                objProduct.Valid = objProductViewModel.Valid;
                objProduct.CreateUserId = Int32.Parse(Session["UserId"].ToString());
                objProduct.CreateDate = DateTime.Now;
                objProduct.LastModifyUserId = Int32.Parse(Session["UserId"].ToString());
                objProduct.LastModifyDate = DateTime.Now;
                objShoppingCartDBEntities.Products.Add(objProduct);
                objShoppingCartDBEntities.SaveChanges();

                return Json(new { Success = true, Message = "Product(Code:" + objProduct.ProductCode + ") is added Successfully." }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Success = false, Message = "Some data is invalid." }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidProduct(Guid ProductId, string ProductCode, bool Valid)
        {
            Products objProduct = objShoppingCartDBEntities.Products.Where(x => x.ProductId == ProductId).FirstOrDefault();
            objProduct.Valid = Valid;
            objShoppingCartDBEntities.SaveChanges();
            return Json(new { Success = true, Message = "The valid of product(code:" + ProductCode + ") change to " + Valid.ToString() }, JsonRequestBehavior.AllowGet) ;
        }

        //The view for editing product
        public ActionResult EditProduct()
        {
            Session["UserId"] = "1";
            if (pl.IsAdmin(Session["UserId"]))
            {
                if (Request["pid"] != null)
                {
                    Guid strProductId;
                    if (Guid.TryParse(Request["pid"].ToString(), out strProductId))
                    {
                        ProductViewModel objProductViewModel = vml.GetProductById(strProductId);
                       
                        if (objProductViewModel != null)
                            return View(objProductViewModel);
                        else return RedirectToAction("Index", "Home");
                    }
                    else return RedirectToAction("Index", "Home");
                }
                else return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Index", "Home");
        }

        //function for saving product
        public JsonResult SaveProduct(ProductViewModel objProductViewModel)
        {
            if (ModelState.IsValid)
            {  
                Products Product = objShoppingCartDBEntities.Products.First(x => x.ProductId == objProductViewModel.ProductId);     

                if (objProductViewModel.ImagePath2 != null)
                {
                    //delete old image;
                    System.IO.File.Delete(Server.MapPath(Product.ImagePath));
                    //add new image
                    string NewImage = Guid.NewGuid() + Path.GetExtension(objProductViewModel.ImagePath2.FileName);
                    objProductViewModel.ImagePath2.SaveAs(Server.MapPath("~/Images/" + NewImage));
                    Product.ImagePath = "~/Images/" + NewImage;
                }

                Product.CategoryId = objProductViewModel.CategoryId;
                Product.Description = objProductViewModel.Description;
                Product.ProductCode = objProductViewModel.ProductCode;
                Product.ProductName = objProductViewModel.ProductName;
                Product.ProductPrice = objProductViewModel.ProductPrice;
                Product.Valid = objProductViewModel.Valid;
                Product.LastModifyUserId = Int32.Parse(Session["UserId"].ToString());
                Product.LastModifyDate = DateTime.Now;
                objShoppingCartDBEntities.SaveChanges();

                return Json(new { Success = true, Message = "Product(Code:" + Product.ProductCode + ") is edited Successfully." }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Success = false, Message = "Some data is invalid." }, JsonRequestBehavior.AllowGet);
        }

        //The view for adding new category
        public ActionResult AddCategory()
        {
            Session["UserId"] = "1";

            //mode for adding new category
            CategoryViewModel objCategoryViewModel = new CategoryViewModel();
            objCategoryViewModel.Valid = true;

            IEnumerable<CategoryViewModel> listOfCategoryViewModel;
            listOfCategoryViewModel = (from objCate in objShoppingCartDBEntities.Categories

                                       select new CategoryViewModel()
                                       {
                                           CategoryId = objCate.CategoryId,
                                           CategoryName = objCate.CategoryName,
                                           Valid = objCate.Valid
                                       }
               ).ToList();

            return View(Tuple.Create(objCategoryViewModel, listOfCategoryViewModel));
        }

        //function for adding new category

        [HttpPost]
        public JsonResult AddCategory(CategoryViewModel objCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                Categories objCategory = new Categories();
                objCategory.CategoryName = objCategoryViewModel.CategoryName;
                objCategory.Valid = objCategoryViewModel.Valid;
                objCategory.CreateUserId = Int32.Parse(Session["UserId"].ToString());
                objCategory.CreateDate = DateTime.Now;
                objCategory.LastModifyUserId = Int32.Parse(Session["UserId"].ToString());
                objCategory.LastModifyDate = DateTime.Now;
                objShoppingCartDBEntities.Categories.Add(objCategory);
                objShoppingCartDBEntities.SaveChanges();

                return Json(new { Success = true, Message = "Category(Name:" + objCategory.CategoryName + ") is added Successfully." }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Success = false, Message = "Some data is invalid." }, JsonRequestBehavior.AllowGet);
        }

        //The view for editing category
        public ActionResult EditCategory()
        {
            Session["UserId"] = "1";
            if (pl.IsAdmin(Session["UserId"]))
            {
                if (Request["cid"] != null)
                {
                    int intCid;
                    if (int.TryParse(Request["cid"].ToString(), out intCid))
                    {
                        CategoryViewModel objCategoryViewModel = vml.GetCategoryById(intCid);

                        if (objCategoryViewModel != null)
                            return View(objCategoryViewModel);
                        else return RedirectToAction("Index", "Home");
                    }
                    else return RedirectToAction("Index", "Home");

                }
                else return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Index", "Home");
        }

        //Function for saving the category
        public JsonResult SaveCategory(CategoryViewModel objCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                Categories Category = objShoppingCartDBEntities.Categories.First(x => x.CategoryId == objCategoryViewModel.CategoryId);
                Category.CategoryName = objCategoryViewModel.CategoryName;
                Category.Valid = objCategoryViewModel.Valid;
                Category.LastModifyUserId = Int32.Parse(Session["UserId"].ToString());
                Category.LastModifyDate = DateTime.Now;
                objShoppingCartDBEntities.SaveChanges();
                return Json(new { Success = true, Message = "Category(Name:" + Category.CategoryId + ") is edited Successfully." }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Success = false, Message = "Some data is invalid." }, JsonRequestBehavior.AllowGet);
        }
    }
}