using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    [Table("Cart")]
    public class Cart
    {
        [Key]
        public int CartId {get;set;}
       
        public int ProductId { get; set; }

        public string UserId { get; set; }

        public int Quantity { get; set; }

        public bool isPurchased { get; set; }
    }
}