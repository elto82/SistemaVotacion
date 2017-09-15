using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaVotacion.Models;

namespace SistemaVotacion.Controllers
{
    [Authorize(Roles ="Admin?")]
    public class GroupsController : Controller
    {
        private DemocracyContext db = new DemocracyContext();

        // GET: Groups
        public ActionResult Index()
        {
            return View(db.Groups.ToList());
        }

        // GET: Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);

            if (group == null)
            {
                return HttpNotFound();
            }

            var view = new GroupDetailsView
            {
                GroupID = group.GroupID,
                Description = group.Description,
                Members = group.GroupMembers.ToList(),
               
            };
            return View(view);
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupID,Description")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupID,Description")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
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

                return View(group);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AddMember(int groupID)
        {
            ViewBag.UserID = new SelectList(db.Users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName), "UserID", "FullName");

            var view = new AddMemberView
            {
                GroupID = groupID,
            };
            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMember(AddMemberView view)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UserID = new SelectList(db.Users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName), "UserID", "FullName");
                return View(view);
            }

            var member = db.GroupMembers.Where(
                gm => gm.GroupID == view.GroupID && gm.UserID == view.UserID)
                .FirstOrDefault();

            if (member != null)
            {
                ViewBag.UserID = new SelectList(db.Users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName), "UserID", "FullName");

                ViewBag.Error = "the member already belongs to group";
                return View(view);
            }

            member = new GroupMember
            {
                GroupID = view.GroupID,
                UserID = view.UserID,
            };

            db.GroupMembers.Add(member);
            try
            {
                db.SaveChanges();

            }
            catch (Exception ex)
            {

                ViewBag.Error = ex.Message;
            }

            return RedirectToAction(string.Format("Details/{0}",view.GroupID));
        }

        public ActionResult DeleteMember(int id)
        {
            var member = db.GroupMembers.Find(id);
            if (member != null)
            {
                db.GroupMembers.Remove(member);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    ViewBag.Error = ex.Message;
                }

                return RedirectToAction(string.Format("Details/{0}", member.GroupID));
            }
            return RedirectToAction(string.Format("Details/{0}", member.GroupID));

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
