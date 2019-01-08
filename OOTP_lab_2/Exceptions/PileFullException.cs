using System;

namespace OOTP_lab_2.Exceptions
{
    public class PileFullException : Exception
    {
        public PileFullException() : base("Pile is full")
        {
        }
    }
}
