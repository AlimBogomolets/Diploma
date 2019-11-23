using OnlineStore.Enum;
using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore.ViewModels
{
    public class GPUViewModel
    {
        public Product Product { get; set; }
        public GPU GPU { get; set; }
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Введите тип")]
        [Display(Name = "Тип")]
        public ProductType TypeID { get; set; } = ProductType.GPU;

        [Required(ErrorMessage = "Введите брэнд")]
        [Display(Name = "Брэнд")]
        public string Manufacturer { get; set; }

        [Required(ErrorMessage = "Введите название")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите цену")]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Введите VRAM")]
        [Display(Name = "Видеопамять")]
        public string VRAM { get; set; }

        [Display(Name = "Изображение")]
        public byte[] Image { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }
    }
}