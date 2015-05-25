using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using SPS.Web.Models;
using SPS.BO;
using SPS.Model;
using SPS.Web.Extensions;
using System.Net;
using System.Web.Routing;
using System.Security.Claims;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Owin;
using SPS.Web.Services;
using System.Web.Script.Serialization;
using SPS.Repository;
using SPS.BO.Exceptions;
using SPS.Web.Common;


namespace SPS.Web.Controllers
{
    public class LocalAdminController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: LocalAdmin
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var user = User.Identity.GetApplicationUser();

                if (user.UserType != Models.UserType.LocalAdmin)
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

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /LocalAdmin/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = model.ToApplicationUser(UserType.LocalAdmin);
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    LocalManager client = model.ToUser<LocalManager>(user.PasswordHash);

                    try
                    {
                        BusinessManager.Instance.LocalManagers.Add(client);
                    }
                    catch (UniqueKeyViolationException ex)
                    {
                        ModelState["CPF"].Errors.Add(ex.Message);
                    }                 

                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = model.Password }, protocol: Request.Url.Scheme);
                    var message = @"<p>Olá, {0}!</p><p>Segue os dados de acesso para sua conta.</p><p>Usuário: {1}</p><p>Senha: {2}</p>
                                   <p><b>Nunca compartilhe esses dados com ninguém. Caso contrário, ações admistrativas serão tomadas.</b></p>";

                    message = string.Format(message, client.FirstName, client.Email, model.Password);
                    await UserManager.SendEmailAsync(user.Id, "Sua conta no Smart Parking System", message);

                    return RedirectToAction("Index", "GlobalAdmin");
                }
                else
                {
                    var emailErrors = result.Errors.Where(e => e.Contains("email"));

                    if (emailErrors.Count() > 0)
                    {
                        foreach (var error in emailErrors)
                        {
                            ModelState["Email"].Errors.Add(error);
                        }
                    }
                }
            }

            // Se chegamos até aqui e houver alguma falha, exiba novamente o formulário
            return View(model);
        }
        public PartialViewResult GetLocalAdmins()
        {
            return PartialView("_LocalAdminsListPartial");
        }

        [HttpPost]
        public ActionResult FullEdit(FormCollection form)
        {
            var selectedLocalAdmin = form["LocalAdminsDropDownList"];
            var localAdmin = BusinessManager.Instance.LocalManagers.Find(selectedLocalAdmin);
            var model = localAdmin.ToFullEditLocalAdminViewModel();

            return View(model);
        }

        private ActionResult FullEdit(FullEditCollaboratorViewModel model)
        {
            return View(model);
        }

        public ActionResult Edit()
        {
            var user = User.Identity.GetApplicationUser();
            var localAdmin = BusinessManager.Instance.LocalManagers.FindAll().Where(c => c.Email == user.Email).FirstOrDefault();
            var model = localAdmin.ToFullEditLocalAdminViewModel();

            return View(model);
        }

        private ActionResult Edit(EditCollaboratorViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveEditChanges(EditLocalAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindByEmailAsync(model.Email);
                bool error = false;

                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    var result = UserManager.ChangePassword(user.Id, model.Password, model.NewPassword);

                    if (!result.Succeeded)
                    {
                        ModelState["Password"].Errors.Add("Senha incorreta");
                        error = true;
                    }
                }
                if (!error)
                {
                    user = await UserManager.FindByEmailAsync(model.Email);

                    LocalManager localAdmin = model.ToLocalAdmin(user.PasswordHash);
                    BusinessManager.Instance.LocalManagers.Update(localAdmin);

                    return RedirectToAction("Index", "LocalAdmin");
                }
            }

            return View("Edit", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveFullEditChanges(FullEditLocalAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindByEmailAsync(model.Email);
                bool error = false;

                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    var result = UserManager.ChangePassword(user.Id, model.Password, model.NewPassword);

                    if (!result.Succeeded)
                    {
                        ModelState["Password"].Errors.Add("Senha incorreta");
                        error = true;
                    }
                }
                if (!error)
                {
                    user = await UserManager.FindByEmailAsync(model.Email);

                    LocalManager localAdmin = model.ToLocalAdmin(user.PasswordHash);
                    BusinessManager.Instance.LocalManagers.Update(localAdmin);

                    return RedirectToAction("Index", "GlobalAdmin");
                }
            }

            return View("FullEdit", model);
        }

        public ActionResult Billing()
        {
            return View();
        }

        public ActionResult GenerateBilling(GenerateBillingViewModel model)
        {
            var user = User.Identity.GetApplicationUser();
            var localAdmin = BusinessManager.Instance.LocalManagers.FindAll().SingleOrDefault(l => l.Email == user.Email);
            var parking = BusinessManager.Instance.Parkings.FindAll().SingleOrDefault(p => p.LocalManager.CPF == localAdmin.CPF);
            var start = DateTime.Parse(model.StartDateTime);
            var end = DateTime.Parse(model.EndDateTime);
            var billings = BillingHelper.GetBillingsForParking(parking.CNPJ, start, end);

            return PartialView("_BillingPartial", billings);
        }
    }
}