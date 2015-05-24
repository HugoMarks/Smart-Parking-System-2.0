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
    public class PriceController : Controller
    {
        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Add(PriceViewModel model)
        {
            var parking = GetParkingFromCurrentLocalAdmin();
            var price = model.ToPrice();

            price.Parking = parking;

            try
            {
                BusinessManager.Instance.Prices.Add(price);
            }
            catch (UniqueKeyViolationException)
            {
                ModelState.AddModelError("", "Já existe um preço nesse intervalo de horário");
                return View(model);
            }

            return View("Manage");
        }

        public ActionResult Edit(string id)
        {
            var parking = GetParkingFromCurrentLocalAdmin();
            var price = parking.Prices.SingleOrDefault(p => p.Id == int.Parse(id));
            var model = price.ToPriceViewModel();

            model.Id = int.Parse(id);
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Save(PriceViewModel model)
        {
            var price = model.ToPrice();

            price.Id = model.Id;
            BusinessManager.Instance.Prices.Update(price);
            return View("Manage");
        }

        private Parking GetParkingFromCurrentLocalAdmin()
        {
            var user = User.Identity.GetApplicationUser();
            var localAdmin = BusinessManager.Instance.LocalManagers.FindAll().SingleOrDefault(l => l.Email == user.Email);
            
            return BusinessManager.Instance.Parkings.FindAll().SingleOrDefault(p => p.LocalManager.CPF == localAdmin.CPF);
        }
    }
}