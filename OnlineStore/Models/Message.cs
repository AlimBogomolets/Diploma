using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    public class Message
    {
        public string Subject { get; set; }
        public string MailTo { get; set; }
        public string MailContent { get; set; }
    }
}