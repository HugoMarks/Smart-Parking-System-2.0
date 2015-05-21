using System;

namespace SPS.BO.Exceptions
{
    public class UniqueKeyViolationException : Exception
    {
        public UniqueKeyViolationException()
            : base()
        {
        }

        public UniqueKeyViolationException(string message)
            : base(message)
        {
        }

        public UniqueKeyViolationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
