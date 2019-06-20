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
    public class RENTsController : Controller
    {
        private HOTEL3Entities db = new HOTEL3Entities();

        string createId()
        {
            var max = db.RENTs.ToList().Select(n => n.Rent_id).Max();
            int id = int.Parse(max.Substring(2)) + 1;
            string type = String.Concat("00", id.ToString());
            return "R" + type.Substring(id.ToString().Length - 1);
        }


        // GET: RENTs
        public ActionResult Index()
        {
            var rENTs = db.RENTs.Include(r => r.ROOMTYPE);
            return View(rENTs.ToList());
        }

        // GET: RENTs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RENT rENT = db.RENTs.Find(id);
            if (rENT == null)
            {
                return HttpNotFound();
            }
            return View(rENT);
        }

        // GET: RENTs/Create
        public ActionResult Create()
        {
            ViewBag.RoomType_id = new SelectList(db.ROOMTYPEs, "RoomType_id", "RType");
            return View();
        }

        // POST: RENTs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Rent_id,Price,IsActive,From_Date,To_Date,RoomType_id")] RENT rENT)
        {
            if (ModelState.IsValid)
            {
                rENT.Rent_id = createId();
                db.RENTs.Add(rENT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoomType_id = new SelectList(db.ROOMTYPEs, "RoomType_id", "RType", rENT.RoomType_id);
            return View(rENT);
        }

        // GET: RENTs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RENT rENT = db.RENTs.Find(id);
            if (rENT == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomType_id = new SelectList(db.ROOMTYPEs, "RoomType_id", "RType", rENT.RoomType_id);
            return View(rENT);
        }

        // POST: RENTs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Rent_id,Price,IsActive,From_Date,To_Date,RoomType_id")] RENT rENT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rENT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomType_id = new SelectList(db.ROOMTYPEs, "RoomType_id", "RType", rENT.RoomType_id);
            return View(rENT);
        }

        // GET: RENTs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RENT rENT = db.RENTs.Find(id);
            if (rENT == null)
            {
                return HttpNotFound();
            }
            return View(rENT);
        }

        // POST: RENTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RENT rENT = db.RENTs.Find(id);
            db.RENTs.Remove(rENT);
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
