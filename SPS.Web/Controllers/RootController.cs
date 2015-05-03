using SPS.Repository;
using SPS.Security;
using SPS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPS.Web.Controllers
{
    public class RootController : Controller
    {
        private const int TokenExpirationTimeSeconds = 60;

        // GET: Root
        public ActionResult Index()
        {
            return View();
        }

        //
        // POST: /Root/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(RootUserViewModel rootUser)
        {
            var user = SPSDb.Instance.Roots.Find(rootUser.CPF);

            if (user == null)
            {
                ModelState.AddModelError("", "Esse CPF não tem permissão para acessar essa página.");
                return View("Index", rootUser);
            }

            if (!SPS.Security.TokenGeneratorService.IsTokenValid(user.PasswordHash, rootUser.Token, TokenExpirationTimeSeconds))
            {
                ModelState.AddModelError("", "Token inválido ou expirado!");
                return View("Index", rootUser);
            }

            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("ParkingManagement", "Root");
        }

        // GET: Root/ParkingManagement
        public ActionResult ParkingManagement()
        {
            return View();
        }
    }
}