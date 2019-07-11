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
    public class ROOMTYPEsController : Controller
    {
        private HOTEL3Entities db = new HOTEL3Entities();


        string createId()
        {
            var max = db.ROOMTYPEs.ToList().Select(n => n.RoomType_id).Max();
            int id = int.Parse(max.Substring(2)) + 1;
            string type = String.Concat("0", id.ToString());
            return "RT" + type.Substring(id.ToString().Length - 1);
        }


        // GET: ROOMTYPEs
        public ActionResult Index()
        {
            if (Session["AdminId"] == null || Session["AdminId"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }

            return View(db.ROOMTYPEs.ToList());
        }

        // GET: ROOMTYPEs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROOMTYPE rOOMTYPE = db.ROOMTYPEs.Find(id);
            if (rOOMTYPE == null)
            {
                return HttpNotFound();
            }
            return View(rOOMTYPE);
        }

        // GET: ROOMTYPEs/Create
        public ActionResult Create()
        {
            if (Session["AdminId"] == null || Session["AdminId"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        // POST: ROOMTYPEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase[] files, [Bind(Include = "RoomType_id,RType,Descriptions,Images,Views,Bed,MaxPerson,Size")] ROOMTYPE rOOMTYPE)
        {
            List<string> listImg = new List<string>();

            if (ModelState.IsValid)
            {

                foreach (HttpPostedFileBase file in files)
                {
                    if (file != null)
                    {
                        string postedFileName = System.IO.Path.GetFileName(file.FileName);
                        var path = System.IO.Path.Combine(Server.MapPath("/images/" + postedFileName));
                        listImg.Add(postedFileName);
                        file.SaveAs(path);
                    }
                }

                rOOMTYPE.RoomType_id = createId();
                rOOMTYPE.Images = string.Join(",", listImg);
                db.ROOMTYPEs.Add(rOOMTYPE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rOOMTYPE);
        }

        // GET: ROOMTYPEs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROOMTYPE rOOMTYPE = db.ROOMTYPEs.Find(id);
            if (rOOMTYPE == null)
            {
                return HttpNotFound();
            }

            return View(rOOMTYPE);
        }
   

        // POST: ROOMTYPEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase[] files, [Bind(Include = "RoomType_id,RType,Descriptions,Images,Views,Bed,MaxPerson,Size")] ROOMTYPE rOOMTYPE)
        {
            List<string> listImg = new List<string>();

           
            if (ModelState.IsValid)
            {
                foreach (HttpPostedFileBase file in files)
                {
                    if (file != null)
                    {
                        string postedFileName = System.IO.Path.GetFileName(file.FileName);
                        var path = System.IO.Path.Combine(Server.MapPath("/images/" + postedFileName));
                        listImg.Add(postedFileName);
                        file.SaveAs(path);
                    }
                }
                //rOOMTYPE.Images = string.Join(",", listImg);

                db.Entry(rOOMTYPE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rOOMTYPE);
        }

        // GET: ROOMTYPEs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROOMTYPE rOOMTYPE = db.ROOMTYPEs.Find(id);
            if (rOOMTYPE == null)
            {
                return HttpNotFound();
            }
            return View(rOOMTYPE);
        }

        // POST: ROOMTYPEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ROOMTYPE rOOMTYPE = db.ROOMTYPEs.Find(id);
            db.ROOMTYPEs.Remove(rOOMTYPE);
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
