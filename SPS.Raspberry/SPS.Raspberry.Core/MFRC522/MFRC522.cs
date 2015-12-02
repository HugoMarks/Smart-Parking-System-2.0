using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Gpio;
using Windows.Devices.Spi;

namespace SPS.Raspberry.Core.MFRC522
{
    ///<summary>
    ///A MFRC522 RFID reader controller.
    ///</summary>
    public sealed class MFRC522 : IMFRC522
    {
        #region Const fields

        /// <summary>
        /// Gets the max length of a block, in bytes.
        /// </summary>
        private const int MaxBlockLength = 16;

        /// <summary>
        /// Gets the number of bytes for an authentication package.
        /// </summary>
        private const int AuthenticationPacketLength = 12;

        ///<summary>
        ///Gets the Spi device clock frequency.
        ///</summary>
        private const int SpiClockFrequency = 10000000; //10Mhz

        ///<summary>
        ///Gets the default number of milliseconds a thread will sleep.
        ///</summary>
        private const int DefaultThreadSleepTime = 50;

        #endregion

        #region Readonly fields

        ///<summary>
        ///Gets the default Gpio Controller.
        ///</summary>
        private readonly GpioController DefaultGpioController = GpioController.GetDefault();

        #endregion

        #region Fields

        private SpiDevice _spiDevice;
        private GpioPin _commandPin;
        private GpioPin _resetPin;

        #endregion

        #region Constructor

        ///<summary>
        ///Initializes a new instance of the <see cref="MFRC522"/> class wih the
        ///specified command pin and reset pin numbers.
        ///</summary>
        ///<param name="commandPin"></param>
        ///<param name="resetPin"></param>
        public MFRC522(int commandPin, int resetPin)
        {
            if (DefaultGpioController == null)
            {
                throw new InvalidOperationException("No GpioController found in this device.");
            }

            _commandPin = DefaultGpioController.OpenPin(commandPin);
            _resetPin = DefaultGpioController.OpenPin(resetPin);
        }

        #endregion

        #region Public methods

        ///<summary>
        ///Initializes the component asynchronously.
        ///</summary>
        ///<param name="spiControllerName">The name of the Spi controller.</param>
        ///<param name="spiChipLine">The spi chip select line number.</param>
        ///<returns><see cref="IAsyncAction"/></returns>
        public async Task InitAsync(string spiControllerName, int spiChipLine)
        {
            var settings = new SpiConnectionSettings(spiChipLine)
            {
                Mode = SpiMode.Mode3,
                ClockFrequency = SpiClockFrequency
            };

            string spis = SpiDevice.GetDeviceSelector(spiControllerName);
            var devicesInfo = await DeviceInformation.FindAllAsync(spis);

            if (devicesInfo.Count == 0)
            {
                throw new ArgumentException("No device information for the provided parameters.");
            }

            _spiDevice = await SpiDevice.FromIdAsync(devicesInfo[0].Id, settings);
            Reset();
        }


        /// <summary>
        /// Resets the MFRC522 reader.
        /// </summary>
        public void Reset()
        {
            _resetPin.Write(GpioPinValue.Low);
            _resetPin.Write(GpioPinValue.High);

            //Force 100% ASK modulation
            WriteRegister(MFRC522Registers.TxAsk, 0x40);
            //Set CRC to 0x6363
            WriteRegister(MFRC522Registers.Mode, 0x3D);
            //Enable antenna
            SetRegisterBits(MFRC522Registers.TxControl, 0x03);
        }

        /// <summary>
        /// Gets if a MFRC522 tag is being read by the MFRC522 reader.
        /// </summary>
        /// <returns></returns>
        public bool IsTagPresent()
        {
            //Enable short frames
            WriteRegister(MFRC522Registers.BitFraming, 0x07);

            //Transceive the Request command to the tag
            Transceive(false, PiccCommands.Request);

            //Disable short frames
            WriteRegister(MFRC522Registers.BitFraming, 0x00);

            //Check if we found a card
            return GetFifoLevel() == 2 && ReadFromFifoShort() == PiccResponses.AnswerToRequest;
        }

        /// <summary>
        /// Reads the MFRC522 tag information.
        /// </summary>
        /// <returns></returns>
        public TagUid ReadUid()
        {
            //Run the anti-collision loop on the card
            Transceive(false, PiccCommands.Anticollision1, PiccCommands.Anticollision2);

            //Return tag UID from FIFO
            return new TagUid(ReadFromFifo(5));
        }

        /// <summary>
        /// Halts the selected MFRC522 tag.
        /// </summary>
        public void HaltTag()
        {
            //Transceive the Halt command to the tag
            Transceive(false, PiccCommands.Halt1, PiccCommands.Halt2);
        }

        /// <summary>
        /// Selects a MFRC522 tag.
        /// </summary>
        /// <param name="uid">The tag to be selected.</param>
        /// <returns>true if the specified tag was selected; false otherwise.</returns>
        public bool SelectTag(TagUid uid)
        {
            //Send Select command to tag
            var data = new byte[7];

            data[0] = PiccCommands.Select1;
            data[1] = PiccCommands.Select2;

            uid.FullUid.CopyTo(data, 2);
            Transceive(true, data);

            return GetFifoLevel() == 1 && ReadFromFifo() == PiccResponses.SelectAcknowledge;
        }

        /// <summary>
        /// Reads a block of data from the MFRC522 reader.
        /// </summary>
        /// <param name="blockNumber">The number of the block to read data from.</param>
        /// <param name="uid">The MFRC522 tag to read data from.</param>
        /// <param name="keyA">The first key to use while reading.</param>
        /// <param name="keyB">The second key to use while reading.</param>
        /// <returns>The data read.</returns>
        public byte[] ReadBlock(byte blockNumber, TagUid uid, byte[] keyA = null, byte[] keyB = null)
        {
            if (keyA != null)
            {
                Authenticate(PiccCommands.AuthenticateKeyA, blockNumber, uid, keyA);
            }
            else if (keyB != null)
            {
                Authenticate(PiccCommands.AuthenticateKeyB, blockNumber, uid, keyB);
            }
            else
            {
                return null;
            }

            //Read block
            Transceive(true, PiccCommands.Read, blockNumber);

            return ReadFromFifo(MaxBlockLength);
        }

        /// <summary>
        /// Writes a block of data to the MFRC522 reader.
        /// </summary>
        /// <param name="blockNumber">The number of the block to write data to.</param>
        /// <param name="uid">The MFRC522 tag to write data to.</param>
        /// <param name="keyA">The first key to use while writing.</param>
        /// <param name="keyB">The second key to use while writing.</param>
        /// <returns>true if the data was written; false otherwise.</returns>
        public bool WriteBlock(byte blockNumber, TagUid uid, byte[] data, byte[] keyA = null, byte[] keyB = null)
        {
            if (keyA != null)
            {
                Authenticate(PiccCommands.AuthenticateKeyA, blockNumber, uid, keyA);
            }
            else if (keyB != null)
            {
                Authenticate(PiccCommands.AuthenticateKeyB, blockNumber, uid, keyB);
            }
            else
            {
                return false;
            }

            //Write block
            Transceive(true, PiccCommands.Write, blockNumber);

            if (ReadFromFifo() != PiccResponses.Acknowledge)
            {
                return false;
            }

            //Make sure we write only 16 bytes
            var buffer = new byte[MaxBlockLength];

            data.CopyTo(buffer, 0);
            Transceive(true, buffer);

            return ReadFromFifo() == PiccResponses.Acknowledge;
        }

        #endregion

        #region Helpers

        private void WriteRegister(byte register, byte value)
        {
            register <<= 1;
            TransferSpi(new byte[] { register, value });
        }

        private byte ReadRegister(byte register)
        {
            register <<= 1;
            register |= 0x80;

            return TransferSpi(new byte[] { register, 0x00 })[1];
        }

        private byte[] TransferSpi(byte[] writeBuffer)
        {
            var readBuffer = new byte[writeBuffer.Length];

            _commandPin.Write(GpioPinValue.Low);
            _spiDevice.TransferFullDuplex(writeBuffer, readBuffer);
            _commandPin.Write(GpioPinValue.High);

            return readBuffer;
        }

        private byte[] ReadFromFifo(int length)
        {
            var buffer = new byte[length];

            for (int i = 0; i < length; i++)
            {
                buffer[i] = ReadRegister(MFRC522Registers.FifoData);
            }

            return buffer;
        }

        private byte ReadFromFifo()
        {
            return ReadFromFifo(1)[0];
        }

        private void WriteToFifo(params byte[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                WriteRegister(MFRC522Registers.FifoData, values[i]);
            }
        }

        private int GetFifoLevel()
        {
            return ReadRegister(MFRC522Registers.FifoLevel);
        }

        private ushort ReadFromFifoShort()
        {
            var low = ReadRegister(MFRC522Registers.FifoData);
            var high = (ushort)(ReadRegister(MFRC522Registers.FifoData) << 8);

            return (ushort)(high | low);
        }

        private void SetRegisterBits(byte register, byte bits)
        {
            var currentValue = ReadRegister(register);

            WriteRegister(register, (byte)(currentValue | bits));
        }

        private void ClearRegisterBits(byte register, byte bits)
        {
            var currentValue = ReadRegister(register);

            WriteRegister(register, (byte)(currentValue & ~bits));
        }

        private void Transceive(bool enableCrc, params byte[] data)
        {
            if (enableCrc)
            {
                //Enable CRC
                SetRegisterBits(MFRC522Registers.TxMode, 0x80);
                SetRegisterBits(MFRC522Registers.RxMode, 0x80);
            }

            //Put reader in Idle mode
            WriteRegister(MFRC522Registers.Command, PcdCommands.Idle);

            //Clear the FIFO
            SetRegisterBits(MFRC522Registers.FifoLevel, 0x80);

            //Write the data to the FIFO
            WriteToFifo(data);

            //Put reader in Transceive mode and start sending
            WriteRegister(MFRC522Registers.Command, PcdCommands.Transceive);
            SetRegisterBits(MFRC522Registers.BitFraming, 0x80);
            
            //Stop sending
            ClearRegisterBits(MFRC522Registers.BitFraming, 0x80);

            if (enableCrc)
            {
                //Disable CRC
                ClearRegisterBits(MFRC522Registers.TxMode, 0x80);
                ClearRegisterBits(MFRC522Registers.RxMode, 0x80);
            }
        }

        private void Authenticate(byte command, byte blockNumber, TagUid uid, [ReadOnlyArray] byte[] key)
        {
            //Put reader in Idle mode
            WriteRegister(MFRC522Registers.Command, PcdCommands.Idle);
            //Clear the FIFO
            SetRegisterBits(MFRC522Registers.FifoLevel, 0x80);

            //Create Authentication packet
            var data = new byte[AuthenticationPacketLength];

            data[0] = command;
            data[1] = (byte)(blockNumber & 0xFF);
            key.CopyTo(data, 2);
            uid.Bytes.CopyTo(data, 8);
            WriteToFifo(data);
            //Put reader in MfAuthent mode
            WriteRegister(MFRC522Registers.Command, PcdCommands.Authenticate);
        }

        #endregion
    }
}
