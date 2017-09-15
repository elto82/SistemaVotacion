using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaVotacion.Models;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SistemaVotacion.Classes;

namespace SistemaVotacion.Controllers
{
    public class UsersController : Controller
    {
        private DemocracyContext db = new DemocracyContext();
        [Authorize(Roles = "Admin")]
        // GET: Users
        public ActionResult Index()
        {
            var userContext = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var users = db.Users.ToList();
            var usersView = new List<UserIndexView>();  
            foreach (var user in users)
            {
                var userASP = userManager.FindByEmail(user.UserName);

                usersView.Add(new UserIndexView
                {
                    Address = user.Address,
                    Candidates = user.Candidates,
                    FirstName = user.FirstName,
                    Grade = user.Grade,
                    Group = user.Group,
                    GroupMembers = user.GroupMembers,
                    IsAdmin = userASP != null && userManager.IsInRole(userASP.Id, "Admin"),
                    LastName = user.LastName,
                    Phone = user.Phone,
                    Photo = user.Photo,
                    UserID = user.UserID,
                    UserName = user.UserName,

                });
            }

            return View(usersView);
        }
        [Authorize(Roles = "Admin")]
        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [Authorize(Roles = "Admin")]
        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserView userView)
        {
            if (!ModelState.IsValid)
            {
                return View(userView);
            }
            //subir foto
            string path = string.Empty;
            string pic = string.Empty;

            if (userView.Photo != null)
            {
                pic = Path.GetFileName(userView.Photo.FileName);
                path = Path.Combine(Server.MapPath("~/Content/Photos/"), pic);
                userView.Photo.SaveAs(path);
                using (MemoryStream ms = new MemoryStream())
                {
                    userView.Photo.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
            }
            //gravar user
            var user = new User
            {
                Address = userView.Address,
                FirstName = userView.FirstName,
                LastName = userView.LastName,
                Grade = userView.Grade,
                Group = userView.Group,
                Phone = userView.Phone,
                Photo = pic == string.Empty ? string.Empty : string.Format($"~/Content/Photos/{pic}"),
                UserName = userView.UserName,
            };

            db.Users.Add(user);
            try
            {
                db.SaveChanges();
                this.CreateASPUser(userView);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("UserNameIndex"))
                {
                    ViewBag.Error = "Este correo ya esta en uso";
                }

                else
                {
                    ViewBag.Error = ex.Message;
                }

                return View(userView);
            }

            return RedirectToAction("Index");

        }

        private void CreateASPUser(UserView userView)
        {
            // User management
            var userContext = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));

            // Create User role
            string roleName = "User";

            // Check to see if Role Exists, if not create it
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }

            // Create the ASP NET User
            var userASP = new ApplicationUser
            {
                UserName = userView.UserName,
                Email = userView.UserName,
                PhoneNumber = userView.Phone,
            };

            userManager.Create(userASP, userASP.UserName);

            // Add user to role
            userASP = userManager.FindByName(userView.UserName);
            userManager.AddToRole(userASP.Id, "User");

        }

        [Authorize(Roles = "Admin")]
        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var userView = new UserView
            {
                Address = user.Address,
                FirstName = user.FirstName,
                Grade = user.Grade,
                Group = user.Group,
                LastName = user.LastName,
                Phone = user.Phone,
                UserID = user.UserID,
                UserName = user.UserName,
            };

            return View(userView);
        }

        // POST: Users/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserView userView)
        {
            if (!ModelState.IsValid)
            {
                return View(userView);
            }

            //subir foto
            string path = string.Empty;
            string pic = string.Empty;

            if (userView.Photo != null)
            {
                pic = Path.GetFileName(userView.Photo.FileName);
                path = Path.Combine(Server.MapPath("~/Content/Photos"), pic);
                userView.Photo.SaveAs(path);
                using (MemoryStream ms = new MemoryStream())
                {
                    userView.Photo.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
            }

            var user = db.Users.Find(userView.UserID);

            user.Address = userView.Address;
            user.Grade = userView.Grade;
            user.Group = userView.Group;
            user.LastName = userView.LastName;
            user.Phone = userView.Phone;
            user.FirstName = userView.FirstName;

            if (!string.IsNullOrEmpty(pic))
            {
                user.Photo = string.Format($"~/Content/Photos/{pic}");

            }

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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

                return View(user);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult OnOffAdministrator(int id)
        {
            var user = db.Users.Find(id);

            if (user != null)
            {
                var userContext = new ApplicationDbContext();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
                var userASP = userManager.FindByEmail(user.UserName);

                if (userASP != null)
                {
                    if (userManager.IsInRole(userASP.Id, "Admin"))
                    {
                        userManager.RemoveFromRoles(userASP.Id, "Admin");
                    }
                    else
                    {
                        userManager.AddToRole(userASP.Id, "Admin");

                    }
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public ActionResult MySetting()
        {
            var user = db.Users.Where(u => u.UserName == this.User.Identity.Name).FirstOrDefault();

            var view = new UserSettingView
            {
                Address = user.Address,
                FirstName = user.FirstName,
                Grade = user.Grade,
                Group = user.Group,
                LastName = user.LastName,
                Phone = user.Phone,
                Photo = user.Photo,
                UserID = user.UserID,
                UserName = user.UserName,
            };

            return View(view);
        }

        [HttpPost]
        public ActionResult MySetting(UserSettingView view)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;
                string pic = string.Empty;

                if (view.NewPhoto != null)
                {
                    pic = Path.GetFileName(view.NewPhoto.FileName);
                    path = Path.Combine(Server.MapPath("~/Content/Photos"), pic);
                    view.NewPhoto.SaveAs(path);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        view.NewPhoto.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }

                var user = db.Users.Find(view.UserID);

                user.Address = view.Address;
                user.Grade = view.Grade;
                user.Group = view.Group;
                user.LastName = view.LastName;
                user.Phone = view.Phone;
                user.FirstName = view.FirstName;

                if (!string.IsNullOrEmpty(pic))
                {
                    user.Photo = string.Format($"~/Content/Photos/{pic}");

                }

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","Home");

            }
            return View(view);
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
