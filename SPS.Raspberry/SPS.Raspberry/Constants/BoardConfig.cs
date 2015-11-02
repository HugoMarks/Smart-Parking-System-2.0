namespace SPS.Raspberry
{
    /// <summary>
    /// Constants values for board configuration.
    /// </summary>
    public static class BoardConfig
    {
        public const int MFRC522CmdPin = 22;

        public const int MFRC522ResetPin = 23;

        public const int MFRC522SpiSelectChipLine = 0;

        public const string MFRC522SpiControllerName = "SPI0";
    }
}
