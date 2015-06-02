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
using System.Threading.Tasks;

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
                    Client dailyClient = new Client();

                    parking = model.ToParking();
                    BusinessManager.Instance.Parkings.Add(parking);
                    dailyClient.Email = "daily@" + parking.Name.Replace(" ", "_").ToLower() + ".com";
                    BusinessManager.Instance.Clients.Add(dailyClient);

                    BusinessManager.Instance.Clients.AttachToParking(dailyClient, parking.CNPJ);

                    return RedirectToAction("Index", "GlobalAdmin");
                }

                ModelState["CNPJ"].Errors.Add("Já existe um estacionamento com este CNPJ");
            }

            return View(model);
        }

        
         [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection form)
        {
            //obtem o estacionamento procurado
            var selectedCNPJ = form["ParkingSelectList"];
            var parking = BusinessManager.Instance.Parkings.Find(selectedCNPJ);
            
            //obtem o dono deste estcionamento
            var user = BusinessManager.Instance.LocalManagers.Find(parking.LocalManager.CPF);

             //obtem o index deste administrador para preencher futuramente o combobox
            var localAdmins = BusinessManager.Instance.LocalManagers.FindAll().ToList();
            var index = localAdmins.FindIndex(l => l.Email == user.Email);

             //transforma para model as irformações a serem alteradas
            var model = parking.ToRegisterParkingViewModel();
 
             ViewBag.LocalAdminCPF = user.CPF;

             return View(model);
         }

         public ActionResult EditLocal()
         {
             var user = User.Identity.GetApplicationUser();
             var localAdmins = BusinessManager.Instance.LocalManagers.FindAll().ToList();
             var localAdmin = localAdmins.SingleOrDefault(l => l.Email == user.Email);
             var index = localAdmins.FindIndex(l => l.Email == user.Email);
             var parking = BusinessManager.Instance.Parkings.FindAll().SingleOrDefault(p => p.LocalManager.CPF == localAdmin.CPF);
             var model = parking.ToRegisterParkingViewModel();

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult SaveChangesLocal(RegisterParkingViewModel model)
        {
            if (ModelState.IsValid)
            {
                Parking parking = BusinessManager.Instance.Parkings.Find(model.CNPJ);

                if (parking != null)
                {
                    parking = model.ToParking();
                    BusinessManager.Instance.Parkings.Update(parking);

                    return RedirectToAction("Index", "LocalAdmin");
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
                    Spaces = parking.Spaces.Where(s => s.Status == ParkingSpaceState.Free).Count()
                }
            });
        }

        [HttpPost]
        public async Task<ActionResult> RequestAttach(string cnpj)
        {
            try
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = User.Identity.GetApplicationUser();
                var client = BusinessManager.Instance.Clients.FindAll().SingleOrDefault(c => c.Email == user.Email);
                var parking = BusinessManager.Instance.Parkings.Find(cnpj);

                if (parking == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                var localManagerUser = userManager.FindByEmail(parking.LocalManager.Email);
                var message = @"Olá, {0}! {1} {2} gostaria de fazer parte de nossa rede!<br/>
                                Ele(a) tem interesse no estacionamento ""{3}"" ({4}).<br/>
                                Por favor, responda o(a) cliente o mais rápido possível!
                                <br/><br/>
                                Dados do(a) cliente para contato:
                                <br />
                                Telefone: {5}
                                <br />
                                Email: {6}
                                <br />
                                <br />
                                <b>Equipe Smart Parking System®</b>";

                message = string.Format(message, parking.LocalManager.FirstName, client.FirstName, client.LastName, 
                    parking.Name, parking.CNPJ, client.Telephone, client.Email);

                await userManager.SendEmailAsync(localManagerUser.Id, "Requisição de Vínculo", message);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        [HttpPost]
        public JsonResult GetFreeSpacesCount(string parkingCNPJ)
        {
            var parking = BusinessManager.Instance.Parkings.Find(parkingCNPJ);

            return Json(new { Success = true, Count = parking.Spaces.Count(s => s.Status == ParkingSpaceState.Free) });
        }
    }
}