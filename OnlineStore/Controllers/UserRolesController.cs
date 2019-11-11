using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Controllers
{
    public class UserRolesController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: UserRoles
        public ActionResult Index()
        {
            using (var context = new ApplicationDbContext())
            {
                var roles = context.Roles.ToList();

                return View(roles);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(IdentityRole role)
        {
            using (var context = new ApplicationDbContext())
            {
                if (!ModelState.IsValid)
                    return View();
                context.Roles.Add(role);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult Details(string roleID)
        {
            using (var context = new ApplicationDbContext())
            {
                var role = context.Roles.FirstOrDefault(x => x.Id == roleID);
                return View(role);
            }
        }

        [HttpGet]
        public ActionResult Edit(string roleID)
        {
            using (var context = new ApplicationDbContext())
            {
                var role = context.Roles.FirstOrDefault(x => x.Id == roleID);
                return View(role);
            }
        }
        [HttpPost]
        public ActionResult Edit(IdentityRole role)
        {
            using (var context = new ApplicationDbContext())
            {
                if (!ModelState.IsValid)
                    return View();
                var r = context.Roles.FirstOrDefault(x => x.Id == role.Id);
                r.Name = role.Name;
                context.Entry(r).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Users()
        {
            using (var context = new ApplicationDbContext())
            {
                var userRoles = new List<UserRolesViewModel>();
                var roles = context.Roles.ToList();
                var users = context.Users.ToList();
                foreach (var user in users)
                {
                    var roleNames = new List<string>();
                    foreach (var userRole in user.Roles)
                    {
                        var role = roles.FirstOrDefault(r => r.Id == userRole.RoleId);
                        roleNames.Add(role.Name);
                    }
                    userRoles.Add(new UserRolesViewModel
                    {
                        UserID = user.Id,
                        UserName = user.UserName,
                        RoleNames = string.Join(", ", roleNames)
                    });
                }
                return View(userRoles);
            }
        }
        [HttpGet]
        public ActionResult EditUser(string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == userId);

                var roles = context.Roles.ToList();
                var roleNames = new List<string>();
                foreach (var userRole in user.Roles)
                {
                    var role = roles.First(r =>
                    r.Id == userRole.RoleId);
                    roleNames.Add(role.Name);
                }
                ViewBag.Roles = roles
                    .Select(r => new SelectListItem
                    {
                        Value = r.Id,
                        Text = r.Name
                    }).ToList();
                return View(new UserRolesViewModel
                {
                    UserID = user.Id,
                    UserName = user.UserName,
                    RoleNames = string.Join(" ,", roleNames)
                });
            }
        }

            [HttpPost]
        public ActionResult AddRoleToUser(string UserName, string roleID)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserName == UserName);
                var roleName = context.Roles.FirstOrDefault(r => r.Id == roleID).Name;
                UserManager.AddToRole(user.Id, roleName);
                return RedirectToAction("Users");
            }
        }

        [HttpPost]
        public ActionResult Remove(string roleID)
        {
            using (var context = new ApplicationDbContext())
            {
                var roleToDelete = context.Roles.Find(roleID);
                {
                    if (roleToDelete == null) return Json(new { statusCode = 404, message = "Role not found!" });
                    if (roleToDelete.Name == "Admin")
                    {
                        return Json(new { statusCode = 403, message = "Cannot remove this role!" });
                    }
                    context.Roles.Remove(roleToDelete);
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return Json(new { statusCode = 500, message = "Shit happens!" });
                    }

                    return Json(new { statusCode = 204, message = "Role has been deleted!" });

                }
            }
        }
    }
}
