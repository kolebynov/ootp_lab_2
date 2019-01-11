using System;

namespace OOTP_lab_2.Exceptions
{
    public class CantDoStepException : Exception
    {
        public CantDoStepException()
        {
        }

        public CantDoStepException(string message) : base(message)
        {
        }

        public CantDoStepException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
