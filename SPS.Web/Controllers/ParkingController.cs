using SPS.BO;
using SPS.Model;
using SPS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SPS.Web.Extensions;
using System.Net;

namespace SPS.Web.Controllers
{
    public class ParkingController : Controller
    {
        // GET: Parking
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterParkingViewModel model)
        {
            if(ModelState.IsValid)
            {
                Parking parking = BusinessManager.Instance.Parkings.Find(model.CNPJ);

                if (parking == null)
                {
                    parking = model.ToParking();
                    BusinessManager.Instance.Parkings.Add(parking);

                    return RedirectToAction("Index", "GlobalAdmin");
                }

                ModelState["CNPJ"].Errors.Add("Já existe um estacionamento com este CNPJ");
            }

            return View(model);
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection form)
        {
            var selectedCNPJ = form["ParkingSelectList"];
            var parking = BusinessManager.Instance.Parkings.Find(selectedCNPJ);
            var model = parking.ToRegisterParkingViewModel();

            return View(model);
        }

        public ActionResult SaveChanges(RegisterParkingViewModel model)
        {
            if (ModelState.IsValid)
            {
                Parking parking = BusinessManager.Instance.Parkings.Find(model.CNPJ);

                if (parking != null)
                {
                    parking = model.ToParking();
                    BusinessManager.Instance.Parkings.Update(parking);

                    return RedirectToAction("Index", "GlobalAdmin");
                }
            }

            return View(model);
        }
    }
}