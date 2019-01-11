using System.Collections.Generic;
using OOTP_lab_2.Abstractions;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Implementations.StringViewProviders
{
    public class CardNumberStringViewProvider : IStringViewProvider<CardNumber>
    {
        private static readonly Dictionary<CardNumber, string> CardNumberStringViews = new Dictionary<CardNumber, string>
        {
            { CardNumber.Six, "6" },
            { CardNumber.Seven, "7" },
            { CardNumber.Eight, "8" },
            { CardNumber.Nine, "9" },
            { CardNumber.Ten, "10" },
            { CardNumber.Jack, "J" },
            { CardNumber.Queen, "Q" },
            { CardNumber.King, "K" },
            { CardNumber.Ace, "A" }
        };

        public string ToString(CardNumber value) => CardNumberStringViews[value];
    }
}
