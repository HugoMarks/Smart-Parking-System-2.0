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
    }
}