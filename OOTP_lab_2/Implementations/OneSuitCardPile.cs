using System;
using System.Collections;
using System.Collections.Generic;
using OOTP_lab_2.Abstractions;
using OOTP_lab_2.Exceptions;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Implementations
{
    public class OneSuitCardPile : IOneSuitCardPile
    {
        private readonly IUniqueCardPile _wrappedCardPile;

        public CardSuit Suit { get; }

        public int MaxCardsInPile => _wrappedCardPile.MaxCardsInPile;

        public int Count => _wrappedCardPile.Count;

        public bool IsReadOnly => false;

        public OneSuitCardPile(CardSuit suit, IUniqueCardPile wrappedCardPile)
        {
            if (wrappedCardPile == null)
            {
                throw new ArgumentNullException(nameof(wrappedCardPile));
            }

            _wrappedCardPile = wrappedCardPile;
            Suit = suit;
        }

        public void Add(Card item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (item.Suit != Suit)
            {
                throw new IncompatibleCardSuitException(Suit, item.Suit);
            }

            _wrappedCardPile.Add(item);
        }

        public void Clear() => _wrappedCardPile.Clear();

        public bool Contains(Card item) => _wrappedCardPile.Contains(item);

        public void CopyTo(Card[] array, int arrayIndex) => _wrappedCardPile.CopyTo(array, arrayIndex);

        public bool Remove(Card item) => _wrappedCardPile.Remove(item);

        public IEnumerator<Card> GetEnumerator() => _wrappedCardPile.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _wrappedCardPile.GetEnumerator();
        }
    }
}
