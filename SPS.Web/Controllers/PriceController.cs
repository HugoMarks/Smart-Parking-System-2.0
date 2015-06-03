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
            if (Request.IsAuthenticated)
            {
                var prices = GetParkingFromCurrentLocalAdmin().Prices;

                return View(prices);
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
            var prices = GetParkingFromCurrentLocalAdmin().Prices;
            return View("Manage", prices);
        }

        public ActionResult Remove(string id)
        {
            var parking = GetParkingFromCurrentLocalAdmin();
            var price = parking.Prices.SingleOrDefault(p => p.Id == int.Parse(id));

            BusinessManager.Instance.Prices.Remove(price);
            return View("Manage", parking.Prices);
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

            var prices = GetParkingFromCurrentLocalAdmin().Prices;

            return View("Manage", prices);
        }

        private Parking GetParkingFromCurrentLocalAdmin()
        {
            var user = User.Identity.GetApplicationUser();
            var localAdmin = BusinessManager.Instance.LocalManagers.FindAll().SingleOrDefault(l => l.Email == user.Email);
            
            return BusinessManager.Instance.Parkings.FindAll().SingleOrDefault(p => p.LocalManager.CPF == localAdmin.CPF);
        }
    }
}