namespace SPS.Raspberry
{
    /// <summary>
    /// Constants values for board configuration.
    /// </summary>
    public static class BoardConfig
    {
        public const int MFRC522CmdPin = 24;

        public const int MFRC522ResetPin = 22;

        public const int MFRC522SpiSelectChipLine = 0;

        public const string MFRC522SpiControllerName = "SPI0";

        public const int ServoMotorPin = 6;

        public const int UltrasonicSensorTriggerPin = 17;

        public const int UltrasonicSensorEchoPin  = 27;
    }
}
