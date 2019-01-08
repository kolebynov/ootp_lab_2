using System;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Exceptions
{
    public class CardExistsInPileException : Exception
    {
        public Card Card { get; }

        public CardExistsInPileException(Card card) : base($"Card \"{card}\" already exists in pile")
        {
            Card = card;
        }
    }
}
