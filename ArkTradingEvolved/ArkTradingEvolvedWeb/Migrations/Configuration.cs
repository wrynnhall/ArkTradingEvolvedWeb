namespace ArkTradingEvolvedWeb.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Security.Claims;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ArkTradingEvolvedWeb.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ArkTradingEvolvedWeb.Models.ApplicationDbContext";
        }


        protected override void Seed(ArkTradingEvolvedWeb.Models.ApplicationDbContext context)
        {
            // These lines will get us a user manager to create accounts
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            const string admin = "admin@ark.com";
            const string defaultPassword = "P@ssw0rd";

            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Admin" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "General" });
            context.SaveChanges();

            // create an admin user if there isnt one
            if (!context.Users.Any(u => u.UserName == admin))
            {
                var user = new ApplicationUser()
                {
                    UserName = admin,
                    Email = admin
                };

                IdentityResult result = userManager.Create(user, defaultPassword);
                context.SaveChanges(); 
                // save before trying to use this user. If the user is not saved, will not be
                // able to create a join table that joins the user to a role

                if (result.Succeeded)
                {
                    // add roles to the admin user
                    userManager.AddToRole(user.Id, "Admin");
                    context.SaveChanges();
                }
            }



            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
