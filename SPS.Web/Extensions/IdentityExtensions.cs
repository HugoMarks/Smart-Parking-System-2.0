using SPS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Microsoft.AspNet.Identity
{
    public static class IdentityExtensions
    {
        public static ApplicationUser GetApplicationUser(this IIdentity identity)
        {
            return ApplicationDbContext.Create().Users.Find(identity.GetUserId());
        }
    }
}