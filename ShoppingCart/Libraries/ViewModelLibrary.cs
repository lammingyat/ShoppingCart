using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Libraries
{
    public class ViewModelLibrary
    {
        ShoppingCartDBEntities objShoppingCartDBEntities = new ShoppingCartDBEntities();

        //model for show the category
        public IEnumerable<CategoryViewModel> CategoryList()
        {         
            return (from objCate in objShoppingCartDBEntities.Categories
                    where objCate.Valid == true
                    select new CategoryViewModel()
                    {
                        CategoryId = objCate.CategoryId,
                        CategoryName = objCate.CategoryName
                    }

                ).ToList();

            //useless code
            //listOfShoppingViewModels = listOfShoppingViewModels.Where(model => model.CategoryId == Int32.Parse(strCid));
            /*ProductViewModel objProductViewModel = new ProductViewModel();
            objProductViewModel.CategorySelectListProduct = (from objCat in objShoppingCartDBEntities.Categories
                                                       select new SelectListProduct()
                                                       {
                                                           Text = objCat.CategoryName,
                                                           Value = objCat.CategoryId.ToString(),
                                                           Selected = true
                                                       });  */
        }

        //model for choose Product to buy/edit
       
        public IEnumerable<ShoppingViewModel> ProductList(string strCid, bool blnOnlyValid)
        {
            IEnumerable<ShoppingViewModel> listOfShoppingViewModels;
            if (String.IsNullOrEmpty(strCid))
            {
                listOfShoppingViewModels = (from objProduct in objShoppingCartDBEntities.Products
                                            join
                                                objCate in objShoppingCartDBEntities.Categories
                                                on objProduct.CategoryId equals objCate.CategoryId
                                            where !blnOnlyValid || objProduct.Valid == true
                                            select new ShoppingViewModel()
                                            {
                                                ImagePath = String.IsNullOrEmpty(objProduct.ImagePath) ? "~/Images/NoImage.jpg" : objProduct.ImagePath,
                                                ProductName = objProduct.ProductName,
                                                Description = String.IsNullOrEmpty(objProduct.Description) ? "" : objProduct.Description,
                                                ProductPrice = objProduct.ProductPrice,
                                                ProductId = objProduct.ProductId,
                                                CategoryId = objProduct.CategoryId,
                                                CategoryName = objCate.CategoryName,
                                                ProductCode = objProduct.ProductCode,
                                                Valid = objProduct.Valid
                                            }
               ).ToList();
            }
            else
            {
                int intCid = Int32.Parse(strCid);
                listOfShoppingViewModels = (from objProduct in objShoppingCartDBEntities.Products
                                            join
                                                objCate in objShoppingCartDBEntities.Categories
                                                on objProduct.CategoryId equals objCate.CategoryId
                                            where objProduct.CategoryId == intCid
                                            select new ShoppingViewModel()
                                            {
                                                ImagePath = String.IsNullOrEmpty(objProduct.ImagePath) ? "~/Images/NoImage.jpg" : objProduct.ImagePath,
                                                ProductName = objProduct.ProductName,
                                                Description = String.IsNullOrEmpty(objProduct.Description) ? "" : objProduct.Description,
                                                ProductPrice = objProduct.ProductPrice,
                                                ProductId = objProduct.ProductId,
                                                CategoryId = objProduct.CategoryId,
                                                CategoryName = objCate.CategoryName,
                                                ProductCode = objProduct.ProductCode
                                            }

               ).ToList();
            }

            return listOfShoppingViewModels;
        }

        public IEnumerable<SelectListItem> CategorySelectList()
        {
            return (from objCat in objShoppingCartDBEntities.Categories
                    select new SelectListItem()
                    {
                        Text = objCat.CategoryName,
                        Value = objCat.CategoryId.ToString(),
                        Selected = true
                    });
        }

        public ProductViewModel GetProductById(Guid strProductId)
        {
            //model for edit Product
            ProductViewModel objProductViewModel = new ProductViewModel();
            objProductViewModel = (from objProduct in objShoppingCartDBEntities.Products
                                join
                                    objCate in objShoppingCartDBEntities.Categories
                                    on objProduct.CategoryId equals objCate.CategoryId
                                where objProduct.ProductId == strProductId
                                select new ProductViewModel()
                                {
                                    CategoryId = objProduct.CategoryId,
                                    ProductId = objProduct.ProductId,
                                    ProductCode = objProduct.ProductCode,
                                    ProductName = objProduct.ProductName,
                                    Description = String.IsNullOrEmpty(objProduct.Description) ? "" : objProduct.Description,
                                    ProductPrice = objProduct.ProductPrice,
                                    ImagePath = String.IsNullOrEmpty(objProduct.ImagePath) ? "~/Images/NoImage.jpg" : objProduct.ImagePath,
                                    Valid = objProduct.Valid
                                }
                 ).ToList().FirstOrDefault();

            if (objProductViewModel != null)
                objProductViewModel.CategorySelectListItem = CategorySelectList();

            return objProductViewModel;
        }
        public CategoryViewModel GetCategoryById(int intCid)
        {
            return (from objCate in objShoppingCartDBEntities.Categories
                    where objCate.CategoryId == intCid
                    select new CategoryViewModel()
                    {
                        CategoryId = objCate.CategoryId,
                        CategoryName = objCate.CategoryName,
                        Valid = objCate.Valid
                    }
                    ).ToList().FirstOrDefault();
        }

        public IEnumerable<OrderViewModel> GetOrderListByUserId(int intUserId)
        {
            return (from objOrd in objShoppingCartDBEntities.Orders
                    where objOrd.UserId == intUserId
                    orderby objOrd.OrderDate descending
                    select new OrderViewModel()
                    {
                        OrderId = objOrd.OrderId,
                        OrderDate = objOrd.OrderDate,
                        OrderNumber = objOrd.OrderNumber,                       
                        UserID = objOrd.UserId,
                        Total = objOrd.Total,
                        Paid = objOrd.Paid,
                        PaidDate = (!string.IsNullOrEmpty(objOrd.PaidDate.ToString())) ? objOrd.PaidDate.ToString() : "-",
                        Valid = objOrd.Valid,
                        InvalidDate = (!string.IsNullOrEmpty(objOrd.InvalidDate.ToString())) ? objOrd.InvalidDate.ToString() : "-",
                        FullAddress = objOrd.Paid ? objOrd.Address + "," + objOrd.City + "," + objOrd.Province + "," + objOrd.PostalCode : String.Empty
                    }
             ).ToList();
        }

        public OrderViewModel GetOrderByOrderId(int intOrderId)
        {
            return (from objOrd in objShoppingCartDBEntities.Orders
                    where objOrd.OrderId == intOrderId
                    select new OrderViewModel()
                    {
                        OrderId = objOrd.OrderId,
                        OrderDate = objOrd.OrderDate,
                        OrderNumber = objOrd.OrderNumber,
                        UserID = objOrd.UserId,
                        Total = objOrd.Total,
                        Paid = objOrd.Paid,
                        PaidDate = (!string.IsNullOrEmpty(objOrd.PaidDate.ToString())) ? objOrd.PaidDate.ToString() : "-",
                        Valid = objOrd.Valid,
                        InvalidDate = (!string.IsNullOrEmpty(objOrd.InvalidDate.ToString())) ? objOrd.InvalidDate.ToString() : "-",
                        FullAddress = objOrd.Paid ? objOrd.Address +"," + objOrd.City + "," +objOrd.Province + "," +objOrd.PostalCode:String.Empty
                    }
               ).ToList().FirstOrDefault();
        }

        public IEnumerable<OrderDetailsViewsModel> GetOrderDetailByOrderId(int intOrderId)
        {
            return (from objOrdDet in objShoppingCartDBEntities.OrderDetails 
                    join objProduct in objShoppingCartDBEntities.Products on objOrdDet.ProductId equals objProduct.ProductId.ToString()
                    where objOrdDet.OrderId == intOrderId && objOrdDet.Valid==true
                    select new OrderDetailsViewsModel()
                    {
                        OrderDetailId = objOrdDet.OrderDetailId,
                        OrderId = objOrdDet.OrderId,
                        ImagePath = objProduct.ImagePath,
                        ProductId = objOrdDet.ProductId,
                        ProductCode = objProduct.ProductCode,
                        ProductName = objProduct.ProductName,
                        Quantity = objOrdDet.Quantity,
                        UnitPrice = objOrdDet.UnitPrice,
                        Total = objOrdDet.Total,
                        Valid = objOrdDet.Valid,
                        InvalidDate = (!string.IsNullOrEmpty(objOrdDet.InvalidDate.ToString())) ? objOrdDet.InvalidDate.ToString() : "-",
                    }
               ).ToList();
        }

       
    }
   
}