using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hotel_test1.Models;


namespace Hotel_test1.Controllers
{
    public class TestController : Controller
    {
        private HOTEL3Entities db = new HOTEL3Entities();


        // GET: Test
        public ActionResult Index()
        {
            return View();
        }


        // GET: Contact
        public ActionResult Contact()
        {
            return View();
        }


        // GET: Roomtypes
        // roomtype
        public ActionResult Roomtypes()
        {
            String query = "SELECT rt.RoomType_id, rt.RType, rt.Descriptions, rt.Images, rt.Views, rt.Bed, rt.MaxPerson, rt.Size, r.Price "
                + "FROM ROOMTYPE rt, RENT r "
                + "WHERE (rt.RoomType_id = r.RoomType_id) and (r.IsActive = 'True')";
            var data = db.Database.SqlQuery<RoomtypeRentViewModel>(query);

            return View(data.ToList());

       //     return View(db.ROOMTYPEs.ToList());
        }

        // GET: Test/RoomtypeDetail/5
        public ActionResult RoomtypeDetail(string id)
        {
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROOMTYPE Roomtype = db.ROOMTYPEs.Find(id);

            if (Roomtype == null)
            {
                return HttpNotFound();
            }
            return View(Roomtype);
        }

    }


}