using System;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Exceptions
{
    public class IncompatibleCardNumberException : Exception
    {
        public CardNumber ExpectedNumber { get; }

        public CardNumber ActualNumber { get; }

        public IncompatibleCardNumberException(CardNumber expectedNumber, CardNumber actualNumber)
            : base($"Card has incompatible number. Expected: {expectedNumber}, actual: {actualNumber}")
        {
            ExpectedNumber = expectedNumber;
            ActualNumber = actualNumber;
        }
    }
}
