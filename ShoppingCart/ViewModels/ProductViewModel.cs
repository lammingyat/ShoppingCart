using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.ViewModels
{
    public class ProductViewModel
    {
        public Guid ProductId { get; set; }
        public int CategoryId { get; set; }
        [Required]
        [Display(Name ="Product Code")]
        [StringLength(20,ErrorMessage ="Product Code can't exceed 20 words")]
        public string ProductCode { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(0,99999)]
        public decimal ProductPrice { get; set; }
        public HttpPostedFileBase ImagePath2 { get; set; }
        public string ImagePath { get; set; }
        public bool Valid { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
        public int LastModifyUserId { get; set; }
        public DateTime LastModifyDate { get; set; }
        public IEnumerable<SelectListItem> CategorySelectListItem { get; set; }
    }
}