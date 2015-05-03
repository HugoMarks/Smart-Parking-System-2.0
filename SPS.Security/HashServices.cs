using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using SPS.Security.Utilities;
using System;

namespace SPS.Security
{
    public static class HashServices
    {
        public static string HashPassword(string rawPassword, string salt, int hashLength = 10)
        {
            SecureRandom rng = new SecureRandom();
            Pkcs5S2ParametersGenerator kdf = new Pkcs5S2ParametersGenerator();
            byte[] seed = StringHelper.GetBytes(salt);

            kdf.Init(StringHelper.GetBytes(rawPassword), seed, 100);

            byte[] hash = ((KeyParameter)kdf.GenerateDerivedMacParameters(8 * hashLength)).GetKey();

            salt = StringHelper.ToBase64String(seed);

            return StringHelper.ToBase64String(hash);
        }

        public static bool CheckPassword(string rawPassword, string hashPassword, string salt)
        {
            Pkcs5S2ParametersGenerator kdf = new Pkcs5S2ParametersGenerator();
            byte[] seed = StringHelper.FromBase64String(salt);

            kdf.Init(StringHelper.GetBytes(rawPassword), seed, 100);

            byte[] hash = ((KeyParameter)kdf.GenerateDerivedMacParameters(8 * rawPassword.Length)).GetKey();
            byte[] hashToCheck = StringHelper.FromBase64String(hashPassword);

            return AreBytesArraysEqual(hash, hashToCheck);
        }

        private static bool AreBytesArraysEqual(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                    return false;
            }

            return true;
        }
    }
}
