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
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

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
            var index = BusinessManager.Instance.LocalManagers.FindAll().ToList().FindIndex(l => l.CPF == model.LocalAdmin);

            ViewBag.LocalAdminIndex = index;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
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

        public ActionResult Finder()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetDetails(string cnpj)
        {
            var parking = BusinessManager.Instance.Parkings.Find(cnpj);

            if (parking == null)
            {
                return Json(new { Success = false, Error = "Estacionamento não encontrado" });
            }

            return Json(new
            {
                Success = true,
                Data = new
                {
                    Name = parking.Name,
                    CNPJ = parking.CNPJ,
                    Address = parking.Address,
                    Number = parking.PhoneNumber,
                    StreetNumber = parking.StreetNumber,
                    Spaces = parking.Spaces.Count
                }
            });
        }

        [HttpPost]
        public ActionResult RequestAttach(string cnpj)
        {
            try
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = User.Identity.GetApplicationUser();
                var client = BusinessManager.Instance.MontlyClients.FindAll().SingleOrDefault(c => c.Email == user.Email);
                var parking = BusinessManager.Instance.Parkings.Find(cnpj);

                if (parking == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                var localManagerUser = userManager.FindByEmail(parking.LocalManager.Email);
                var message = @"Olá, {0}! O cliente {1} {2} gostaria de fazer parte de nossa rede!<br/>
                                Ele tem interesse no estacionamento ""{3}"" ({4}).<br/>
                                Por favor, responda o cliente no email {5} o mais rápido possível!
                                <br/><br/>
                                <b>Equipe Smart Parking System®</b>";

                message = string.Format(message, parking.LocalManager.FirstName, client.FirstName, client.LastName, parking.Name, parking.CNPJ, client.Email);

                userManager.SendEmail(localManagerUser.Id, "Requisição de Vínculo", message);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
    }
}