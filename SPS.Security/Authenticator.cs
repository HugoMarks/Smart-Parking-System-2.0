﻿using Org.BouncyCastle.Crypto.Prng;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SPS.Security
{
    public abstract class Authenticator
    {
        private static readonly ThreadedSeedGenerator Random = new ThreadedSeedGenerator();    // Is Thread-Safe
        private static readonly int KeyLength = 16;
        private static readonly string AvailableKeyChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        public static string GenerateKey()
        {
            var keyChars = new char[KeyLength];
            for (int i = 0; i < keyChars.Length; i++)
            {
                keyChars[i] = AvailableKeyChars[RandomInt(AvailableKeyChars.Length)];
            }
            return new String(keyChars);
        }

        protected string GetCodeInternal(string secret, ulong challengeValue)
        {
            ulong chlg = challengeValue;
            byte[] challenge = new byte[8];
            for (int j = 7; j >= 0; j--)
            {
                challenge[j] = (byte)((int)chlg & 0xff);
                chlg >>= 8;
            }

            var key = Base32Encoding.ToBytes(secret);
            for (int i = secret.Length; i < key.Length; i++)
            {
                key[i] = 0;
            }

            HMACSHA1 mac = new HMACSHA1(key);
            var hash = mac.ComputeHash(challenge);

            int offset = hash[hash.Length - 1] & 0xf;

            int truncatedHash = 0;
            for (int j = 0; j < 4; j++)
            {
                truncatedHash <<= 8;
                truncatedHash |= hash[offset + j];
            }

            truncatedHash &= 0x7FFFFFFF;
            truncatedHash %= 1000000;

            string code = truncatedHash.ToString();
            return code.PadLeft(6, '0');
        }

        protected bool ConstantTimeEquals(string a, string b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;

            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)a[i] ^ (uint)b[i];
            }

            return diff == 0;
        }

        protected static int RandomInt(int max)
        {
            var randomBytes = Random.GenerateSeed(4, false);

            return Math.Abs((int)BitConverter.ToUInt32(randomBytes, 0) % max);
        }


    }
}
