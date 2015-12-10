using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.System.Threading;
using Microsoft.IoT.Devices.Pwm;
using Windows.Devices.Pwm;
using Microsoft.IoT.DeviceCore.Pwm;
using Windows.Foundation;

namespace SPS.Raspberry.Core.ServoMotor
{
    public class ServoMotor : IDisposable
    {
        private const double MinPulseWidth = .544;

        private const double MaxPulseWidth = 2.400;

        private const double DefaultPulseWidth = 1.500;

        private const int DefaultInterval = 20;

        private const int TrimDuration = 2;

        ///<summary>
        ///Gets the default Gpio Controller.
        ///</summary>
        private readonly GpioController DefaultGpioController = GpioController.GetDefault();
        
        private GpioPin _pin;
        private Stopwatch _stopwatch;
        private double _oldAngle;

        public ServoMotor(int pin)
        {
            _pin = DefaultGpioController.OpenPin(pin);
            _stopwatch = Stopwatch.StartNew();
            _oldAngle = 0d;
        }

        public void Init()
        {
            _pin.Write(GpioPinValue.High);
            _pin.SetDriveMode(GpioPinDriveMode.Output);
        }

        public Task RotateAsync(double angle)
        {
            return Task.Factory.StartNew(() =>
            {
                if (_oldAngle == angle)
                {
                    return;
                }

                var value = Map(angle, 0, 180, MinPulseWidth, MaxPulseWidth);
                var iterations = (int)Map(angle, 0, 90, 0, 25);
                
                while (iterations > 0)
                {
                    _pin.Write(GpioPinValue.High);
                    Wait(value);
                    _pin.Write(GpioPinValue.Low);
                    Wait(DefaultInterval - value);
                    iterations--;
                }

                _oldAngle = angle;
            });
        }

        private void Wait(double milliseconds)
        {
            long initialTick = _stopwatch.ElapsedTicks;
            long initialElapsed = _stopwatch.ElapsedMilliseconds;
            double desiredTicks = milliseconds / 1000.0 * Stopwatch.Frequency;
            double finalTick = initialTick + desiredTicks;

            while (_stopwatch.ElapsedTicks < finalTick)
            {

            }
        }

        public void Dispose()
        {
            //_pin.Dispose();
            //_pin = null;
        }

        private double Map(double x, double inMin, double inMax, double outMin, double outMax)
        {
            return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }

        private long MillisecondToTicks(long ms)
        {
            return (ms * Stopwatch.Frequency) / 1000;
        }
    }
}
