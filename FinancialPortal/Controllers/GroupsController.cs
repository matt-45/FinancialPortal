using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Helpers;
using FinancialPortal.Models;
using Microsoft.AspNet.Identity;

namespace FinancialPortal.Controllers
{
    public class GroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        

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

        // GET: Groups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
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
