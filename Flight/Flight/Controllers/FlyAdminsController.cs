using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flight.Models;

namespace Flight.Controllers
{
    public class FlyAdminsController : Controller
    {
        private FlightsContext db = new FlightsContext();

        // GET: FlyAdmins
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                string email = Session["EmailId"].ToString();
                var res = db.FlyAdmins.Where(a => a.Email == email).ToList();
                if (res != null)
                {
                    return View(res);
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Registration");
            }
        }

        // GET: FlyAdmins/Details/5
        public ActionResult Details(string UserName)
        {
            if (Session["UserName"] != null)
            {
                if (UserName == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                FlyRegister flyAdmin = db.FlyRegisters.Where(a=>a.UserName == UserName).FirstOrDefault();
                if (flyAdmin == null)
                {
                    return HttpNotFound();
                }
                return View(flyAdmin);
            }
            else
            {
                return RedirectToAction("Login", "Registration");
            }
        }

        // GET: FlyAdmins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FlyAdmins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserName,Password,Role,EmailID")] FlyRegister flyAdmin)
        {
            if (ModelState.IsValid)
            {
                db.FlyRegisters.Add(flyAdmin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(flyAdmin);
        }

        // GET: FlyAdmins/Edit/5
        public ActionResult Edit(string UserName)
        {
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlyRegister flyAdmin = db.FlyRegisters.Where(a => a.UserName == UserName).FirstOrDefault();
            if (flyAdmin == null)
            {
                return HttpNotFound();
            }
            return View(flyAdmin);
        }

        // POST: FlyAdmins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserName,Password,Role,EmailID")] FlyRegister flyAdmin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flyAdmin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(flyAdmin);
        }

        // GET: FlyAdmins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlyAdmin flyAdmin = db.FlyAdmins.Find(id);
            if (flyAdmin == null)
            {
                return HttpNotFound();
            }
            return View(flyAdmin);
        }

        // POST: FlyAdmins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FlyAdmin flyAdmin = db.FlyAdmins.Find(id);
            db.FlyAdmins.Remove(flyAdmin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //-----------------------------------------create Flight-----------------------------------------------

        // GET: FlightsDetails
        public ActionResult FlightList()
        {
            return View(db.FlightsDetails.ToList());
        }

        // GET: FlightsDetails/Details/5
        public ActionResult FlightDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlightsDetail flightsDetail = db.FlightsDetails.Find(id);
            if (flightsDetail == null)
            {
                return HttpNotFound();
            }
            return View(flightsDetail);
        }

        // GET: FlightsDetails/Create
        public ActionResult FlightCreate()
        {
            return View();
        }

        // POST: FlightsDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FlightCreate([Bind(Include = "FlightId,LaunchDate,Origin,Destination,DeptTime,ArrivalTime,NoOfSeats,Fare")] FlightsDetail flightsDetail)
        {
            if (ModelState.IsValid)
            {

                if (flightsDetail.Origin == flightsDetail.Destination)
                {
                    TempData["message"] = "Origin and Destination cannot be same";
                    return View();
                }
                else
                {
                    db.FlightsDetails.Add(flightsDetail);
                    db.SaveChanges();
                    return RedirectToAction("FlightList");
                }
            }

            return View(flightsDetail);
        }

        // GET: FlightsDetails/Edit/5
        public ActionResult FlightEdit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlightsDetail flightsDetail = db.FlightsDetails.Find(id);
            if (flightsDetail == null)
            {
                return HttpNotFound();
            }
            return View(flightsDetail);
        }

        // POST: FlightsDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FlightEdit([Bind(Include = "FlightId,LaunchDate,Origin,Destination,DeptTime,ArrivalTime,NoOfSeats,Fare")] FlightsDetail flightsDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flightsDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(flightsDetail);
        }

        // GET: FlightsDetails/Delete/5
        public ActionResult FlightDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlightsDetail flightsDetail = db.FlightsDetails.Find(id);
            if (flightsDetail == null)
            {
                return HttpNotFound();
            }
            return View(flightsDetail);
        }

        // POST: FlightsDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult FlightDeletedone(string id)
        {
            FlightsDetail flightsDetail = db.FlightsDetails.Find(id);
            db.FlightsDetails.Remove(flightsDetail);
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
