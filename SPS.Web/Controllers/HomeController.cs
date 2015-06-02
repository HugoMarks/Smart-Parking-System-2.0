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
using System.ServiceModel;


namespace SPS.Web.Controllers
{
    public class HomeController : Controller
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

        public ActionResult Index()
        {
            if (BusinessManager.Instance.GlobalManagers.FindAll().Count == 0)
            {
                //cria um usuário de sistema.
                ApplicationUserManager systemUserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                ApplicationUser systemUser = new ApplicationUser
                {
                    Email = "sps.smartparkingsystem@gmail.com",
                    FirstName = "Smart Parking ",
                    LastName = "System",
                    EmailConfirmed = true,
                    UserName = "sps.smartparkingsystem@gmail.com",
                    PhoneNumber = "(00) 00000-0000",
                    UserType = UserType.Client
                };

                systemUserManager.Create(systemUser, "System12__");
            }

             

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        // GET: /Home/Contact
        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // POST: /Home/Contact
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = UserManager.FindByEmail("sps@engineer.com");

                string message = "<br/>Contato criado por: " + model.Name + ".<br/>" +
                                 "Assunto: " + model.Subject + ".<br/>" +
                                 "Telefone: " + model.PhoneNumber + " <br/>" +
                                 "Email: " + model.Email + " <br/>" +
                                 "Mensagem: " + model.Message;

                await UserManager.SendEmailAsync(user.Id, "Contato - " + model.Subject, message);

                return RedirectToAction("Index", "Home");  
            }

            // Se chegamos até aqui e houver alguma falha, exiba novamente o formulário
            return View(model);
        }
    }
}