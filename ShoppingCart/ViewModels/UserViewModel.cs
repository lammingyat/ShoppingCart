using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "The maximum digit is 20")]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public bool Admin { get; set; }
        public bool Valid { get; set; }
        public int CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public int LastModifyUserId { get; set; }

        public DateTime LastModifyDate { get; set; }

    }
}