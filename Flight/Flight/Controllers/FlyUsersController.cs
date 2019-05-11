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
    public class FlyUsersController : Controller
    {
        private FlightsContext db = new FlightsContext();

        // GET: FlyUsers
        public ActionResult Index()
        {
            return View(db.FlyUsers.ToList());
        }

        // GET: FlyUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlyUser flyUser = db.FlyUsers.Find(id);
            if (flyUser == null)
            {
                return HttpNotFound();
            }
            return View(flyUser);
        }

        // GET: FlyUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FlyUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,Name,Email")] FlyUser flyUser)
        {
            if (ModelState.IsValid)
            {
                db.FlyUsers.Add(flyUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(flyUser);
        }

        // GET: FlyUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlyUser flyUser = db.FlyUsers.Find(id);
            if (flyUser == null)
            {
                return HttpNotFound();
            }
            return View(flyUser);
        }

        // POST: FlyUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Name,Email")] FlyUser flyUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flyUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(flyUser);
        }

        // GET: FlyUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlyUser flyUser = db.FlyUsers.Find(id);
            if (flyUser == null)
            {
                return HttpNotFound();
            }
            return View(flyUser);
        }

        // POST: FlyUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FlyUser flyUser = db.FlyUsers.Find(id);
            db.FlyUsers.Remove(flyUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult FlightView()
        {
            var query = db.FlightsDetails.ToList();
            return View(query);
        }

        [HttpPost]
        public ActionResult FlightView(string origin,string destination)
        {
            var query = db.FlightsDetails.Where(x => x.Origin == origin && x.Destination == destination).ToList();
            return View(query);

        }


        //------------------------------------------------------------------------Booking-------------------------------------


        public ActionResult Booking(string id)
        {
            var flight = db.FlightsDetails.Where(s => s.FlightId == id).FirstOrDefault();
            FlyReservation res = new FlyReservation();
            res.FlightId = flight.FlightId;
            res.Origin = flight.Origin;
            res.Destination = flight.Destination;
            res.TotalFare = flight.Fare;
            return View(res);
        }

        [HttpPost]
        public ActionResult Booking(FlyReservation res)
        {
            res.TotalFare = res.TotalFare * res.NoOfTickets;
            db.FlyReservations.Add(res);
            db.SaveChanges();
            return RedirectToAction("Invoice", "FlyUsers", new { id = res.TicketNo });
            //return View(res);
        }


        public ActionResult Invoice(int? id)
        {
            var query = db.FlyReservations.Where(x => x.TicketNo == id).FirstOrDefault();
            return View(query);
        }

        public ActionResult YourBooking()
        {
            string mail = Session["EmailId"].ToString();
            var query = db.FlyReservations.Where(x => x.Email == mail).ToList();
            return View(query);
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
