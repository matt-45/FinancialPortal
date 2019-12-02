using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }

        public Group()
        {
            Transactions = new HashSet<Transaction>();
            Notifications = new HashSet<Notification>();
            Users = new HashSet<ApplicationUser>();
            Budgets = new HashSet<Budget>();
            Invitations = new HashSet<Invitation>();
        }

        

    }
}