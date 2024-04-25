using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASPN.Property_Rental_Management_Final_Project.Models;

namespace ASPN.Property_Rental_Management_Final_Project.Controllers
{
    public class ApartmentsController : Controller
    {
        private PropertyRentalDBFourthVersionEntities db = new PropertyRentalDBFourthVersionEntities();

        // GET: Apartments
        public ActionResult Index()
        {
            var apartments = db.Apartments.Include(a => a.Building).Include(a => a.Status);
            return View(apartments.ToList());
        }

        // GET: Apartments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }

        // GET: Apartments/Create
        public ActionResult Create()
        {
            ViewBag.BuildingID = new SelectList(db.Buildings, "BuildingID", "BuildingName");
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName");
            return View();
        }

        // POST: Apartments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ApartmentID,BuildingID,ApartmentNumber,StatusID,Photo")] Apartment apartment, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null && photo.ContentLength > 0)
                {
                    //Stores the img in the file
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    photo.SaveAs(path);
                    //Stores the img in the DB
                    string base64String = Convert.ToBase64String(ReadFully(photo.InputStream));
                    apartment.Photo = base64String;
                }

                db.Apartments.Add(apartment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BuildingID = new SelectList(db.Buildings, "BuildingID", "BuildingName", apartment.BuildingID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", apartment.StatusID);
            return View(apartment);
        }

        // GET: Apartments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            ViewBag.BuildingID = new SelectList(db.Buildings, "BuildingID", "BuildingName", apartment.BuildingID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", apartment.StatusID);
            return View(apartment);
        }

        // POST: Apartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApartmentID,BuildingID,ApartmentNumber,StatusID,Photo")] Apartment apartment, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null && photo.ContentLength > 0)
                {
                    //Stores the img in the file
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    photo.SaveAs(path);
                    string base64String = Convert.ToBase64String(ReadFully(photo.InputStream));
                    apartment.Photo = base64String;
                }

                db.Entry(apartment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BuildingID = new SelectList(db.Buildings, "BuildingID", "BuildingName", apartment.BuildingID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", apartment.StatusID);
            return View(apartment);
        }
        private byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        // GET: Apartments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Apartment apartment = db.Apartments.Find(id);
            db.Apartments.Remove(apartment);
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
       /* public ActionResult ListApartments(int? buildingId)
        {
            var apartments = db.Apartments.Include(a => a.Building).Include(a => a.Status).ToList();

            if (buildingId.HasValue)
            {
                apartments = apartments.Where(a => a.Building.BuildingID == buildingId).ToList();
            }

            ViewBag.BuildingID = new SelectList(db.Buildings, "BuildingID", "BuildingName");
            return View(apartments);
        }*/

        // GET: Apartments/ListApartments
        public ActionResult ListApartments()
        {
            var apartments = db.Apartments.Include(a => a.Building).Include(a => a.Status);
            return View(apartments.ToList());
        }
        private bool IsValidImage(HttpPostedFileBase file)
        {
            try
            {
                using (var img = System.Drawing.Image.FromStream(file.InputStream))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
    
}
