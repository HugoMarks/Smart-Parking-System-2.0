using Microsoft.CSharp.RuntimeBinder;
using SPS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPS.Web.Controllers
{
    public class TokenGeneratorController : Controller
    {
        //
        // GET: TokenGenerator
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: TokenGenerator/GeneratedToken
        public ActionResult GeneratedToken()
        {
            try
            {
                var token = ViewBag.GeneratedToken;
            }
            catch (RuntimeBinderException)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            }

            return View();
        }

        //
        // POST: TokenGenerator/GenerateToken
        [HttpPost]
        public ActionResult GenerateToken(TokenGeneratorViewModel model)
        {
            var token = Security.TokenGeneratorService.GenerateToken(model.CPF);

            ViewBag.GeneratedToken = token;
            return View("GeneratedToken");
        }
    }
}