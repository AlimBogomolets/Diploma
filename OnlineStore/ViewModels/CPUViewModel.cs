using OnlineStore.Enum;
using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore.ViewModels
{
    public class CPUViewModel
    {
        public Product Product { get; set; }
        public CPU CPU { get; set; }

        [Key]
        public int ProductId { get; set; }

        [Display(Name = "Тип")]
        public ProductType TypeID { get; set; } = ProductType.CPU;

        [Display(Name = "Брэнд")]
        public string Manufacturer { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Display(Name = "Частота")]
        public string Frequency{ get; set; }

        [Display(Name = "Изображение")]
        public byte[] Image { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }
    }
}