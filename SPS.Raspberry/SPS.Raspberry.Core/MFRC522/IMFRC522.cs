namespace SPS.Raspberry.Core.MFRC522
{
    public interface IMFRC522
    {
        void Reset();

        bool IsTagPresent();

        TagUid ReadUid();

        void HaltTag();

        bool SelectTag(TagUid uid);

        byte[] ReadBlock(byte blockNumber, TagUid uid, byte[] keyA = null, byte[] keyB = null);

        bool WriteBlock(byte blockNumber, TagUid uid, byte[] data, byte[] keyA = null, byte[] keyB = null);
    }
}
