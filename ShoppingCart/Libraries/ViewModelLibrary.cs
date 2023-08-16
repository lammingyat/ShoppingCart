using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models;
using ShoppingCart.ViewModels;
using System.Security.Cryptography;

namespace ShoppingCart.Libraries
{
    public class ViewModelLibrary
    {
        ShoppingCartDBEntities objShoppingCartDBEntities = new ShoppingCartDBEntities();

        //model for show the user
        public UserViewModel GetUserByLoginAndPassword(string strLogin, string strPawword)
        {
            UserViewModel objUserViewModel = (from objUser in objShoppingCartDBEntities.Users
                                              where objUser.Login == strLogin && objUser.Password == strPawword
                                              select new UserViewModel()
                                              {
                                                  UserId = objUser.UserId,
                                                  Valid = objUser.Valid,
                                                  Admin = objUser.Admin,
                                                  Login = objUser.Login,
                                                  Password = objUser.Password                                                  
                                              }

                                                  ).First();
            return objUserViewModel;
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


        public static class SecurePasswordHasher
        {
            /// <summary>
            /// Size of salt.
            /// </summary>
            private const int SaltSize = 16;

            /// <summary>
            /// Size of hash.
            /// </summary>
            private const int HashSize = 20;

            /// <summary>
            /// Creates a hash from a password.
            /// </summary>
            /// <param name="password">The password.</param>
            /// <param name="iterations">Number of iterations.</param>
            /// <returns>The hash.</returns>
            public static string Hash(string password, int iterations)
            {
                // Create salt
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

                // Create hash
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
                var hash = pbkdf2.GetBytes(HashSize);

                // Combine salt and hash
                var hashBytes = new byte[SaltSize + HashSize];
                Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

                // Convert to base64
                var base64Hash = Convert.ToBase64String(hashBytes);

                // Format hash with extra information
                return string.Format("$MYHASH$V1${0}${1}", iterations, base64Hash);
            }

            /// <summary>
            /// Creates a hash from a password with 10000 iterations
            /// </summary>
            /// <param name="password">The password.</param>
            /// <returns>The hash.</returns>
            public static string Hash(string password)
            {
                return Hash(password, 10000);
            }

            /// <summary>
            /// Checks if hash is supported.
            /// </summary>
            /// <param name="hashString">The hash.</param>
            /// <returns>Is supported?</returns>
            public static bool IsHashSupported(string hashString)
            {
                return hashString.Contains("$MYHASH$V1$");
            }

            /// <summary>
            /// Verifies a password against a hash.
            /// </summary>
            /// <param name="password">The password.</param>
            /// <param name="hashedPassword">The hash.</param>
            /// <returns>Could be verified?</returns>
            public static bool Verify(string password, string hashedPassword)
            {
                // Check hash
                if (!IsHashSupported(hashedPassword))
                {
                    throw new NotSupportedException("The hashtype is not supported");
                }

                // Extract iteration and Base64 string
                var splittedHashString = hashedPassword.Replace("$MYHASH$V1$", "").Split('$');
                var iterations = int.Parse(splittedHashString[0]);
                var base64Hash = splittedHashString[1];

                // Get hash bytes
                var hashBytes = Convert.FromBase64String(base64Hash);

                // Get salt
                var salt = new byte[SaltSize];
                Array.Copy(hashBytes, 0, salt, 0, SaltSize);

                // Create hash with given salt
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
                byte[] hash = pbkdf2.GetBytes(HashSize);

                // Get result
                for (var i = 0; i < HashSize; i++)
                {
                    if (hashBytes[i + SaltSize] != hash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

    }
   
}