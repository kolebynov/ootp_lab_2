using System.Collections.Generic;
using OOTP_lab_2.Abstractions;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Implementations.StringViewProviders
{
    public class CardSuitStringViewProvider : IStringViewProvider<CardSuit>
    {
        private static readonly Dictionary<CardSuit, string> CardSuitStringViews = new Dictionary<CardSuit, string>
        {
            { CardSuit.Club, "\u2663" },
            { CardSuit.Heart, "\u2665" },
            { CardSuit.Diamond, "\u2666" },
            { CardSuit.Spade, "\u2660" },
        };

        public string ToString(CardSuit value) => CardSuitStringViews[value];
    }
}
