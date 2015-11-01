using System;

namespace SPS.Raspberry.Core.MFRC522
{
    public sealed class TagUid
    {
        public byte Bcc { get; private set; }

        public byte[] Bytes { get; private set; }

        public byte[] FullUid { get; private set; }

        public bool IsValid { get; private set; }

        internal TagUid(byte[] uid)
        {
            FullUid = uid;
            Bcc = uid[4];
            Bytes = new byte[4];
            Array.Copy(FullUid, 0, Bytes, 0, 4);

            foreach (var b in Bytes)
            {
                if (b != 0x00)
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

            for (int i = 0; i < 4; i++)
            {
                uid |= Bytes[i] << (i * 8);
            }

            return uid;
        }

        public override string ToString()
        {
            var formatString = "x" + (Bytes.Length * 2);

            return GetHashCode().ToString(formatString);
        }
    }
}
