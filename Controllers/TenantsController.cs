using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ASPN.Property_Rental_Management_Final_Project.Models;

namespace ASPN.Property_Rental_Management_Final_Project.Controllers
{
    public class TenantsController : Controller
    {
        private PropertyRentalDBFourthVersionEntities db = new PropertyRentalDBFourthVersionEntities();

        // GET: Appartments
        public ActionResult Index()
        {
            var apartments = db.Apartments.ToList(); 
            return View(apartments);
        }
       

        // GET: Tenants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // GET: Tenants/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username");
            return View();
        }

        // POST: Tenants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "TenantID,UserID,FirstName,LastName")] Tenant tenant)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Tenants.Add(tenant);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", tenant.UserID);
        //    return View(tenant);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                // Save new user with the password
                var newUser = new User
                {
                    Username = tenant.FirstName + tenant.LastName, // Username creation
                    Password = tenant.Password,
                    Role = "Tenant", 
                };

                // Save User in the DB
                db.Users.Add(newUser);
                db.SaveChanges();

                // Assign UserID to the Tenant
                tenant.UserID = newUser.UserID;

                // Save Tenant in the DB
                db.Tenants.Add(tenant);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            
            return View(tenant);
        }


        // GET: Tenants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", tenant.UserID);
            return View(tenant);
        }

        // POST: Tenants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TenantID,UserID,FirstName,LastName")] Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tenant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", tenant.UserID);
            return View(tenant);
        }

        // GET: Tenants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // POST: Tenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tenant tenant = db.Tenants.Find(id);
            db.Tenants.Remove(tenant);
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

        // GET: Tenants/SignUp
        public ActionResult SignUp()
        {
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username");
            return View();
        }

        // POST: Tenants/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                // Save new user with the password
                var newUser = new User
                {
                    Username = tenant.FirstName + tenant.LastName, // Username creation
                    Password = tenant.Password,
                    Role = "Tenant",
                };

                // Save User in the DB
                db.Users.Add(newUser);
                db.SaveChanges();

                // Assign UserID to the Tenant
                tenant.UserID = newUser.UserID;

                // Save Tenant in the DB
                db.Tenants.Add(tenant);
                db.SaveChanges();

                return RedirectToAction("LogIn", "LogIn"); 
            }


            return View(tenant);
        }
    }
}
