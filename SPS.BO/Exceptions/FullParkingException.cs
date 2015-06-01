using System;

namespace SPS.BO.Exceptions
{
    public class FullParkingException : Exception
    {
        public FullParkingException()
            : base()
        {
        }

        public FullParkingException(string message)
            : base(message)
        {
        }

        public FullParkingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
