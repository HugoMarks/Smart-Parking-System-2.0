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
using System.Threading.Tasks;

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
            var client = BusinessManager.Instance.MontlyClients.FindAll().SingleOrDefault(c => c.Email == user.Email);
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
                var tag = BusinessManager.Instance.Tags.Find(model.Tag);
                var client = BusinessManager.Instance.MontlyClients.Find(tag.Client.CPF);

                model.ParkingCNPJ = collaborator.Parking.CNPJ;

                var record = model.ToUsageRecord();

                record.Client = client;
                BusinessManager.Instance.UsageRecords.Add(record);
                return RedirectToAction("Index", "Collaborator");
            }

            return View(model);
        }
    }
}