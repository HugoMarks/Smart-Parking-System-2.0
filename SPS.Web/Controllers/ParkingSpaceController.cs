using SPS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SPS.Web.Extensions;
using Microsoft.AspNet.Identity;
using SPS.BO;
using SPS.Model;
using SPS.BO.Exceptions;

namespace SPS.Web.Controllers
{
    public class ParkingSpaceController : Controller
    {
        public ActionResult Manage()
        {
            if (Request.IsAuthenticated)
            {
                var spaces = GetParkingFromCurrentLocalAdmin().Spaces;

                return View(spaces);
            }

            return RedirectToAction("Login", "Account");
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Add(ParkingSpaceViewModel model)
        {
            var parking = GetParkingFromCurrentLocalAdmin();
            var space = model.ToParkingSpace();

            space.Parking = parking;

            try
            {
                BusinessManager.Instance.ParkingsSpaces.Add(space);
            }
            catch (UniqueKeyViolationException)
            {
                ModelState.AddModelError("", "Já uma vaga nesse estacionamento com o mesmo número.");
                return View(model);
            }

            var spaces = GetParkingFromCurrentLocalAdmin().Spaces;

            return View("Manage", spaces);
        }

        public ActionResult Edit(string id)
        {
            var parking = GetParkingFromCurrentLocalAdmin();
            var space = parking.Spaces.SingleOrDefault(p => p.Id == int.Parse(id));
            var model = space.ToParkingSpaceViewModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ParkingSpaceViewModel model)
        {
            var space = model.ToParkingSpace();

            space.Parking = GetParkingFromCurrentLocalAdmin();
            BusinessManager.Instance.ParkingsSpaces.Update(space);

            var spaces = GetParkingFromCurrentLocalAdmin().Spaces;

            return View("Manage", spaces);
        }

        private Parking GetParkingFromCurrentLocalAdmin()
        {
            var user = User.Identity.GetApplicationUser();
            var localAdmin = BusinessManager.Instance.LocalManagers.FindAll().SingleOrDefault(l => l.Email == user.Email);

            return BusinessManager.Instance.Parkings.FindAll().SingleOrDefault(p => p.LocalManager.CPF == localAdmin.CPF);
        }
    }
}