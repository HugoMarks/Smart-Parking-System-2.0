using System;
using System.Diagnostics;
using System.Threading;
using Windows.Devices.Gpio;

namespace SPS.Raspberry.Core.UltrasonicSensor
{
    public class UltrasonicDistanceSensor
    {
        ///<summary>
        ///Gets the default Gpio Controller.
        ///</summary>
        private readonly GpioController DefaultGpioController = GpioController.GetDefault();

        private GpioPin _inPin;
        private GpioPin _outPin;

        public UltrasonicDistanceSensor(int trigger, int echo)
        {
            _inPin = DefaultGpioController.OpenPin(echo);
            _outPin = DefaultGpioController.OpenPin(trigger);
            _inPin.SetDriveMode(GpioPinDriveMode.Input);
            _outPin.SetDriveMode(GpioPinDriveMode.Output);
            _outPin.Write(GpioPinValue.Low);
        }

        public double GetDistance()
        {
            var mre = new ManualResetEventSlim(false);

            //Send a 1.5s pulse to start the measurement
            _outPin.Write(GpioPinValue.High);
            mre.Wait(TimeSpan.FromSeconds(3));
            _outPin.Write(GpioPinValue.Low);

            var time = PulseIn();
            // multiply by speed of sound in milliseconds (34000) divided by 2 (cause pulse make rountrip)
            var distance = time * 17000;

            return distance;
        }

        private double PulseIn()
        {
            var sw = new Stopwatch();
            var sw_timeout = new Stopwatch();
            var timeout = 500;

            sw_timeout.Start();

            // Wait for pulse
            while (_inPin.Read() != GpioPinValue.High)
            {
                if (sw_timeout.ElapsedMilliseconds > timeout)
                    return 3.5;
            }

            sw.Start();

            // Wait for pulse end
            while (_inPin.Read() == GpioPinValue.High)
            {
                if (sw_timeout.ElapsedMilliseconds > timeout)
                    return 3.4;
            }

            sw.Stop();

            return sw.Elapsed.TotalSeconds;
        }
    }
}
