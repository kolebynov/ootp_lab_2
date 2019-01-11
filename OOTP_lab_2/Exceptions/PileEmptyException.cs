using System;

namespace OOTP_lab_2.Exceptions
{
    public class PileEmptyException : Exception
    {
        public PileEmptyException() : base("Pile is empty")
        {
        }
    }
}
