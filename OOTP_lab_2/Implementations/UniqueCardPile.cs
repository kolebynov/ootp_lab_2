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

        public bool IsReadOnly => false;

        public int MaxCardsInPile { get; }

        public IEnumerator<Card> GetEnumerator() => _cards.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public virtual void Add(Card item)
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

        public bool Contains(Card item) => _cards.Contains(item);

        public void CopyTo(Card[] array, int arrayIndex) => _cards.CopyTo(array, arrayIndex);

        public bool Remove(Card item) => _cards.Remove(item);
    }
}
