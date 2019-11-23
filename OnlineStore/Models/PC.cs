using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    public class PC
    {
        [Key]
        public int ProductId { get; set; }

        public GPU GPU { get; set; } = new GPU();
        public CPU CPU { get; set; } = new CPU();
        public RAM RAM { get; set; } = new RAM();
        public SSD SSD { get; set; } = new SSD();
        public HDD HDD { get; set; } = new HDD();
    }
}