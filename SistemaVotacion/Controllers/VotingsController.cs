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
    [Authorize(Roles = "Admin")]
    public class VotingsController : Controller
    {
        private DemocracyContext db = new DemocracyContext();

        // GET: Votings
        public ActionResult Index()
        {
            var votings = db.Votings.Include(v => v.State);
            return View(votings.ToList());
        }

        // GET: Votings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var voting = db.Votings.Find(id);

            if (voting == null)
            {
                return HttpNotFound();
            }

            var view = new  DetailsVotingView
                {
                Candidates = voting.Candidates.ToList(),
                CandidateWindID = voting.CandidateWindID,
                DateTimeStart = voting.DateTimeStart,
                DateTimeEnd = voting.DateTimeEnd,
                Description = voting.Description,
                IsEnableBlankVote = voting.IsEnableBlankVote,
                IsForAllUsers = voting.IsForAllUsers,
                QuantityBlankVotes = voting.QuantityBlankVotes,
                QuantityVotes = voting.QuantityVotes,
                Remarks = voting.Remarks,
                StateID = voting.StateID,
                VotingGroups = voting.VotingGroups.ToList(),
                VotingID = voting.VotingID,
                };

            return View(view);
        }

        // GET: Votings/Create
        public ActionResult Create()
        {
            ViewBag.StateID = new SelectList(db.State, "StateID", "Description");
            var view = new VotingView
            {
                DateStart = DateTime.Now,
                DateEnd = DateTime.Now,
            };
            return View(view);
        }

        // POST: Votings/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VotingView view)
        {
            if (ModelState.IsValid)
            {
                var voting = new Voting
                {
                    DateTimeEnd = view.DateEnd.AddHours(view.TimeEnd.Hour).AddMinutes(view.TimeEnd.Minute),
                    DateTimeStart = view.DateStart.AddHours(view.TimeStart.Hour).AddMinutes(view.TimeStart.Minute),
                    Description = view.Description,
                    IsEnableBlankVote = view.IsEnableBlankVote,
                    IsForAllUsers = view.IsForAllUsers,
                    Remarks = view.Remarks,
                    StateID = view.StateID,
                };

                db.Votings.Add(voting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StateID = new SelectList(db.State, "StateID", "Description", view.StateID);
            return View(view);
        }

        // GET: Votings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Voting voting = db.Votings.Find(id);
            if (voting == null)
            {
                return HttpNotFound();
            }

            var view = new VotingView
            {
                DateEnd = voting.DateTimeEnd,
                DateStart = voting.DateTimeStart,
                Description = voting.Description,
                IsEnableBlankVote = voting.IsEnableBlankVote,
                IsForAllUsers = voting.IsForAllUsers,
                Remarks = voting.Remarks,
                StateID = voting.StateID,
                TimeEnd = voting.DateTimeEnd,
                TimeStart = voting.DateTimeStart,
                VotingID = voting.VotingID,
            };

            ViewBag.StateID = new SelectList(db.State, "StateID", "Description", voting.StateID);
            return View(view);
        }

        // POST: Votings/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VotingView view)
        {
            if (ModelState.IsValid)
            {
                var voting = new Voting
                {
                    DateTimeEnd = view.DateEnd.AddHours(view.TimeEnd.Hour).AddMinutes(view.TimeEnd.Minute),
                    DateTimeStart = view.DateStart.AddHours(view.TimeStart.Hour).AddMinutes(view.TimeStart.Minute),
                    Description = view.Description,
                    IsEnableBlankVote = view.IsEnableBlankVote,
                    IsForAllUsers = view.IsForAllUsers,
                    Remarks = view.Remarks,
                    StateID = view.StateID,
                    VotingID = view.VotingID,


                };

                db.Entry(voting).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();

                }
                catch (Exception ex)
                {

                    ViewBag.Error = ex.Message;

                    return RedirectToAction("Edit");
                }

                return RedirectToAction("Index");
            }

            ViewBag.StateID = new SelectList(db.State, "StateID", "Description", view.StateID);
            return View(view);
        }

        // GET: Votings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voting voting = db.Votings.Find(id);
            if (voting == null)
            {
                return HttpNotFound();
            }
            return View(voting);
        }

        // POST: Votings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Voting voting = db.Votings.Find(id);
            db.Votings.Remove(voting);
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

                return View(voting);

            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AddGroup(int id)
        {
            ViewBag.GroupID = new SelectList(
                db.Groups.OrderBy(g => g.Description),
                "GroupID", "Description");

            var view = new AddGroupView
            {
                VotingID = id,
            };

            return View(view);
        }

        [HttpPost]
        public ActionResult AddGroup(AddGroupView view)
        {
            if (ModelState.IsValid)
            {
                var votingGroup = db.VotingGroup.
                    Where(vg => vg.VotingID == view.VotingID &&
                            vg.GroupID == view.GroupID).FirstOrDefault();

                if (votingGroup != null)
                {
                    ModelState.AddModelError(string.Empty," the group already belongs to voting");


                    ViewBag.GroupID = new SelectList(
                    db.Groups.OrderBy(g => g.Description),
                    "GroupID", "Description");

                    return View(view);
                }

                votingGroup = new VotingGroup
                {
                    GroupID = view.GroupID,
                    VotingID = view.VotingID,
                };

                db.VotingGroup.Add(votingGroup);
                try
                {
                    db.SaveChanges();

                }
                catch (Exception ex)
                {

                    ViewBag.Error = ex.Message;
                }

                return RedirectToAction(string.Format(
                    "Details/{0}", view.VotingID));
            }        

             ViewBag.GroupID = new SelectList(
                   db.Groups.OrderBy(g => g.Description),
                   "GroupID", "Description");

            return View(view);

        }

        [HttpGet]
        public ActionResult AddCandidate(int id )
        {
            var view = new AddCandidateView
            {

                VotingID = id,
            };

            ViewBag.UserID = new SelectList(
                db.Users.OrderBy(u => u.FirstName).
                ThenBy(u => u.LastName), "UserID", "FullName");

            return View(view);
        }

        [HttpPost]
        public ActionResult AddCandidate(AddCandidateView view)
        {
            if (ModelState.IsValid)
            {
                var candidate = db.Candidates.
                    Where(c => c.VotingID == view.VotingID &&
                            c.UserID == view.UserID).FirstOrDefault();

                if (candidate != null)
                {
                    ModelState.AddModelError(string.Empty, " the Candidate already belongs to voting");

                    ViewBag.UserID = new SelectList(
                        db.Users.OrderBy(u => u.FirstName).
                        ThenBy(u => u.LastName), "UserID", "FullName");

                    return View(view);
                }

                candidate = new Candidate
                {
                    UserID = view.UserID,
                    VotingID = view.VotingID,
                };

                db.Candidates.Add(candidate);
                try
                {
                    db.SaveChanges();

                }
                catch (Exception ex)
                {

                    ViewBag.Error = ex.Message;
                }

                return RedirectToAction(string.Format(
                    "Details/{0}", view.VotingID));
            }

            ViewBag.UserID = new SelectList(
                db.Users.OrderBy(u => u.FirstName).
                ThenBy(u => u.LastName), "UserID", "FullName");

            return View(view);

        }

        public ActionResult DeleteGroup(int id)
        {
            var votinGroup = db.VotingGroup.Find(id);
            if (votinGroup != null)
            {
                db.VotingGroup.Remove(votinGroup);
                try
                {
                    db.SaveChanges();

                }
                catch (Exception ex)
                {

                    ViewBag.Error = ex.Message;
                    return RedirectToAction(string.Format("Details/{0}", votinGroup.VotingID));
                }
            }

            return RedirectToAction(string.Format("Details/{0}", votinGroup.VotingID));
        }

        public ActionResult DeleteCandidate(int id)
        {
            var candidate = db.Candidates.Find(id);
            if (candidate != null)
            {
                db.Candidates.Remove(candidate);
                try
                {
                    db.SaveChanges();

                }
                catch (Exception ex)
                {

                    ViewBag.Error = ex.Message;
                    return RedirectToAction(string.Format("Details/{0}", candidate.VotingID));
                }
            }

            return RedirectToAction(string.Format("Details/{0}",candidate.VotingID));
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
