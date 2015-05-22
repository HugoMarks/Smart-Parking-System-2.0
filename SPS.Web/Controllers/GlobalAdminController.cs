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
using SPS.Security;

namespace SPS.Web.Controllers
{
    public class GlobalAdminController : Controller
    {
        // This method is private because only authenticated users can see this view.
        public ActionResult Index()
        {
            if (BusinessManager.Instance.GlobalManagers.FindAll().Count == 0)
            {
                ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                ApplicationUser user = new ApplicationUser
                {
                    Email = "ricardo@sps.com",
                    FirstName = "Ricardo",
                    LastName = "Souza",
                    EmailConfirmed = true,
                    UserName = "ricardo@sps.com",
                    PhoneNumber = "(19) 99856-0989",
                    UserType = UserType.GlobalAdmin
                };

                userManager.Create(user, "Ricardo12__");

                string cpf = "000.000.000-00";
                GlobalManager globalManager = new GlobalManager
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CPF = cpf,
                    Email = user.Email,
                    Password = user.PasswordHash,
                    Telephone = user.PhoneNumber,
                    TokenHash = HashServices.HashPassword("547458", cpf),
                    StreetNumber = 123
                };

                BusinessManager.Instance.GlobalManagers.Add(globalManager);
            }

            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        //
        // GET: /GlobalAdmin/Login
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /GlobalAdmin/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(RootUserViewModel rootUser)
        {
            var user = BusinessManager.Instance.GlobalManagers.FindAll().SingleOrDefault(gm => gm.CPF == rootUser.CPF);

            if (user == null)
            {
                ModelState.AddModelError("", "Esse CPF não tem permissão para acessar essa página.");
                return View(rootUser);
            }

            if (!SPS.Security.TokenGeneratorService.IsTokenValid(user.TokenHash, rootUser.Token))
            {
                ModelState.AddModelError("", "Token inválido ou expirado!");
                return View(rootUser);
            }

            var email = (string)TempData["GlobalAdminEmail"];
            var password = (string)TempData["GlobalAdminPassword"];

            await SignInAsync(email, password);
            return RedirectToAction("Index");
        }

        public ActionResult Edit()
        {
            var user = User.Identity.GetApplicationUser();
            var globalAdmin = BusinessManager.Instance.GlobalManagers.FindAll().SingleOrDefault(g => g.Email == user.Email);
            var model = globalAdmin.ToEditGlobalAdminViewModel();

            return View(model);
        }

        public async Task<ActionResult> SaveChanges(EditGlobalAdminViewModel model)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByEmailAsync(model.Email);
                bool error = false;
                string tokenHash = null;

                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    var result = userManager.ChangePassword(user.Id, model.Password, model.NewPassword);

                    if (!result.Succeeded)
                    {
                        ModelState["Password"].Errors.Add("Senha incorreta");
                        error = true;
                    }
                }

                if (!string.IsNullOrEmpty(model.Token))
                {
                    if (string.IsNullOrEmpty(model.Password))
                    {
                        ModelState["Token"].Errors.Add("O token só pode ser alterado se a senha for digitada");
                        error = true;
                    }
                    else
                    {
                        if (!userManager.CheckPassword(user, model.Password))
                        {
                            ModelState["Password"].Errors.Add("Senha incorreta");
                            error = true;
                        }
                        else
                        {
                            tokenHash = HashServices.HashPassword(model.Token, model.CPF);
                        }
                    }
                }

                if (!error)
                {
                    user = await userManager.FindByEmailAsync(model.Email);

                    if (tokenHash == null)
                    {
                        tokenHash = BusinessManager.Instance.GlobalManagers.FindAll().SingleOrDefault(g => g.Email == user.Email).TokenHash;
                    }

                    GlobalManager globalAdmin = model.ToGlobalManager(user.PasswordHash, tokenHash);

                    BusinessManager.Instance.GlobalManagers.Update(globalAdmin);

                    return RedirectToAction("Index", "GlobalAdmin");
                }
            }

            return View("Edit", model);
        }

        private async Task SignInAsync(string email, string password)
        {
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindAsync(email, password);

            authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, await user.GenerateUserIdentityAsync(userManager));
        }
    }
}