using System;
using System.Text;

namespace SPS.Raspberry.Core.MFRC522
{
    public sealed class TagUid
    {
        private const int UidLength = 4;

        public byte Bcc { get; private set; }

        public byte[] Bytes { get; private set; }

        public byte[] FullUid { get; private set; }

        public bool IsValid { get; private set; }

        internal TagUid(byte[] uid)
        {
            FullUid = uid;
            Bcc = uid[UidLength];
            Bytes = new byte[UidLength];
            Array.Copy(FullUid, 0, Bytes, 0, UidLength);

            for (int i = 0; i < UidLength; i++)
            {
                if (Bytes[i] != 0x00)
                {
                    IsValid = true;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TagUid))
            {
                return false;
            }

            var uidWrapper = (TagUid)obj;

            for (int i = 0; i < 5; i++)
            {
                if (FullUid[i] != uidWrapper.FullUid[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int uid = 0;

            for (int i = 0; i < UidLength; i++)
            {
                uid |= Bytes[i] << (i * 8);
            }

            return uid;
        }

        public override string ToString()
        {
            StringBuilder hex = new StringBuilder(UidLength * 2);

            for (int i = 0; i < UidLength - 1; i++)
            {
                hex.AppendFormat("{0:X2} ", Bytes[i]);
            }

            hex.AppendFormat("{0:X2}", Bytes[UidLength - 1]);
            return hex.ToString();
        }
    }
}
