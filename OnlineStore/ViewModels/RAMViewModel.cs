using OnlineStore.Enum;
using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore.ViewModels
{
    public class RAMViewModel
    {
        public Product Product { get; set; }
        public RAM RAM { get; set; }
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Введите тип")]
        [Display(Name = "Тип")]
        public ProductType TypeID { get; set; } = ProductType.RAM;

        [Required(ErrorMessage = "Введите брэнд")]
        [Display(Name = "Брэнд")]
        public string Manufacturer { get; set; }

        [Required(ErrorMessage = "Введите название")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите цену")]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Введите ёмкость")]
        [Display(Name = "Ёмкость")]
        public string Capacity { get; set; }

        [Required(ErrorMessage = "Введите скорость привода")]
        [Display(Name = "Скорость привода")]
        public string Speed { get; set; }

        [Display(Name = "Изображение")]
        public byte[] Image { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }
    }
}