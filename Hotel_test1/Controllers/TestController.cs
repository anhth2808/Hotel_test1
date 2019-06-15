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
             
            ViewBag.roomtypeData = db.ROOMTYPEs.ToList();
            return View();
        }
        
        [HttpPost]
        public ActionResult Index(string check_in_date, string check_out_date, string roomtype)
        {
            // list roomtype for search
            ViewBag.roomtypeData = db.ROOMTYPEs.ToList();

            String query =
                "DECLARE @S DATE; "
                + " SET @S = '2019-01-11' "
                + " DECLARE @E DATE; "
                + " SET @E = '2019-01-13' "
                + " DECLARE @RT varchar(20); "
                + " SET @RT = 'RT01' "

                + " SELECT r.Room_id, r.RoomType_id"
                + " FROM (SELECT * FROM ROOM WHERE ROOM.RoomType_id = @RT) r "
                + " EXCEPT "
                + " SELECT Distinct c.Room_id, c.RoomType_id "
                + " FROM( "
                + "   SELECT a.Room_id, a.RoomType_id, b.Booking_id, b.Check_in_date, b.Check_out_date, b.Customer_id "
                + "    FROM(SELECT * FROM ROOM WHERE ROOM.RoomType_id = @RT) a, BOOKING b "
                + "    WHERE a.Room_id = b.Room_id "
                + " ) c "
                + " WHERE( "
                + "      (@S <= c.Check_in_date and(@E >= c.Check_in_date and @E <= c.Check_out_date)) "
                + "        or((@S >= c.Check_in_date and @S <= c.Check_out_date) and @E >= c.Check_out_date ) "
                + "       or(@S >= c.Check_in_date and @E <= c.Check_out_date) "
                + "       or(@S <= c.Check_in_date and @E >= c.Check_out_date) "
                + ")";


            var data = db.Database.SqlQuery<ROOM>(query);
            List<ROOM> data1 = data.ToList();
            if (data1.Count <= 0)
            {
                System.Diagnostics.Debug.WriteLine("Khong co phong");
                ViewBag.isExist = false;
                return View();
            }

           
            System.Diagnostics.Debug.WriteLine(data1[0].Room_id);
            ViewBag.room_id = data1[0].Room_id;
            ViewBag.isExist = true;

          
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



        public String FormatDate(string date)
        {
            String[] format = date.Split('/');

            return format[2] + "+" + format[1] + "-" + format[0] ;
        }

    }   
}