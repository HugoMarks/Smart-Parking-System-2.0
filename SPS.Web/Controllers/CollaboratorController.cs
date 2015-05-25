﻿using System;
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
using SPS.BO.Exceptions;

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
        public async Task<ActionResult> Register(RegisterCollaboratorViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = model.ToApplicationUser(UserType.Collaborator);
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    Collaborator collaborator = model.ToCollaborator(user.PasswordHash);
                    ApplicationUser localUser = User.Identity.GetApplicationUser();
                    LocalManager localManager = BusinessManager.Instance.LocalManagers.FindAll().SingleOrDefault(l => localUser.Email == l.Email);

                    try
                    {
                        collaborator.Parking = BusinessManager.Instance.Parkings.FindAll().SingleOrDefault(p => p.LocalManager.CPF == localManager.CPF);
                        BusinessManager.Instance.Collaborators.Add(collaborator);
                    }
                    catch (UniqueKeyViolationException ex)
                    {
                        ModelState["CPF"].Errors.Add(ex.Message);
                    }

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
            var collaborator = BusinessManager.Instance.Collaborators.Find(selectedId);
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

                if(!error)
                {
                    user = await UserManager.FindByEmailAsync(model.Email);

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

        [HttpPost]
        public ActionResult Delete(string email)
        {
            var user = UserManager.FindByEmail(email);

            if (user != null)
            {
                var collaborator = BusinessManager.Instance.Collaborators.FindAll().SingleOrDefault(c => c.Email == user.Email);

                if (collaborator != null)
                {
                    BusinessManager.Instance.Collaborators.Remove(collaborator);
                    UserManager.Delete(user);
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AttachTag(AttachTagViewModel model)
        {
            var client = BusinessManager.Instance.MontlyClients.FindAll().SingleOrDefault(u => u.Email == model.UserEmail);
            var user = User.Identity.GetApplicationUser();
            var collaborator = BusinessManager.Instance.Collaborators.FindAll().SingleOrDefault(c => c.Email == user.Email);

            client.Parking = collaborator.Parking;
            BusinessManager.Instance.MontlyClients.Update(client);
            BusinessManager.Instance.Tags.Add(new Tag { Id = model.TagId.ToUpper(), Client = client });

            return RedirectToAction("Index", "Collaborator");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> RequestNewTag()
        {
            try
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = User.Identity.GetApplicationUser();
                var client = BusinessManager.Instance.MontlyClients.FindAll().SingleOrDefault(c => c.Email == user.Email);
                var parking = BusinessManager.Instance.Parkings.Find(client.Parking.CNPJ);

                if (parking == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                var localManagerUser = userManager.FindByEmail(parking.LocalManager.Email);
                var message = @"Olá, {0}! {1} {2} gostaria de solicitar uma nova tag!<br/>                                
                                Por favor, responda o(a) cliente o mais rápido possível!
                                <br/><br/>
                                Dados do(a) cliente para contato:
                                <br />
                                Telefone: {3}
                                <br />
                                Email: {4}
                                <br />
                                <br />
                                <b>Equipe Smart Parking System®</b>";

                message = string.Format(message, parking.LocalManager.FirstName, client.FirstName, client.LastName, client.Telephone, client.Email);

                await userManager.SendEmailAsync(localManagerUser.Id, "Requisição de Nova Tag", message);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
    }
}