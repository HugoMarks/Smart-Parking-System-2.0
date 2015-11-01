namespace SPS.Raspberry.Core.MFRC522
{
    internal static class PiccResponses
    {
        public const ushort AnswerToRequest = 0x0004;

        public const byte SelectAcknowledge = 0x08;

        public const byte Acknowledge = 0x0A;
    }
}
