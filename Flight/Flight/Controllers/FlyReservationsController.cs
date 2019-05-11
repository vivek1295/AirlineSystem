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
    public class FlyReservationsController : Controller
    {
        private FlightsContext db = new FlightsContext();

        // GET: FlyReservations
        public ActionResult Index()
        {
            var flyReservations = db.FlyReservations.Include(f => f.FlightsDetail);
            return View(flyReservations.ToList());
        }

        // GET: FlyReservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlyReservation flyReservation = db.FlyReservations.Find(id);
            if (flyReservation == null)
            {
                return HttpNotFound();
            }
            return View(flyReservation);
        }

        // GET: FlyReservations/Create
        public ActionResult Create()
        {
            ViewBag.FlightId = new SelectList(db.FlightsDetails, "FlightId", "Origin");
            return View();
        }

        // POST: FlyReservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketNo,FlightId,DateOfBooking,JourneyDate,Origin,Destination,PassengerName,ContactNo,Email,NoOfTickets,TotalFare,Status")] FlyReservation flyReservation)
        {
            if (ModelState.IsValid)
            {
                db.FlyReservations.Add(flyReservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FlightId = new SelectList(db.FlightsDetails, "FlightId", "Origin", flyReservation.FlightId);
            return View(flyReservation);
        }

        // GET: FlyReservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlyReservation flyReservation = db.FlyReservations.Find(id);
            if (flyReservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.FlightId = new SelectList(db.FlightsDetails, "FlightId", "Origin", flyReservation.FlightId);
            return View(flyReservation);
        }

        // POST: FlyReservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TicketNo,FlightId,DateOfBooking,JourneyDate,Origin,Destination,PassengerName,ContactNo,Email,NoOfTickets,TotalFare,Status")] FlyReservation flyReservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flyReservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FlightId = new SelectList(db.FlightsDetails, "FlightId", "Origin", flyReservation.FlightId);
            return View(flyReservation);
        }

        // GET: FlyReservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlyReservation flyReservation = db.FlyReservations.Find(id);
            if (flyReservation == null)
            {
                return HttpNotFound();
            }
            return View(flyReservation);
        }

        // POST: FlyReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FlyReservation flyReservation = db.FlyReservations.Find(id);
            db.FlyReservations.Remove(flyReservation);
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
