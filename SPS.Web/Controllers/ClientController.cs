using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SPS.Web.Models;
using SPS.Web.Extensions;
using SPS.BO;
using SPS.Model;
using Microsoft.AspNet.Identity.Owin;
using System.Net;

namespace SPS.Web.Controllers
{
    public class ClientController : Controller
    {
        // GET: Client
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var user = User.Identity.GetApplicationUser();

                if (user.UserType != Models.UserType.Client)
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public ActionResult Edit()
        {
            var user = User.Identity.GetApplicationUser();
            var client = BusinessManager.Instance.MontlyClients.FindAll().SingleOrDefault(c => c.Email == c.Email);
            var model = client.ToEditClientViewModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEditChanges(EditClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                ApplicationUser user = userManager.FindByEmail(model.Email);
                bool error = false;

                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    var result = userManager.ChangePassword(user.Id, model.Password, model.NewPassword);

                    if (!result.Succeeded)
                    {
                        ModelState["Password"].Errors.Add("Senha incorreta");
                        error = true;
                    }
                }
                if (!error)
                {
                    user = userManager.FindByEmail(model.Email);

                    MonthlyClient client = model.ToClient(user.PasswordHash);
                    BusinessManager.Instance.MontlyClients.Update(client);

                    return RedirectToAction("Index", "Client");
                }
            }

            return View("Edit", model);
        }

        public ActionResult UsageRecords()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveRecord(SaveUsageRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = User.Identity.GetApplicationUser();
                var collaborator = BusinessManager.Instance.Collaborators.FindAll().SingleOrDefault(c => c.Email == user.Email);

                model.ParkingCNPJ = collaborator.Parking.CNPJ;

                var record = model.ToUsageRecord();

                BusinessManager.Instance.UsageRecords.Add(record);
                return RedirectToAction("Index", "Collaborator");
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AttachTag(FormCollection form)
        {
            var userEmail = form["UserEmail"];
            var client = BusinessManager.Instance.MontlyClients.FindAll().SingleOrDefault(u => u.Email == userEmail);

            client.Tags.Add(new Tag { Client = client });
            BusinessManager.Instance.MontlyClients.Update(client);

            return RedirectToAction("Index", "Collaborator");
        }

        [HttpPost]
        public ActionResult RequestParking(string parkingCNPJ)
        {
            try
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = User.Identity.GetApplicationUser();
                var client = BusinessManager.Instance.MontlyClients.FindAll().SingleOrDefault(c => c.Email == c.Email);
                var parking = BusinessManager.Instance.Parkings.Find(parkingCNPJ);

                if (parking == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                var localManagerUser = userManager.FindByEmail(parking.LocalManager.Email);
                var message = @"Olá, {0}! O cliente {1} {2} gostaria de fazer parte de nossa rede!<br/> 
                                Ele tem interesse no estacionamento {3} (CNPJ {4}).<br/>
                                Por favor, responda o client no email {5} o mais rápido possível!
                                <br/><br/>
                                <b>Equipe Smart Parking System</b>";

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