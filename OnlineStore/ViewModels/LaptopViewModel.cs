using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore.ViewModels
{
    public class LaptopViewModel
    {
        public Product Product { get; set; }
        public Laptop Laptop { get; set; }
        [Display(Name = "Изображение")]
        public byte[] Image { get; set; }
    }
}