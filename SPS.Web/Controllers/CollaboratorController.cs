using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using SPS.Web.Models;
using SPS.BO;
using SPS.Web.Services;
using System.Web.Script.Serialization;
using System.Net;
using SPS.Repository;
using SPS.Web.Extensions;
using SPS.Model;

namespace SPS.Web.Controllers
{
    public class CollaboratorController : Controller
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

        // GET: Collaborator
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var user = User.Identity.GetApplicationUser();

                if (user.UserType != Models.UserType.Collaborator)
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

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ValidateAjax]
        public async Task<ActionResult> Register(RegisterCollaboratorViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = model.ToApplicationUser(UserType.Collaborator);
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    Collaborator collaborator = model.ToCollaborator(user.PasswordHash);

                    BusinessManager.Instance.Collaborators.Add(collaborator);

                    // Enviar um email com este link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                    await UserManager.SendEmailAsync(user.Id, "Confirmar sua conta Smart Parking System", "Olá, " +
                        model.FirstName + "!<br/> Para começar a utilizar sua nova conta Smart Parking System,<br/> clique <a href=\"" +
                        callbackUrl + "\">aqui</a>");

                    ViewBag.EmailSent = model.Email;

                    return RedirectToAction("Index", "LocalAdmin");
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

        [HttpPost]
        public ActionResult FullEdit(FormCollection form)
        {
            var selectedId = form["CollaboratorsDropDownList"];
            var collaborator = BusinessManager.Instance.Collaborators.Find(int.Parse(selectedId));
            var model = collaborator.ToFullEditCollaboratorViewModel();

            return View(model);
        }

        private ActionResult FullEdit(FullEditCollaboratorViewModel model)
        {
            return View(model);
        }

        public ActionResult Edit()
        {
            var user = User.Identity.GetApplicationUser();
            var collaborator = BusinessManager.Instance.Collaborators.FindAll().Where(c => c.Email == user.Email).FirstOrDefault();
            var model = collaborator.ToFullEditCollaboratorViewModel();

            return View(model);
        }

        private ActionResult Edit(EditCollaboratorViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveEditChanges(EditCollaboratorViewModel model)
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

                user = await UserManager.FindByEmailAsync(model.Email);

                if(!error)
                {
                    Collaborator collaborator = model.ToCollaborator(user.PasswordHash);

                    BusinessManager.Instance.Collaborators.Update(collaborator);

                    return RedirectToAction("Index", "Collaborator");
                }
            }

            return View("Edit", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveFullEditChanges(FullEditCollaboratorViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindByEmailAsync(model.Email);
                bool error = false;

                if (string.IsNullOrEmpty(model.NewPassword))
                {
                    var result = UserManager.ChangePassword(user.Id, model.Password, model.NewPassword);

                    if (!result.Succeeded)
                    {
                        ModelState["Password"].Errors.Add("Senha incorreta");
                        error = true;
                    }
                }

                user = await UserManager.FindByEmailAsync(model.Email);

                if (!error)
                {
                    Collaborator collaborator = model.ToCollaborator(user.PasswordHash);

                    BusinessManager.Instance.Collaborators.Update(collaborator);

                    return RedirectToAction("Index", "LocalAdmin");
                }
            }

            return View("FullEdit", model);
        }

        public PartialViewResult GetCollaborators()
        {
            return PartialView("_CollaboratorsListPartial");
        }
    }
}