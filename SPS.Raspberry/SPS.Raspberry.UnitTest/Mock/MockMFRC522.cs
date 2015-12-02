using SPS.Raspberry.Core.MFRC522;
using System;
using System.Reflection;

namespace SPS.Raspberry.UnitTest.Mock
{
    public class MockMFRC522 : IMFRC522
    {
        private TagUid _tag;

        public void SetExptectedUid(byte[] uid)
        {
            var type = typeof(TagUid);
            var tagUid = Activator.CreateInstance(typeof(TagUid), uid);

            if (tagUid != null)
            {

            }
        }

        public void HaltTag()
        {

        }

        public bool IsTagPresent()
        {
            return true;
        }

        public byte[] ReadBlock(byte blockNumber, TagUid uid, byte[] keyA = null, byte[] keyB = null)
        {
            return null;
        }

        public TagUid ReadUid()
        {
            return _tag;
        }

        public void Reset()
        {

        }

        public bool SelectTag(TagUid uid)
        {
            return true;
        }

        public bool WriteBlock(byte blockNumber, TagUid uid, byte[] data, byte[] keyA = null, byte[] keyB = null)
        {
            return true;
        }
    }
}
