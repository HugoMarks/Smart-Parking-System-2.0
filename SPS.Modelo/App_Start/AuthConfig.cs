using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using SPS.Modelo.Models;

namespace SPS.Modelo
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // Para permitir que os usuários deste site façam logon em suas contas a partir de outros sites, como Microsoft, Facebook e Twitter,
            // é necessário atualizar este site. Para obter mais informações, acesse http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
