using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        [Required]
        [Display(Name = "Category Name")]
        [StringLength(30, ErrorMessage = "Category Name can't exceed 30 words")]
        public string CategoryName { get; set; }
        public bool Valid { get; set; }
        public int CreateUserID { get; set; }
        public int CreateDateTime { get; set; }
        public int LastModifyUserId { get; set; }
        public int LastModifyDateTime { get; set; }
    }
}