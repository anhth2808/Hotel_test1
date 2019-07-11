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
    public class ROOMsController : Controller
    {
        private HOTEL3Entities db = new HOTEL3Entities();


        // GET: ROOMs
        public ActionResult Index()
        {
            if (Session["AdminId"] == null || Session["AdminId"].ToString() == "" )
            {
                return RedirectToAction("Login", "Admin");
            }

            var rOOMs = db.ROOMs.Include(r => r.ROOMTYPE);
            return View(rOOMs.ToList());    
        }

        // GET: ROOMs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROOM rOOM = db.ROOMs.Find(id);
            if (rOOM == null)
            {
                return HttpNotFound();
            }
            return View(rOOM);
        }

        // GET: ROOMs/Create
        public ActionResult Create()
        {
            if (Session["AdminId"] == null || Session["AdminId"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.RoomType_id = new SelectList(db.ROOMTYPEs, "RoomType_id", "RType");
            return View();
        }

        // POST: ROOMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Room_id,RoomType_id")] ROOM rOOM)
        {
            if (ModelState.IsValid)
            {
                db.ROOMs.Add(rOOM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoomType_id = new SelectList(db.ROOMTYPEs, "RoomType_id", "RType", rOOM.RoomType_id);
            return View(rOOM);
        }

        // GET: ROOMs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROOM rOOM = db.ROOMs.Find(id);
            if (rOOM == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomType_id = new SelectList(db.ROOMTYPEs, "RoomType_id", "RType", rOOM.RoomType_id);
            return View(rOOM);
        }

        // POST: ROOMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Room_id,RoomType_id")] ROOM rOOM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rOOM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomType_id = new SelectList(db.ROOMTYPEs, "RoomType_id", "RType", rOOM.RoomType_id);
            return View(rOOM);
        }

        // GET: ROOMs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROOM rOOM = db.ROOMs.Find(id);
            if (rOOM == null)
            {
                return HttpNotFound();
            }
            return View(rOOM);
        }

        // POST: ROOMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ROOM rOOM = db.ROOMs.Find(id);
            db.ROOMs.Remove(rOOM);
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
