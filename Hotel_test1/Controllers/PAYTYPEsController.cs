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
    public class PAYTYPEsController : Controller
    {
        private HOTEL3Entities db = new HOTEL3Entities();

        // GET: PAYTYPEs
        public ActionResult Index()
        {
            return View(db.PAYTYPEs.ToList());
        }

        // GET: PAYTYPEs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PAYTYPE pAYTYPE = db.PAYTYPEs.Find(id);
            if (pAYTYPE == null)
            {
                return HttpNotFound();
            }
            return View(pAYTYPE);
        }

        // GET: PAYTYPEs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PAYTYPEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PayType_id,PType")] PAYTYPE pAYTYPE)
        {
            if (ModelState.IsValid)
            {
                db.PAYTYPEs.Add(pAYTYPE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pAYTYPE);
        }

        // GET: PAYTYPEs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PAYTYPE pAYTYPE = db.PAYTYPEs.Find(id);
            if (pAYTYPE == null)
            {
                return HttpNotFound();
            }
            return View(pAYTYPE);
        }

        // POST: PAYTYPEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PayType_id,PType")] PAYTYPE pAYTYPE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pAYTYPE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pAYTYPE);
        }

        // GET: PAYTYPEs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PAYTYPE pAYTYPE = db.PAYTYPEs.Find(id);
            if (pAYTYPE == null)
            {
                return HttpNotFound();
            }
            return View(pAYTYPE);
        }

        // POST: PAYTYPEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PAYTYPE pAYTYPE = db.PAYTYPEs.Find(id);
            db.PAYTYPEs.Remove(pAYTYPE);
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
