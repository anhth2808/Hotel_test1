using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hotel_test1.Models;

namespace Hotel_test1.Controllers
{
    public class BOOKINGsController : Controller
    {
        private HOTEL3Entities db = new HOTEL3Entities();

        // GET: BOOKINGs
        public ActionResult Index(string searchString, string category)
        {
            if (Session["AdminId"] == null || Session["AdminId"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }

            var bOOKINGs = db.BOOKINGs.Include(b => b.CUSTOMER).Include(b => b.ROOM);


            if (!String.IsNullOrEmpty(searchString))
            {
                if (!String.IsNullOrEmpty(category))
                {
                    ViewBag.category = category;

                    switch (category)
                    {
                        case "Booking_id":
                            bOOKINGs = bOOKINGs.Where(s => s.Booking_id.Contains(searchString));
                            break;
                        case "CustomerLastName":
                            bOOKINGs = bOOKINGs.Where(s => s.CUSTOMER.CustomerFirstName.Contains(searchString));
                            break;
                        case "CustomerFirstName":
                            bOOKINGs = bOOKINGs.Where(s => s.CUSTOMER.CustomerFirstName.Contains(searchString));
                            break;
                        case "RType":
                            bOOKINGs = bOOKINGs.Where(s => s.ROOM.ROOMTYPE.RType.Contains(searchString));
                            break;
                        case "Room_id":
                            bOOKINGs = bOOKINGs.Where(s => s.ROOM.Room_id.Contains(searchString));
                            break;
                    }
                }
            }




            return View(bOOKINGs.ToList());
        }

        // GET: BOOKINGs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOOKING bOOKING = db.BOOKINGs.Find(id);
            if (bOOKING == null)
            {
                return HttpNotFound();
            }
            return View(bOOKING);
        }

        // GET: BOOKINGs/Create
        public ActionResult Create()
        {
            ViewBag.Customer_id = new SelectList(db.CUSTOMERs, "Customer_id", "CustomerFirstName");
            ViewBag.Room_id = new SelectList(db.ROOMs, "Room_id", "RoomType_id");
            return View();
        }

        // POST: BOOKINGs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Booking_id,Check_in_date,Check_out_date,Customer_id,Room_id")] BOOKING bOOKING)
        {
            if (ModelState.IsValid)
            {
                db.BOOKINGs.Add(bOOKING);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Customer_id = new SelectList(db.CUSTOMERs, "Customer_id", "CustomerFirstName", bOOKING.Customer_id);
            ViewBag.Room_id = new SelectList(db.ROOMs, "Room_id", "RoomType_id", bOOKING.Room_id);
            return View(bOOKING);
        }

        // GET: BOOKINGs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOOKING bOOKING = db.BOOKINGs.Find(id);
            if (bOOKING == null)
            {
                return HttpNotFound();
            }
            ViewBag.Customer_id = new SelectList(db.CUSTOMERs, "Customer_id", "CustomerFirstName", bOOKING.Customer_id);
            ViewBag.Room_id = new SelectList(db.ROOMs, "Room_id", "RoomType_id", bOOKING.Room_id);
            return View(bOOKING);
        }

        // POST: BOOKINGs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Booking_id,Check_in_date,Check_out_date,Customer_id,Room_id")] BOOKING bOOKING)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bOOKING).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Customer_id = new SelectList(db.CUSTOMERs, "Customer_id", "CustomerFirstName", bOOKING.Customer_id);
            ViewBag.Room_id = new SelectList(db.ROOMs, "Room_id", "RoomType_id", bOOKING.Room_id);
            return View(bOOKING);
        }

        // GET: BOOKINGs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOOKING bOOKING = db.BOOKINGs.Find(id);
            if (bOOKING == null)
            {
                return HttpNotFound();
            }
            return View(bOOKING);
        }

        // POST: BOOKINGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            BOOKING bOOKING = db.BOOKINGs.Find(id);
            var s = db.BILLs.Where(u => u.Booking_id == bOOKING.Booking_id).ToList();

            if (s != null && s.Count > 0)
            {
                db.BILLs.Remove(s[0]);
            }

            db.BOOKINGs.Remove(bOOKING);

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
