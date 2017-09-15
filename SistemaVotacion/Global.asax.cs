using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SistemaVotacion.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SistemaVotacion
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            this.CheckSuperUser();
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Models.DemocracyContext,
                Migrations.Configuration>());
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void CheckSuperUser()
        {
            var userContext = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var db = new DemocracyContext();

            this.CheckRole("Admin", userContext);
            this.CheckRole("User", userContext);

            var user = db.Users.Where(u => u.UserName.ToLower().Equals("elto.82@gmail.com")).FirstOrDefault();

            if (user == null)
            {
                user = new User
                {
                    Address = "Rio Arriba",
                    FirstName = "Argiro",
                    LastName = "Arias",
                    Phone = "3226917839",
                    UserName = "elto.82@gmail.com",
                    
                };

                db.Users.Add(user);
                db.SaveChanges();
            }

            var userASP = userManager.FindByName(user.UserName);

            if (userASP == null)
            {
                // Create the ASP NET User
                 userASP = new ApplicationUser
                {
                    UserName = user.UserName,
                    Email = user.UserName,
                    PhoneNumber = user.Phone,
                };

                userManager.Create(userASP,"a71385542A.");
            }

            userManager.AddToRole(userASP.Id, "Admin");
        }

        private void CheckRole(string roleName, ApplicationDbContext userContext)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));
            
            // Check to see if Role Exists, if not create it
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }
        }
    }
}
