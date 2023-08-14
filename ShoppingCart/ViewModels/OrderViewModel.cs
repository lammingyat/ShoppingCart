using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

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
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string FullAddress { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "The Card No must be 16 digits")]
        public string CardNo { get; set; }
        public string CardHolder { get; set; }
        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "The CVV must be 3 digits")]
        public string CVV { get; set; }

    }
}