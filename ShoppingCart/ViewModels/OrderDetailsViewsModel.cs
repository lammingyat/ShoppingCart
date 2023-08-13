using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.ViewModels
{
    public class OrderDetailsViewsModel
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public string ImagePath { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }        
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
        public bool Valid { get; set; }
        public string InvalidDate { get; set; }
    }
}