using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    [Table("History")]
    public class History
    {
        [Key]
        public int HistoryId { get; set; }

        public int ProductId { get; set; }

        public string UserId { get; set; }

        public int Quantity { get; set; }

        public bool isPurchased { get; set; }

        public DateTime PurchaseDate { get; set; }
    }
}