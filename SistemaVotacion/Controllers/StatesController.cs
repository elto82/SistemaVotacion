using SistemaVotacion.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SistemaVotacion.Controllers
{
    [Authorize(Roles = "Admin")]

    public class StatesController : Controller
    {
        private DemocracyContext db = new DemocracyContext();

        // GET: States
        [HttpGet]
        public ActionResult Index()
        {
            
            return View(db.State.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(State state)
        {
            if (!ModelState.IsValid)
            {
                return View(state);
            }

            db.State.Add(state);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var state = db.State.Find(id);
            if (state == null)
            {
                return HttpNotFound();
            }
            return View(state);
        }

        [HttpPost]
        public ActionResult Edit(State state)
        {
            if (!ModelState.IsValid)
            {
                return View(state);
            }

            db.Entry(state).State = EntityState.Modified;
            db.SaveChanges();
          
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var state = db.State.Find(id);
            if (state == null)
            {
                return HttpNotFound();
            }
            return View(state);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var state = db.State.Find(id);
            if (state == null)
            {
                return HttpNotFound();
            }
            return View(state);
        }

        [HttpPost]
        public ActionResult Delete(int id,State state)
        {
            state = db.State.Find(id);

            if (state == null)
            {
                return HttpNotFound();
            }

            db.State.Remove(state);
            try
            {
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("REFERENCE")) 
                {
                    ViewBag.Error = "no puede eliminar datos relacionados ";
                }

                else
                {
                   ViewBag.Error = ex.Message;
                }

                return View(state);

            }         
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