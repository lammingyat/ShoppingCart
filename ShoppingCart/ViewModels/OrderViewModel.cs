using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public int UserID { get; set; }
        public decimal Total { get; set; }
        public bool Paid { get; set; }
        public string PaidDate { get; set; }
        public bool Valid { get; set; }
        public string InvalidDate { get; set; }
    }
}