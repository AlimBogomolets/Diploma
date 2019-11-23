using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore.ViewModels
{
    public class CartViewModel
    {
        public int CartID { get; set; }
        public Product product { get; set; }
        [Display(Name = "Количество")]
        public int Quantity { get; set; }
    }
}