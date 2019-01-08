using System;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Exceptions
{
    public class IncompatibleCardSuitException : Exception
    {
        public CardSuit ExpectedSuit { get; }

        public CardSuit ActualSuit { get; }

        public IncompatibleCardSuitException(CardSuit expectedSuit, CardSuit actualSuit) 
            : base($"Card has incompatible suit. Expected: {expectedSuit}, actual: {actualSuit}")
        {
            ExpectedSuit = expectedSuit;
            ActualSuit = actualSuit;
        }
    }
}
