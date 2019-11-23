using OnlineStore.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Введите тип")]
        [Display(Name = "Тип")]
        public ProductType TypeID { get; set; }

        [Required(ErrorMessage = "Введите брэнд")]
        [Display(Name = "Брэнд")]
        public string Manufacturer { get; set; }

        [Required(ErrorMessage = "Введите ")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите цену")]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Display(Name = "Изображение")]
        public byte[] Image { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }
    }
}