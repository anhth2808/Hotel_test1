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
using System.Net.Mail;

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
            int price = rent.ToList()[0].Price;
            int total = numberOfDays * price;


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
                ViewBag.showAlert = true;
                return View();
            } else
            {
                // flag for create booking form
                ViewBag.isExist = true;
            }


            //System.Diagnostics.Debug.WriteLine(data1[0].Room_id);

            // booking form
            ViewBag.roomtype_input = roomtype;
            ViewBag.roomtype_show = db.ROOMTYPEs.Find(roomtype).RType;
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

            ViewBag.paytypeData = db.PAYTYPEs.ToList();

            return View(Roomtype);
        }


        // POST: Test/RoomtypeDetail/5
        [HttpPost]
        public ActionResult RoomtypeDetail(string check_in_date, string check_out_date, string roomtype)
        {
            // list roomtype for search
            // list paytype for booking form

            ROOMTYPE Roomtype = db.ROOMTYPEs.Find(roomtype);

            ViewBag.roomtypeData = db.ROOMTYPEs.ToList();
            ViewBag.paytypeData = db.PAYTYPEs.ToList();


            // -------------check user input-------------
            if (isCorrectFormat(check_in_date) == false || isCorrectFormat(check_out_date) == false)
            {
                return View(Roomtype);
            }
            else if (NumberOfDays(check_in_date, check_out_date) < 0)
            {
                return View(Roomtype);
            }


            // -------price and total for booking form----------
            int numberOfDays = NumberOfDays(check_in_date, check_out_date);

            String queryRent =
            "SELECT r.Rent_id, r.Price, r.IsActive, r.From_Date, r.To_Date, r.RoomType_id "
            + "FROM(SELECT * FROM ROOMTYPE rt WHERE(rt.RoomType_id = '" + roomtype + "')) rt, RENT r "
            + "WHERE(r.RoomType_id = rt.RoomType_id) and r.IsActive = 'True' ";

            var rent = db.Database.SqlQuery<RENT>(queryRent);
            int price = rent.ToList()[0].Price;
            int total = numberOfDays * price;


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
                ViewBag.showAlert = true;
                return View(Roomtype);
            }
            else
            {
                // flag for create booking form
                ViewBag.isExist = true;
            }


            //System.Diagnostics.Debug.WriteLine(data1[0].Room_id);

            // booking form
            ViewBag.roomtype_input = roomtype;
            ViewBag.roomtype_show = db.ROOMTYPEs.Find(roomtype).RType;
            ViewBag.check_in_date_input = check_in_date;
            ViewBag.check_out_date_input = check_out_date;
            ViewBag.room_id = data1[0].Room_id;
            ViewBag.price = price;
            ViewBag.total = total;


            return View(Roomtype);
        }




        public ActionResult Confirm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Confirm(string check_in_date_input, string check_out_date_input, string roomtype_input)
        {
            ViewBag.roomtypeData = db.ROOMTYPEs.ToList();
            ViewBag.paytypeData = db.PAYTYPEs.ToList();

            string customerLastName = Request["customerLastName"];
            string customerFirstName = Request["customerFirstName"];
            string tel = Request["tel"];
            string email = Request["email"];
            string paytype = Request["paytype"];


            // -------------check user input-------------
            if (isCorrectFormat(check_in_date_input) == false || isCorrectFormat(check_out_date_input) == false)
            {
                return RedirectToAction("Index");
            }
            else if (NumberOfDays(check_in_date_input, check_out_date_input) < 0)
            {
                return RedirectToAction("Index");
            }


            // -------price and total for booking form----------
            int numberOfDays = NumberOfDays(check_in_date_input, check_out_date_input);

            String queryRent =
            "SELECT r.Rent_id, r.Price, r.IsActive, r.From_Date, r.To_Date, r.RoomType_id "
            + "FROM(SELECT * FROM ROOMTYPE rt WHERE(rt.RoomType_id = '" + roomtype_input + "')) rt, RENT r "
            + "WHERE(r.RoomType_id = rt.RoomType_id) and r.IsActive = 'True' ";

            var rent = db.Database.SqlQuery<RENT>(queryRent);
            int price = rent.ToList()[0].Price;
            int total = numberOfDays * price;


            // ---------------check room availability--------------

            string check_in_date_format = FormatDate(check_in_date_input);
            string check_out_date_format = FormatDate(check_out_date_input);

            String query =
                "DECLARE @S DATE; "
                + " SET @S = '" + check_in_date_format + "' "
                + " DECLARE @E DATE; "
                + " SET @E = '" + check_out_date_format + "' "
                + " DECLARE @RT varchar(20); "
                + " SET @RT = '" + roomtype_input + "' "

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
                ViewBag.showAlert = true;
                return RedirectToAction("Index");
            }
            else
            {
                // flag for create booking form
                ViewBag.isExist = true;
            }


            // create customer
            CUSTOMER mCustomer = new CUSTOMER();
            mCustomer.Customer_id = createCustomerId();
            mCustomer.CustomerFirstName = customerFirstName;
            mCustomer.CustomerLastName = customerLastName;
            mCustomer.CustomerTel = tel;
            mCustomer.CustomerEmail = email;
            db.CUSTOMERs.Add(mCustomer);

            // create booking
            BOOKING mBooking = new BOOKING();
            mBooking.Booking_id = createBookingId();
            mBooking.Check_in_date = Convert.ToDateTime(check_in_date_input);
            mBooking.Check_out_date = Convert.ToDateTime(check_out_date_input);
            mBooking.Customer_id = mCustomer.Customer_id;
            mBooking.Room_id = data1[0].Room_id;
            db.BOOKINGs.Add(mBooking);

            // create billpay
            BILLPAY mBillPay = new BILLPAY();
            mBillPay.BillPay_id = createBillPayId();
            mBillPay.Date = DateTime.Now;
            mBillPay.PayType_id = paytype;
            mBillPay.Customer_id = mCustomer.Customer_id;
            db.BILLPAYs.Add(mBillPay);

            // create bill
            BILL mBill = new BILL();
            mBill.Bill_id = createBillId();
            mBill.Total = total;
            mBill.BillPay_id = mBillPay.BillPay_id;
            mBill.Rent_id = rent.ToList()[0].Rent_id; // ...
            mBill.Booking_id = mBooking.Booking_id;
            db.BILLs.Add(mBill);

            db.SaveChanges();

            SendEmail( mCustomer.CustomerLastName + " " + mCustomer.CustomerFirstName, mCustomer.CustomerTel, mCustomer.CustomerEmail, htmlMail(mBill, db.ROOMTYPEs.Find(roomtype_input).RType ,price));

            return RedirectToAction("ConfirmBillPay", new { id = mBill.Bill_id });            
        }


        public ActionResult ConfirmBillPay(string id)
        {
            BILL mBill = db.BILLs.Find(id);

            return View(mBill);
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



        string createBookingId()
        {
            var max = db.BOOKINGs.ToList().Select(n => n.Booking_id).Max();
            int id = int.Parse(max.Substring(2)) + 1;
            string type = String.Concat("000", id.ToString());
            return "B" + type.Substring(id.ToString().Length - 1);
        }

        string createCustomerId()
        {
            var max = db.CUSTOMERs.ToList().Select(n => n.Customer_id).Max();
            int id = int.Parse(max.Substring(2)) + 1;
            string type = String.Concat("000", id.ToString());
            return "C" + type.Substring(id.ToString().Length - 1);
        }

        string createBillId()
        {
            var max = db.BILLs.ToList().Select(n => n.Bill_id).Max();
            int id = int.Parse(max.Substring(2)) + 1;
            string type = String.Concat("000", id.ToString());
            return "B" + type.Substring(id.ToString().Length - 1);
        }

        string createBillPayId()
        {
            var max = db.BILLPAYs.ToList().Select(n => n.BillPay_id).Max();
            int id = int.Parse(max.Substring(2)) + 1;
            string type = String.Concat("000", id.ToString());
            return "BP" + type.Substring(id.ToString().Length - 1);
        }


        void SendEmail(string _name, string _phone, string _email, string _description)
        {
            string senderID = "banbanga12345@gmail.com";
            string senderPassword = "gatigun1";
            string result = "Email Sent Successfully";

            string body =  _description;
            
            /*
            string body = " " + _name + " has sent an email from " + _email;
            body += "Phone : " + _phone;
            body += _description;
            */

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(_email);
                mail.From = new MailAddress(senderID);
                mail.Subject = "Hóa đơn book phòng Deluxe";
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                smtp.Credentials = new System.Net.NetworkCredential(senderID, senderPassword);
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                result = "problem occurred";
                Response.Write("Exception in sendEmail:" + ex.Message);
            }

        }

        String htmlMail(BILL mBill, string RType, int Price)
        {
            /*
            don't khow why mbill contain null RENT, ROOM connect, can't using mbill to shows price, Rtype of bill              
             */

            String a = 
              
                 "   <div class='container'>  " + 
                 "       <div class='row'>  " + 
                 "           <div class='col-xs-12'>  " + 
                 "               <div class='invoice-title'>  " +
                 "                   <h2>Hóa đơn</h2><h3 class='pull-right'>" + mBill.Bill_id + "</h3>  " + 
                 "               </div>  " + 
                 "               <hr>  " + 
                 "               <div class='row'>  " + 
                 "                   <div class='col-xs-6'>  " + 
                 "                       <address>  " + 
                 "                           <strong>Hóa đơn tới:</strong><br>  " + 
                 "                           " + mBill.BOOKING.CUSTOMER.CustomerLastName + " " + mBill.BOOKING.CUSTOMER.CustomerFirstName + "< br>  "  + 
                 "                           " + mBill.BOOKING.CUSTOMER.CustomerTel + "<br>  " + 
                 "                           " + mBill.BOOKING.CUSTOMER.CustomerEmail + "<br>  " + 
                 "                       </address>  " + 
                 "                   </div>  " + 
                 "                     " + 
                 "               </div>  " + 
                 "               <div class='row'>  " + 
                 "                   <div class='col-xs-6' > "  + 
                 "                       <address>  " + 
                 "                           <strong>Phương thức thanh toán:</strong><br>  " + 
                 "                           " + mBill.BILLPAY.PAYTYPE.PType + "<br>  " +
                 "                           Thời gian thanh toán: " + mBill.BILLPAY.Date.ToString("dd/MM/yyyy") + "<br>  " + 
                 "                       </address>  " + 
                 "                   </div>                 " + 
                 "               </div>  " + 
                 "           </div>  " + 
                 "       </div>  " + 
                 "     " + 
                 "       <div class='row'>  " + 
                 "           <div class='col-md-12'>  " + 
                 "               <div class='panel panel-default'>  " + 
                 "                   <div class='panel-heading'>  " + 
                 "                       <h3 class='panel-title'><strong>Chi tiết Book:</strong></h3>  " + 
                 "                   </div>  " + 
                 "                   <div class='panel-body'>  " + 
                 "                       <div class='table-responsive'>  " + 
                 "                           <table class='table table-condensed'>  " + 
                 "                               <thead>  " + 
                 "                                   <tr>  " + 
                 "                                       <td><strong>Số phòng</strong></td>  " + 
                 "                                       <td class='text-center'><strong>Loại phòng</strong></td>  " + 
                 "                                       <td class='text-center'><strong>Ngày nhận phòng</strong></td>  " + 
                 "                                       <td class='text-center'><strong>Ngày trả phòng</strong></td>  " + 
                 "                                       <td class='text-right'><strong>Giá 1 đêm</strong></td>  " + 
                 "                                   </tr>  " + 
                 "                               </thead>  " + 
                 "                               <tbody>  " + 
                 "     " + 
                 "                                   <tr>  " + 
                 "                                       <td>" + mBill.BOOKING.Room_id + "</td>  " +
                 "                                       <td class='text-center'>" + RType + "</td>  " +
                 "                                       <td class='text-center'>" + mBill.BOOKING.Check_in_date.ToString("dd/MM/yyyy") + "</td>  " + 
                 "                                       <td class='text-center'>" + mBill.BOOKING.Check_out_date.ToString("dd/MM/yyyy") + "</td>  " +
                 "                                       <td class='text-right'>" + Price + "</td>  " + 
                 "                                   </tr>                                  " + 
                 "                                    " + 
                 "                                   <tr>  " + 
                 "                                       <td class='no-line'></td>  " + 
                 "                                       <td class='no-line'></td>  " + 
                 "                                       <td class='no-line'></td>  " + 
                 "                                       <td class='no-line text-center'><strong>Tổng cộng</strong></td>  " +
                 "                                       <td class='no-line text-right'>" + mBill.Total + "</td>  " + 
                 "                                   </tr>  " + 
                 "                               </tbody>  " + 
                 "                           </table>  " + 
                 "                       </div>  " + 
                 "                   </div>  " + 
                 "               </div>  " + 
                 "           </div>  " + 
                 "       </div>  " + 
                 "  </div>  ";
                 
            return a;
        }



    }   
}