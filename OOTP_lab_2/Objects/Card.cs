using System;

namespace OOTP_lab_2.Objects
{
    public class Card : IEquatable<Card>
    {
        public CardSuit Suit { get; }

        public CardNumber Number { get; }

        public Card(CardSuit suit, CardNumber number)
        {
            Suit = suit;
            Number = number;
        }

        public bool Equals(Card other)
        {
            if (other == null)
            {
                return false;
            }

            return Suit == other.Suit && Number == other.Number;
        }

        public override bool Equals(object obj) => Equals(obj as Card);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Suit * 397) ^ (int) Number;
            }
        }

        public override string ToString()
        {
            return $"{Number} {Suit}";
        }
    }
}
