using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OOTP_lab_2.Abstractions;
using OOTP_lab_2.Exceptions;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Implementations
{
    public class SequentialOneSuitCardPile : ISequentialOneSuitCardPile
    {
        private readonly IOneSuitCardPile _wrappedCardPile;

        private readonly CardNumber[] _cardNumbersSequence;

        public IEnumerable<CardNumber> CardNumbersSequence => _cardNumbersSequence;

        public int MaxCardsInPile => Math.Min(_cardNumbersSequence.Length, _wrappedCardPile.MaxCardsInPile);

        public CardSuit Suit => _wrappedCardPile.Suit;

        public int Count => _wrappedCardPile.Count;

        public Card Peek() => _wrappedCardPile.Peek();

        public Card Pop() => _wrappedCardPile.Pop();

        public SequentialOneSuitCardPile(
            IEnumerable<CardNumber> cardNumbersSequence,
            IOneSuitCardPile wrappedCardPile)
        {
            if (cardNumbersSequence == null)
            {
                throw new ArgumentNullException(nameof(cardNumbersSequence));
            }

            if (wrappedCardPile == null)
            {
                throw new ArgumentNullException(nameof(wrappedCardPile));
            }

            _wrappedCardPile = wrappedCardPile;
            _cardNumbersSequence = cardNumbersSequence.ToArray();
        }

        public void Push(Card item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            int nextCardNumberIndex =
                Count == 0 ? 0 : Array.IndexOf(_cardNumbersSequence, _wrappedCardPile.Last().Number) + 1;

            if (nextCardNumberIndex == _cardNumbersSequence.Length)
            {
                throw new PileFullException();
            }

            if (item.Number != _cardNumbersSequence[nextCardNumberIndex])
            {
                throw new IncompatibleCardNumberException(_cardNumbersSequence[nextCardNumberIndex], item.Number);
            }

            _wrappedCardPile.Push(item);
        }

        public void Clear() => _wrappedCardPile.Clear();

        public bool Remove(Card item) => _wrappedCardPile.Remove(item);

        public IEnumerator<Card> GetEnumerator() => _wrappedCardPile.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _wrappedCardPile.GetEnumerator();
    }
}
