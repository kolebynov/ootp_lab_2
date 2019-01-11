using OOTP_lab_2.Abstractions;
using System;
using System.Collections.Generic;
using OOTP_lab_2.Objects;
using System.Collections;
using System.Linq;

namespace OOTP_lab_2.Implementations
{
    public class CardDeck : ICardDeck
    {
        private readonly List<Card> _cards;
        private readonly Random _random;

        private static readonly List<Card> DefaultCards;

        public CardDeck()
        {
            _cards = new List<Card>(DefaultCards);
            _random = new Random();
        }

        static CardDeck()
        {
            DefaultCards = new List<Card>();

            foreach (CardNumber cardNumber in Enum.GetValues(typeof(CardNumber)))
            {
                foreach (CardSuit cardSuit in Enum.GetValues(typeof(CardSuit)))
                {
                    DefaultCards.Add(new Card(cardSuit, cardNumber));
                }
            }
        }

        public void Shuffle()
        {
            _cards.Clear();
            List<Card> cardsForRand = new List<Card>(DefaultCards);

            while (cardsForRand.Any())
            {
                int index = _random.Next(0, cardsForRand.Count);
                _cards.Add(cardsForRand[index]);
                cardsForRand.RemoveAt(index);
            }
        }

        public IEnumerator<Card> GetEnumerator() => _cards.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
