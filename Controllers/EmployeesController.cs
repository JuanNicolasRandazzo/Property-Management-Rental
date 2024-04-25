using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASPN.Property_Rental_Management_Final_Project.Models;

namespace ASPN.Property_Rental_Management_Final_Project.Controllers
{
    public class EmployeesController : Controller
    {
        private PropertyRentalDBFourthVersionEntities db = new PropertyRentalDBFourthVersionEntities();

        // GET: Employees
        public ActionResult Index()
        {

            var propertyManagers = db.Employees.Include(e => e.User).Where(e => e.User.Role == "Property Manager");
            return View(propertyManagers.ToList());
            //var propertyManagers = db.Employees.Include(e => e.User).Where(e => e.User.Role == "Property Manager").ToList();
            //var tenants = db.Tenants.Include(t => t.User).Select(t => new Employee
            //{
            //    EmployeeID = t.TenantID,
            //    UserID = t.UserID,
            //    FirstName = t.FirstName,
            //    LastName = t.LastName,
            //    User = t.User
            //}).ToList();

            //var employees = propertyManagers.Cast<Employee>().Concat(tenants.Cast<Employee>()).ToList();

            //return View(employees);
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "EmployeeID,UserID,FirstName,LastName")] Employee employee)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Employees.Add(employee);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", employee.UserID);
        //    return View(employee);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                // Save new user with the password
                var newUser = new User
                {
                    Username = employee.FirstName + employee.LastName, // Username creation
                    Password = employee.Password,
                    Role = "Property Manager",
                };

                // Save User in the DB
                db.Users.Add(newUser);
                db.SaveChanges();

                // Assign UserID to the Tenant
                employee.UserID = newUser.UserID;

                // Save Tenant in the DB
                db.Employees.Add(employee);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // Si llegamos aquí, hay un problema con el modelo, devolvemos la vista con errores
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", employee.UserID);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,UserID,FirstName,LastName")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", employee.UserID);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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

        // GET: Employees/ManageTenants
        public ActionResult ManageTenants()
        {
            var tenants = db.Tenants.Include(t => t.User);
            return View(tenants.ToList());
        }
        // GET: Employees/ManagePropertyManagers
        public ActionResult ManagePropertyManager()
        {
            var propertyManagers = db.Employees.Include(e => e.User).Where(e => e.User.Role == "Property Manager");
            return View(propertyManagers.ToList());

        }

        // GET: Employees/CreateTenant
        public ActionResult CreateTenant()
        {
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username");
            return View();
        }

        // POST: Employees/CreateTenant
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTenant([Bind(Include = "TenantID,UserID,FirstName,LastName")] Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                db.Tenants.Add(tenant);
                db.SaveChanges();
                return RedirectToAction("ManageTenants");
            }

            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", tenant.UserID);
            return View(tenant);
        }

        // GET: Employees/EditTenant/5
        public ActionResult EditTenant(int? id)
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

        // POST: Employees/EditTenant/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTenant([Bind(Include = "TenantID,UserID,FirstName,LastName")] Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tenant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ManageTenants");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", tenant.UserID);
            return View(tenant);
        }

        // GET: Employees/DetailsTenant/5
        public ActionResult DetailsTenant(int? id)
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

        // GET: Employees/DeleteTenant/5
        public ActionResult DeleteTenant(int? id)
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

        // POST: Employees/DeleteTenant/5
        [HttpPost, ActionName("DeleteTenant")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTenantConfirmed(int id)
        {
            Tenant tenant = db.Tenants.Find(id);
            db.Tenants.Remove(tenant);
            db.SaveChanges();
            return RedirectToAction("ManageTenants");
        }
        public ActionResult PropertyManager()
        {

            var propertyManagers = db.Employees.Include(e => e.User).Where(e => e.User.Role == "Property Manager");
            return View(propertyManagers.ToList());

        }
    }
}
