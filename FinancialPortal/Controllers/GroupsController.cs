using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using FinancialPortal.Helpers;
using FinancialPortal.Models;
using Microsoft.AspNet.Identity;

namespace FinancialPortal.Controllers
{
    public class GroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper roleHelper = new UserRoleHelper();
        

        [Authorize] // role head
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InviteUser(string userId, int groupId, string email)
        {
            var user = db.Users.Find(userId);

            Invitation invitation = new Invitation
            {
                Code = Guid.NewGuid(),
                GroupId = groupId,
                Created = DateTime.Now,
                TimeToLive = DateTime.Now.AddDays(1),
                IsValid = true,
                RecipientEmail = email
            };
            db.Invitations.Add(invitation);
            db.SaveChanges();

            var callbackUrl = Url.Action("RegisterUser", "Account", new { code = invitation.Code }, protocol: Request.Url.Scheme);
            var mailMessage = new MailMessage();
            mailMessage.To.Add(new MailAddress(email));
            mailMessage.From = new MailAddress("mattpark102@outlook.com");
            mailMessage.Subject = "Invite to financial portal.";
            mailMessage.Body = $"You got an invite from {user.FullName} to join group {user.Group.Name}. Click <a href=\"" + callbackUrl + "\">here</a>.";
            mailMessage.IsBodyHtml = true;
            ModelState.AddModelError("Message", "Message has been sent.");
            SendEmail.Send(mailMessage);

            return RedirectToAction("Index", "Home");
        }

        // GET: Groups
        public ActionResult Index()
        {
            return View(db.Groups.ToList());
        }

        // GET: Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string groupName)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            Group group = new Group
            {
                Name = groupName,
                Balance = 10000
            };

            db.Groups.Add(group);

            #region Budgets

            Budget food = new Budget
            {
                Name = "Food"
            };

            Budget utilities = new Budget
            {
                Name = "Utilities"
            };

            Budget entertainment = new Budget
            {
                Name = "Entertainment"
            };

            BudgetItem restaurant = new BudgetItem
            {
                Name = "Restaurant"
            };

            BudgetItem fastFood = new BudgetItem
            {
                Name = "Fast Food"
            };

            BudgetItem groceries = new BudgetItem
            {
                Name = "Groceries"
            };

            BudgetItem electricity = new BudgetItem
            {
                Name = "Electricity"
            };

            BudgetItem gas = new BudgetItem
            {
                Name = "Gas"
            };

            BudgetItem water = new BudgetItem
            {
                Name = "Water"
            };

            BudgetItem internet = new BudgetItem
            {
                Name = "Cable and Internet"
            };

            db.SaveChanges();

            food.BudgetItems.Add(groceries);
            food.BudgetItems.Add(fastFood);
            food.BudgetItems.Add(restaurant);

            utilities.BudgetItems.Add(gas);
            utilities.BudgetItems.Add(water);
            utilities.BudgetItems.Add(electricity);

            entertainment.BudgetItems.Add(internet);

            db.SaveChanges();

            group.Budgets.Add(food);
            group.Budgets.Add(utilities);
            group.Budgets.Add(entertainment);

            #endregion

            db.SaveChanges();

            user.GroupId = group.Id;

            db.SaveChanges();

            roleHelper.ChangeUserRoleTo(user.Id, "Head");

            return RedirectToAction("Index", "Home");
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LeaveMember(string userId)
        {
            var user = db.Users.Find(userId);
            user.GroupId = null;
            db.SaveChanges();
            return RedirectToAction("Login", "Account");
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
