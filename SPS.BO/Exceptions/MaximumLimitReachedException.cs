using System;

namespace SPS.BO.Exceptions
{
    public class MaximumLimitReachedException : Exception
    {
        public MaximumLimitReachedException()
            : base()
        {
        }

        public MaximumLimitReachedException(string message)
            : base(message)
        {
        }

        public MaximumLimitReachedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
