using SPS.BO;
using SPS.Model;
using SPS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SPS.Web.Extensions;

namespace SPS.Web.Controllers
{
    public class ParkingController : Controller
    {
        // GET: Parking
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterParkingViewModel model)
        {
            if(ModelState.IsValid)
            {
                Parking parking = BusinessManager.Instance.Parkings.Find(model.CNPJ);

                if (parking == null)
                {
                    parking = model.ToParking();
                    BusinessManager.Instance.Parkings.Add(parking);

                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
                }

                ModelState["CNPJ"].Errors.Add("Já existe um estacionamento com este CNPJ");
            }

            return View(model);
        }
    }
}