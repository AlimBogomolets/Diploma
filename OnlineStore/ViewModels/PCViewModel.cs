using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore.ViewModels
{
    public class PCViewModel
    {
        public Product Product { get; set; }
        public PC PC { get; set; }
        [Display(Name = "Изображение")]
        public byte[] Image { get; set; }
    }
}