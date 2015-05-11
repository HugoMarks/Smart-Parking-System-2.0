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
    public class GlobalAdminController : Controller
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

        private static bool IsLogged = false;

        // GET: GlobalAdmin
        public ActionResult Index()
        {
            if (!IsLogged)
            {
                IsLogged = false;
                return View("Login");
            }

            IsLogged = false;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(RootUserViewModel rootUser)
        {
            var user = BusinessManager.Instance.GlobalManagers.FindAll().Where(gm => gm.CPF == rootUser.CPF).FirstOrDefault();

            if (user == null)
            {
                ModelState.AddModelError("", "Esse CPF não tem permissão para acessar essa página.");
                IsLogged = false;
                return View(rootUser);
            }

            if (!SPS.Security.TokenGeneratorService.IsTokenValid(user.Password, rootUser.Token))
            {
                ModelState.AddModelError("", "Token inválido ou expirado!");
                IsLogged = false;
                return View(rootUser);
            }

            HttpContext.GetOwinContext().Authentication.SignOut();
            IsLogged = true;
            return View("Index");
        }

        //
        // POST: /Account/RegisterLocalManager
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterLocalManager(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = model.ToApplicationUser(UserType.LocalAdmin);
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    LocalManager client = model.ToUser<LocalManager>(user.PasswordHash);

                    BusinessManager.Instance.LocalManagers.Add(client);

                    return RedirectToLocal(Url.Action("Index", "LocalAdmin"));
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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}