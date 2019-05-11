using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using Flight.Models;

namespace Flight.Controllers
{
    public class FlyRegistersController : Controller
    {
        private FlightsContext db = new FlightsContext();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FlyRegister objUser)
        {

            if (ModelState.IsValid)
            {
                using (FlightsContext db = new FlightsContext())
                {
                    var obj = db.FlyRegisters.Where(a => a.UserName.Equals(objUser.UserName) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserName"] = obj.UserName.ToString();
                        Session["EmailId"] = obj.EmailId.ToString();
                        if (obj.Role == "Admin")
                        {
                            //var res = db.FlyAdmins.Where(a => a.Email == obj.EmailId).FirstOrDefault();
                            //Session["Id"] = res.adminID;
                            return RedirectToAction("Index", "FlyAdmins");
                        }
                        else if (obj.Role == "User")
                        {
                            //var res = db.FlyUsers.Where(a => a.Email == obj.EmailId).FirstOrDefault();
                            //Session["Id"] = res.UserId;
                            return RedirectToAction("Index", "FlyUsers");
                        }
                    }
                    else
                    {
                        //ViewBag.Javascript = "<script language='javascript' type='text/javascript'>alert('Invalid Username or Password');</script>";
                        return View(objUser);
                    }
                }
            }
            return View(objUser);
        }
        private bool Isvalid(string username, string password)
        {
            bool Isvalid = false;
            using (FlightsContext db = new FlightsContext())
            {
                var user = db.FlyRegisters.FirstOrDefault(s => s.UserName == username);
                if (user != null)
                {
                    if (user.Password == password)
                    {
                        Isvalid = true;
                    }
                }
            }
            return Isvalid;
        }

        
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(FlyRegister objUser)
        {
            if (ModelState.IsValid)
            {
                FlightsContext db = new FlightsContext();
                db.FlyRegisters.Add(objUser);
                db.SaveChanges();
            }
            ViewBag.Message = "Successfull Created";
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            if (Session["UserName"] != null)
            {
                Session.Abandon();

            }
            return RedirectToAction("Login", "FlyRegisters");
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
