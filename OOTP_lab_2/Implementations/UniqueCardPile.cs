using System;
using System.Collections;
using System.Collections.Generic;
using OOTP_lab_2.Abstractions;
using OOTP_lab_2.Exceptions;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Implementations
{
    public class UniqueCardPile : IUniqueCardPile
    {
        private readonly List<Card> _cards;

        public UniqueCardPile(int maxCardsInPile)
        {
            MaxCardsInPile = maxCardsInPile;

            _cards = new List<Card>(MaxCardsInPile);
        }

        public int Count => _cards.Count;

        public Card Peek()
        {
            if (Count == 0)
            {
                throw new PileEmptyException();
            }

            return _cards[_cards.Count - 1];
        }

        public Card Pop()
        {
            if (Count == 0)
            {
                throw new PileEmptyException();
            }

            var card = _cards[_cards.Count - 1];
            _cards.RemoveAt(_cards.Count - 1);

            return card;
        }

        public int MaxCardsInPile { get; }

        public IEnumerator<Card> GetEnumerator() => _cards.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Push(Card item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (Count == MaxCardsInPile)
            {
                throw new PileFullException();
            }

            if (Contains(item))
            {
                throw new CardExistsInPileException(item);
            }

            _cards.Add(item);
        }

        public void Clear() => _cards.Clear();

        public bool Remove(Card item) => _cards.Remove(item);

        private bool Contains(Card item) => _cards.Contains(item);
    }
}
