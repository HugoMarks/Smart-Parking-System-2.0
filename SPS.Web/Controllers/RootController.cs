using SPS.Repository;
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
            var user = SPSDb.Instance.Root.Find(rootUser.Token);

            if (user == null)
            {
                return View();
            }

            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("ParkingManagement", "Root");
        }

        public ActionResult ParkingManagement()
        {
            return View();
        }
    }
}