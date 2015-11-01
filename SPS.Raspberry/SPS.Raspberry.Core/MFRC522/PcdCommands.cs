namespace SPS.Raspberry.Core.MFRC522
{
    internal static class PcdCommands
    {
        public const byte Idle = 0x00;

        public const byte Authenticate = 0x0E;

        public const byte Transceive = 0x0C;
    }
}
