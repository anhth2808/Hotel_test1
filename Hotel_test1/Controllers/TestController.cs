﻿using System;
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
            // list roomtype for search
            // list paytype for booking form 
            ViewBag.roomtypeData = db.ROOMTYPEs.ToList();
            ViewBag.paytypeData = db.PAYTYPEs.ToList();


            return View();
        }
        
        [HttpPost]
        public ActionResult Index(string check_in_date, string check_out_date, string roomtype)
        {
            // list roomtype for search
            // list paytype for booking form
            ViewBag.roomtypeData = db.ROOMTYPEs.ToList();
            ViewBag.paytypeData = db.PAYTYPEs.ToList();


            // -------------check user input-------------
            if (isCorrectFormat(check_in_date) == false || isCorrectFormat(check_out_date) == false)
            {
                return View();
            } else if (NumberOfDays(check_in_date, check_out_date) < 0)
            {
                return View();
            }


            // -------price and total for booking form----------
            int numberOfDays = NumberOfDays(check_in_date, check_out_date);

            String queryRent =
            "SELECT r.Rent_id, r.Price, r.IsActive, r.From_Date, r.To_Date, r.RoomType_id "
            + "FROM(SELECT * FROM ROOMTYPE rt WHERE(rt.RoomType_id = '" + roomtype + "')) rt, RENT r "
            + "WHERE(r.RoomType_id = rt.RoomType_id) and r.IsActive = 'True' ";

            var rent = db.Database.SqlQuery<RENT>(queryRent);
            double price = rent.ToList()[0].Price;
            double total = numberOfDays * price;


            // ---------------check room availability--------------

            string check_in_date_format = FormatDate(check_in_date);
            string check_out_date_format = FormatDate(check_out_date);

            String query =
                "DECLARE @S DATE; "
                + " SET @S = '" + check_in_date_format + "' "
                + " DECLARE @E DATE; "
                + " SET @E = '" + check_out_date_format + "' "
                + " DECLARE @RT varchar(20); "
                + " SET @RT = '" + roomtype + "' "

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

            // not availability
            if (data1.Count <= 0)
            {
                System.Diagnostics.Debug.WriteLine("Khong co phong");
                ViewBag.isExist = false;
                return View();
            } else
            {
                // flag for create booking form
                ViewBag.isExist = true;
            }

           
            //System.Diagnostics.Debug.WriteLine(data1[0].Room_id);

            // booking form
            ViewBag.roomtype_input = db.ROOMTYPEs.Find(roomtype).RType;
            ViewBag.check_in_date_input = check_in_date;
            ViewBag.check_out_date_input = check_out_date;
            ViewBag.room_id = data1[0].Room_id;
            ViewBag.price = price;
            ViewBag.total = total;

            
            

          
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


        public ActionResult Confirm()
        {
            /*


            string roomtype_input = Request["roomtype_input"];
            string customerLastName = Request["customerLastName"];
            string customerFirstName = Request["customerFirstName"];
            string tel = Request["tel"];
            string email = Request["email"];
            string check_in_date_input = Request["check_in_date_input"];
            string check_out_date_input = Request["check_out_date_input"];
            string paytype = Request["paytype"];

            ViewBag.roomtype_input = roomtype_input;
            ViewBag.customerLastName = customerLastName;
            ViewBag.customerFirstName = customerFirstName;
            ViewBag.tel = tel;
            ViewBag.email = email;
            ViewBag.check_in_date_input = check_in_date_input;
            ViewBag.check_out_date_input = check_out_date_input;
            ViewBag.paytype = paytype;

            */

            return View();
        }






        //========================================PartialViews========================================

        public PartialViewResult RenderRoomtype()
        {


            String query = "SELECT rt.RoomType_id, rt.RType, rt.Descriptions, rt.Images, rt.Views, rt.Bed, rt.MaxPerson, rt.Size, r.Price "
                   + "FROM ROOMTYPE rt, RENT r "
                   + "WHERE (rt.RoomType_id = r.RoomType_id) and (r.IsActive = 'True')";
            var data = db.Database.SqlQuery<RoomtypeRentViewModel>(query);


            return PartialView(data.ToList());
        }


        //========================================Functions========================================
        public bool isCorrectFormat(string inputString)
        {
            DateTime dDate;

            if (DateTime.TryParse(inputString, out dDate))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public String FormatDate(string date)
        {
            String[] format = date.Split('/');

            return format[2] + "-" + format[1] + "-" + format[0] ;
        }

        public int NumberOfDays(string startDate, string endDate)
        {
            DateTime dStartDate = Convert.ToDateTime(startDate);
            DateTime dEndDate = Convert.ToDateTime(endDate);


            return (dEndDate.Date - dStartDate.Date).Days;
        }
    }   
}