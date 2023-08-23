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
        private ShoppingCartDBEntities objShoppingCartDBEntities = new ShoppingCartDBEntities();
        private StringLibrary sl = new StringLibrary();

        //model for show the user
        public IEnumerable<UserViewModel> GetAllUser()
        {
           return (from objUser in objShoppingCartDBEntities.Users

             select new UserViewModel()
             {
                 UserId = objUser.UserId,
                 Login = objUser.Login,
                 Password = objUser.Password,
                 Valid = objUser.Valid,
                 Admin = objUser.Admin
             }
                       ).ToList();
        }

        public UserViewModel GetUserByLoginAndPassword(string strLogin, string strPawword)
        {
            UserViewModel objUserViewModel = (from objUser in objShoppingCartDBEntities.Users
                                              where objUser.Login == strLogin
                                              select new UserViewModel()
                                              {
                                                  UserId = objUser.UserId,
                                                  Valid = objUser.Valid,
                                                  Admin = objUser.Admin,
                                                  Login = objUser.Login,
                                                  Password = objUser.Password
                                              }

                                                  ).FirstOrDefault();

            if (objUserViewModel != null && sl.CheckHashStringMatch(strPawword, objUserViewModel.Password))
                return objUserViewModel;
            else
                return null;
        }

        public UserViewModel GetUserByLogin(string strLogin)
        {
            UserViewModel objUserViewModel = (from objUser in objShoppingCartDBEntities.Users
                                              where objUser.Login == strLogin
                                              select new UserViewModel()
                                              {
                                                  UserId = objUser.UserId,
                                                  Admin = objUser.Admin
                                              }

                                                  ).FirstOrDefault();
            return objUserViewModel;
        }

        public UserViewModel GetUserById(int intUid)
        {
            return (from objUser in objShoppingCartDBEntities.Users
                    where objUser.UserId == intUid
                    select new UserViewModel()
                    {
                        UserId = objUser.UserId,
                        Login = objUser.Login,
                        Password = objUser.Password,
                        Valid = objUser.Valid,
                        Admin = objUser.Admin
                    }
                    ).ToList().FirstOrDefault();
        }

        //model for show the category

        public IEnumerable<CategoryViewModel> GetCategory(bool blnOnlyValid)
        {
            return (from objCate in objShoppingCartDBEntities.Categories
                    where !blnOnlyValid || objCate.Valid == true
                    select new CategoryViewModel()
                    {
                        CategoryId = objCate.CategoryId,
                        CategoryName = objCate.CategoryName
                    }

                ).ToList();
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
                                            where (String.IsNullOrEmpty(strCid) || objProduct.CategoryId == intCid) && (!blnOnlyValid || objProduct.Valid == true)
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

        //the view of order
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
                        Address = objOrd.Address,
                        City = objOrd.City,
                        Province = objOrd.Province,
                        PostalCode = objOrd.PostalCode,
                        FullAddress = objOrd.Paid ? objOrd.Address + "," + objOrd.City + "," + objOrd.Province + "," + objOrd.PostalCode : String.Empty
                    }
             ).ToList();
        }

        public OrderViewModel GetLastPaidOrderByUserId(int intUserId)
        {
            return (from objOrd in objShoppingCartDBEntities.Orders
                           where objOrd.UserId == intUserId && objOrd.Paid
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
                               Address = objOrd.Address,
                               City = objOrd.City,
                               Province = objOrd.Province,
                               PostalCode = objOrd.PostalCode,
                               FullAddress = objOrd.Paid ? objOrd.Address + "," + objOrd.City + "," + objOrd.Province + "," + objOrd.PostalCode : String.Empty
                           }
             ).ToList().FirstOrDefault();
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