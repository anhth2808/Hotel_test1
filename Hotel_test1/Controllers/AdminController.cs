using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hotel_test1.Models;
using System.Web.Mvc;

namespace Hotel_test1.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login");
            }

            return View();

        }

        public ActionResult Login()
        {
            return View();
        }       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(ADMIN objAdmin)
        {
            if (ModelState.IsValid)
            {
                using (HOTEL3Entities db = new HOTEL3Entities())
                {
                    var obj = db.ADMINs.Where(a => a.Admin_id.Equals(objAdmin.Admin_id) && a.password.Equals(objAdmin.password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["AdminId"] = obj.Admin_id.ToString();
                        Session["AdminName"] = obj.AdminName.ToString();
                        return RedirectToAction("Index");
                    }
                }
            }

            return View(objAdmin);
        }

    }
}