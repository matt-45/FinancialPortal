using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        public bool IsValid { get; set; }
        public DateTime Created { get; set; }
        public DateTime TimeToLive { get; set; }
        public string RecipientEmail { get; set; }
        public Guid Code { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}