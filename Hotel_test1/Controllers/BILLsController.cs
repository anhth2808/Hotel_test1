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
    public class BILLsController : Controller
    {
        private HOTEL3Entities db = new HOTEL3Entities();

        // GET: BILLs
        public ActionResult Index()
        {
            var bILLs = db.BILLs.Include(b => b.BILLPAY).Include(b => b.BOOKING).Include(b => b.RENT);
            return View(bILLs.ToList());
        }

        // GET: BILLs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BILL bILL = db.BILLs.Find(id);
            if (bILL == null)
            {
                return HttpNotFound();
            }
            return View(bILL);
        }

        // GET: BILLs/Create
        public ActionResult Create()
        {
            ViewBag.BillPay_id = new SelectList(db.BILLPAYs, "BillPay_id", "PayType_id");
            ViewBag.Booking_id = new SelectList(db.BOOKINGs, "Booking_id", "Customer_id");
            ViewBag.Rent_id = new SelectList(db.RENTs, "Rent_id", "RoomType_id");
            return View();
        }

        // POST: BILLs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Bill_id,Total,BillPay_id,Rent_id,Booking_id")] BILL bILL)
        {
            if (ModelState.IsValid)
            {
                db.BILLs.Add(bILL);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BillPay_id = new SelectList(db.BILLPAYs, "BillPay_id", "PayType_id", bILL.BillPay_id);
            ViewBag.Booking_id = new SelectList(db.BOOKINGs, "Booking_id", "Customer_id", bILL.Booking_id);
            ViewBag.Rent_id = new SelectList(db.RENTs, "Rent_id", "RoomType_id", bILL.Rent_id);
            return View(bILL);
        }

        // GET: BILLs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BILL bILL = db.BILLs.Find(id);
            if (bILL == null)
            {
                return HttpNotFound();
            }
            ViewBag.BillPay_id = new SelectList(db.BILLPAYs, "BillPay_id", "PayType_id", bILL.BillPay_id);
            ViewBag.Booking_id = new SelectList(db.BOOKINGs, "Booking_id", "Customer_id", bILL.Booking_id);
            ViewBag.Rent_id = new SelectList(db.RENTs, "Rent_id", "RoomType_id", bILL.Rent_id);
            return View(bILL);
        }

        // POST: BILLs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Bill_id,Total,BillPay_id,Rent_id,Booking_id")] BILL bILL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bILL).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BillPay_id = new SelectList(db.BILLPAYs, "BillPay_id", "PayType_id", bILL.BillPay_id);
            ViewBag.Booking_id = new SelectList(db.BOOKINGs, "Booking_id", "Customer_id", bILL.Booking_id);
            ViewBag.Rent_id = new SelectList(db.RENTs, "Rent_id", "RoomType_id", bILL.Rent_id);
            return View(bILL);
        }

        // GET: BILLs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BILL bILL = db.BILLs.Find(id);
            if (bILL == null)
            {
                return HttpNotFound();
            }
            return View(bILL);
        }

        // POST: BILLs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            BILL bILL = db.BILLs.Find(id);
            db.BILLs.Remove(bILL);
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
