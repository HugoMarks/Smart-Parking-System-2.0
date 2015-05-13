using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using SPS.Web.Models;
using SPS.BO;
using SPS.Web.Services;
using System.Web.Script.Serialization;
using System.Net;
using SPS.Repository;
using SPS.Web.Extensions;
using SPS.Model;

namespace SPS.Web.Controllers
{
    public class GlobalAdminController : Controller
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

        // This method is private because only authenticated users can see this view.
        private ActionResult Index()
        {
            return View();
        }

        //
        // GET: /GlobalAdmin/Login
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /GlobalAdmin/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(RootUserViewModel rootUser)
        {
            var user = BusinessManager.Instance.GlobalManagers.FindAll().Where(gm => gm.CPF == rootUser.CPF).FirstOrDefault();

            if (user == null)
            {
                ModelState.AddModelError("", "Esse CPF não tem permissão para acessar essa página.");
                return View(rootUser);
            }

            if (!SPS.Security.TokenGeneratorService.IsTokenValid(user.Password, rootUser.Token))
            {
                ModelState.AddModelError("", "Token inválido ou expirado!");
                return View(rootUser);
            }

            HttpContext.GetOwinContext().Authentication.SignOut();
            ViewBag.UserName = user.FirstName;
            return View("Index");
        }
    }
}