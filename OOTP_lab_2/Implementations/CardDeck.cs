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
        private const int ShuffleIterations = 5;

        private readonly List<Card> _cards = new List<Card>(DefaultCards);
        private readonly Random _random = new Random(Environment.TickCount);

        private static readonly List<Card> DefaultCards;

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
            List<Card> cardsForRand1 = new List<Card>(DefaultCards);
            List<Card> cardsForRand2 = new List<Card>();

            for (int i = 0; i < ShuffleIterations; ++i)
            {
                cardsForRand2.Clear();

                while (cardsForRand1.Any())
                {
                    int index = _random.Next(0, cardsForRand1.Count);
                    cardsForRand2.Add(cardsForRand1[index]);
                    cardsForRand1.RemoveAt(index);
                }

                cardsForRand1.AddRange(cardsForRand2);
            }

            _cards.AddRange(cardsForRand2);
        }

        public IEnumerator<Card> GetEnumerator() => _cards.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
