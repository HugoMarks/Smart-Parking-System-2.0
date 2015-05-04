using SPS.Security.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPS.Security
{
    public static class TokenGeneratorService
    {
        public static string GenerateToken(string key, int intervalSeconds = 10)
        {
            var authenticator = new TimeAuthenticator(intervalSeconds: intervalSeconds);
            var keyBytes = StringHelper.GetBytes(key);
            var base32Key = Base32Helper.ToBase32String(keyBytes);
            
            return authenticator.GetCode(base32Key);
        }

        public static bool IsTokenValid(string key, string token, int intervalSeconds = 10)
        {
            var authenticator = new TimeAuthenticator(intervalSeconds: intervalSeconds);
            var keyBytes = StringHelper.GetBytes(key);
            var base32Key = Base32Helper.ToBase32String(keyBytes);

            return authenticator.CheckCode(base32Key, token);
        }
    }
}