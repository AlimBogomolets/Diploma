using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore.ViewModels
{
    public class HistoryViewModel
    {
        public int HistoryID { get; set; }

        [Display(Name = "Аккаунт")]
        public string UserName { get; set; }
        
        public Product product { get; set; }

        [Display(Name = "Количество")]
        public int Quantity { get; set; }

        [Display(Name = "Дата приобретения")]
        public DateTime PurchaseDate { get; set; }
    }
}