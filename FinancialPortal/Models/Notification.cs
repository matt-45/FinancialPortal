using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        public int GroupId { get; set; }
        public virtual Group? Group { get; set; }
    }
}