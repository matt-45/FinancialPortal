using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FinancialPortal.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public partial class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [NotMapped]
        public string FullName
        {
            get 
            { 
                return $"{this.FirstName} {this.LastName}";
            }
        }

        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        public double IncomeAmount { get; set; }
        public IncomeType IncomeType { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        

        public ApplicationUser()
        {
            Transactions = new HashSet<Transaction>();
            Notifications = new HashSet<Notification>();
            BankAccounts = new HashSet<BankAccount>();
        }

        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<BudgetItem> BudgetItems { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
    }

    public enum IncomeType
    {
        Hourly,
        Daily,
        Weekly,
        BiWeekly,
        SemiMonthly,
        Monthly,
        Annually
    }
}