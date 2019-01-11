using System;
using OOTP_lab_2.Abstractions;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Implementations.StringViewProviders
{
    public class CardStringViewProvider : IStringViewProvider<Card>
    {
        private readonly IStringViewProvider<CardNumber> _cardNumberStringViewProvider;

        private readonly IStringViewProvider<CardSuit> _cardSuitStringViewProvider;

        public CardStringViewProvider(IStringViewProvider<CardNumber> cardNumberStringViewProvider, IStringViewProvider<CardSuit> cardSuitStringViewProvider)
        {
            if (cardNumberStringViewProvider == null)
            {
                throw new ArgumentNullException(nameof(cardNumberStringViewProvider));
            }

            if (cardSuitStringViewProvider == null)
            {
                throw new ArgumentNullException(nameof(cardSuitStringViewProvider));
            }

            _cardNumberStringViewProvider = cardNumberStringViewProvider;
            _cardSuitStringViewProvider = cardSuitStringViewProvider;
        }

        public string ToString(Card value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return
                $"{_cardNumberStringViewProvider.ToString(value.Number)}{_cardSuitStringViewProvider.ToString(value.Suit)}";
        }
    }
}
